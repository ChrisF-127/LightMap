using System;
using UnityEngine;
using RimWorld;
using Verse;

namespace LightMap.Overlays
{
    public class LightOverlay : OverlayBase
	{
        public LightOverlay()
        {
			CreateMappedColors();

			_showRoofedOnly = Main.Instance.GetConfiguredShowRoofedOnly();
		}

		#region FIELDS
		private Color[] _mappedColors = null;
		private readonly bool _showRoofedOnly;
		#endregion

		#region PROPERTIES
		#endregion

		#region PRIVATE METHODS
		private void CreateMappedColors()
		{
			_mappedColors = new Color[11];

			_mappedColors[10] = new Color(1, 1, 1);
			_mappedColors[9] = new Color(0.75f, 1, 1);

			_mappedColors[8] = new Color(0, 1, 1);
			_mappedColors[7] = new Color(0, 1, 0.75f);
			_mappedColors[6] = new Color(0, 1, 0.5f);
			_mappedColors[5] = new Color(0, 1, 0);

			_mappedColors[4] = new Color(0.75f, 1, 0);
			_mappedColors[3] = new Color(1, 1, 0);

			_mappedColors[2] = new Color(1, 0, 0);
			_mappedColors[1] = new Color(0.5f, 0, 0);
			_mappedColors[0] = new Color(0, 0, 0);
		}

		private Color GetColorForGlow(float glow)
		{
			int index = (int)(glow * 10.0);
			if (index > 10)
				index = 10;
			else if (index < 0)
				index = 0;
			return _mappedColors[index];
		}
		#endregion

		#region INTERFACES
		public override bool GetCellBool(int index)
		{
			var map = Find.CurrentMap;
            if (map.fogGrid.IsFogged(index))
                return false;

			var cell = map.cellIndices.IndexToCell(index);
			if (!_showRoofedOnly)
			{
				var glow = map.glowGrid.GroundGlowAt(cell);
				_nextColor = GetColorForGlow(glow);
				return true;
			}
			else
			{
				var roof = cell.GetRoof(map);
				if (roof != null)
				{
					var glow = map.glowGrid.GroundGlowAt(cell);
					_nextColor = GetColorForGlow(glow);
					return true;
				}

				// logic for checking for rooms (old)
				//var room = map.cellIndices.IndexToCell(index).GetRoom(map, RegionType.Set_All);
				//if (room != null && !room.PsychologicallyOutdoors)
				//{
				//	var glow = map.glowGrid.GameGlowAt(map.cellIndices.IndexToCell(index));
				//	_nextColor = GetColorForTemperature(glow);
				//	return true;
				//}
			}
            return false;
		}
		#endregion
	}
}