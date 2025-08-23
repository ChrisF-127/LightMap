using UnityEngine;
using Verse;

namespace LightMap.Overlays
{
	public class LightOverlay : OverlayBase
	{
		#region FIELDS
		private readonly Color[] _mappedColors = new Color[12];
		private readonly bool _showRoofedOnly;
		#endregion

		#region CONSTRUCTORS
		public LightOverlay()
		{
			CreateMappedColors();

			_showRoofedOnly = LightMap.Settings.LightMapShowRoofedOnly;
		}
		#endregion

		#region PUBLIC METHODS
		private void CreateMappedColors()
		{
			var hues = LightMap.Settings.LightMapGradientHue;
			var bright = hues[2].Value.ToColor();				// considered lit or brightly lit, light level for growing most plants
			var litHue = hues[1].Value;
			var darkHue = hues[0].Value;
			var dark = darkHue.ToColor();						// considered dark

			var step = ColorExtensions.GetHueStepWidth(litHue, darkHue, 2 * 3); // lit -> dark; halfway between lit/dark colors & three steps for bigger difference between 50% and 40%

			_mappedColors[11] = new Color(1f, 1f, 1f);			// >=100% brightly lit - white
			_mappedColors[10] = bright.ToWhite(0.85f);			// >  90% brightly lit
			_mappedColors[9] = bright.ToWhite(0.6f);			// >  80% lit
			_mappedColors[8] = bright.ToWhite(0.4f);			// >  70% lit
			_mappedColors[7] = bright.ToWhite(0.2f);			// >  60% lit
			_mappedColors[6] = bright;							// >  50% lit - default: cyan (setting)

			_mappedColors[5] = litHue.ToColor();				//  = 50% lit - default: green (setting)
			_mappedColors[4] = (darkHue + step * 4).ToColor();	// >  40% lit
			_mappedColors[3] = (darkHue + step * 3).ToColor();	// >  30% lit - default: yellow

			_mappedColors[2] = dark;							// >  20% dark - default: red (setting)
			_mappedColors[1] = dark.ToBlack(0.5f);				// >  10% dark
			_mappedColors[0] = new Color(0f, 0f, 0f);			// >=  0% dark - black
		}
		#endregion

		#region PRIVATE METHODS
		private Color GetColorForGlow(float glow)
		{
			var index = (int)(glow * 10.0f);
			if (glow > 0.5f)
			{
				index++;
				if (index >= _mappedColors.Length)
					index = _mappedColors.Length;
			}
			else
			{
				if (index < 0)
					index = 0;
			}
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