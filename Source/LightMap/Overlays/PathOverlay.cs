using UnityEngine;
using Verse;

namespace LightMap.Overlays
{
	public class PathOverlay : OverlayBase
	{
		#region FIELDS
		private readonly Color[] _mappedColors = new Color[13];
		#endregion

		#region CONSTRUCTORS
		public PathOverlay()
		{
			CreateMappedColors();
		}
		#endregion

		#region PRIVATE METHODS
		private void CreateMappedColors()
		{
			var hues = LightMap.Settings.MovementSpeedMapGradientHue;
			var over = hues[2].Value.ToColor();
			var maxHue = hues[1].Value;
			var max = maxHue.ToColor();
			var minHue = hues[0].Value;
			var min = minHue.ToColor();
			if (maxHue < minHue)
				maxHue += 360f;
			var step = (maxHue - minHue) / 9f;			// min -> max

			_mappedColors[12] = over;					// > 100% - default: cyan (setting)
			_mappedColors[11] = new Color(1f, 1f, 1f);	// = 100% -  white

			_mappedColors[10] = max;					// < 100% - default: green (setting)
			for (int i = 9; i > 1; i--)					// <  90% to <  20%
				_mappedColors[i] = min.ChangeHue(step * (i - 1));
			_mappedColors[1] = min;                     // <  10% - default: red (setting)

			_mappedColors[0] = new Color(0f, 0f, 0f);	// no path -  black
		}

		private Color GetColorForPathCost(int pathCost)
		{
			if (pathCost == 10000) // no path
				return _mappedColors[0]; 
			if (pathCost < 0) // > 100% - unless the calculation gets changed, pathCost will never be < 0
				return _mappedColors[12];

			var speedPercent = 13f / (pathCost + 13f);
			int index = Mathf.CeilToInt((speedPercent + 0.001f) * 10f);
			return _mappedColors[index];
		}
		#endregion

		#region INTERFACES
		public override bool GetCellBool(int index)
		{
			var map = Find.CurrentMap;
			if (map.fogGrid.IsFogged(index))
				return false;

			var path = map.pathing.Normal.pathGrid.pathGrid[index];
			_nextColor = GetColorForPathCost(path);
			return true;
		}
		#endregion
	}
}
