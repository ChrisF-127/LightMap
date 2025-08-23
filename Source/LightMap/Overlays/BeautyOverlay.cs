using System;
using UnityEngine;
using RimWorld;
using Verse;

namespace LightMap.Overlays
{
	public class BeautyOverlay : OverlayBase
	{
		public BeautyOverlay(bool useAverage)
		{
			CreateMappedColors();

			_useAverage = useAverage;
			if (!_useAverage)
			{
				_beautyFactor = 10f;
				_beautyCalculation = (root, map) => BeautyUtility.CellBeauty(root, map);
			}
			else // calculate average beauty; used for pawn beauty need but VERY VERY SLOW !
			{
				_beautyFactor = 1f;
				_beautyCalculation = (root, map) => BeautyUtility.AverageBeautyPerceptible(root, map);
			}
		}

		#region FIELDS
		private readonly Color[] _mappedColors = new Color[22];

		private readonly bool _useAverage = false;
		private readonly float _beautyFactor = 1f;

		private readonly Func<IntVec3, Map, float> _beautyCalculation = null;
		#endregion

		#region PROPERTIES
		#endregion

		#region PRIVATE METHODS
		private void CreateMappedColors()
		{
			_mappedColors[21] = new Color(0f, 0f, 0f, 0f);

			var hues = LightMap.Settings.BeautyMapGradientHue;
			var max = hues[1].Value;
			var min = hues[0].Value;
			var step = ColorExtensions.GetHueStepWidth(max, min, _mappedColors.Length - 1);

			for (int i = 0; i < _mappedColors.Length - 1; i++)
				_mappedColors[i] = (min + step * i).ToColor();
		}

		private float GetBeautyForIndex(int index, Map map)
		{
			IntVec3 root = map.cellIndices.IndexToCell(index);
			return !root.IsValid /*|| !cell.InBounds(map)*/ ? float.MinValue : _beautyCalculation(root, map);
		}

		private Color GetColorForBeauty(float value)
		{
			if (value == float.MinValue)
				return _mappedColors[21];

			int index = (int)(value / _beautyFactor) + 10;
			return _mappedColors[index < 0 ? 0 : index > 20 ? 20 : index];
		}
		#endregion

		#region INTERFACES
		public override bool GetCellBool(int index)
		{
			var map = Find.CurrentMap;
			if (map.fogGrid.IsFogged(index))
				return false;
			
			var beauty = GetBeautyForIndex(index, map);
			_nextColor = GetColorForBeauty(beauty);
			return true;
		}

		public override void Update(int tick, int delay, bool show)
		{
			base.Update(tick, delay, show);

			// Pause the game because we'd not want this to be executed more than once.
			if (show && _useAverage)
				Find.TickManager.Pause();
		}
		#endregion
	}
}
