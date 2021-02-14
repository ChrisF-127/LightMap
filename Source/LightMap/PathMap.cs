using System;
using UnityEngine;
using RimWorld;
using Verse;

namespace LightMap
{
	public class PathMap : MapOverlayBase
	{
		public PathMap()
		{
			CreateMappedColors();
		}

		#region FIELDS
		private Color[] _mappedColors = null;
		#endregion

		#region PROPERTIES
		public override bool ShowMap => Main.Instance.ShowPathMap;
		#endregion

		#region PUBLIC METHODS
		public void CreateMappedColors()
		{
			_mappedColors = new Color[13];

			_mappedColors[12] = new Color	(0,		0.25f,	1); // > 100%; blue-ish
			_mappedColors[11] = new Color	(1,		1,		1); // = 100%; white

			_mappedColors[10] = new Color	(0,		1,		0); // < 100%; green
			_mappedColors[9] = new Color	(0.25f, 1,		0); // < 90%
			_mappedColors[8] = new Color	(0.5f,	1,		0); // < 80%
			_mappedColors[7] = new Color	(0.75f, 1,		0); // < 70%
			_mappedColors[6] = new Color	(1,		1,		0); // < 60%; yellow
			_mappedColors[5] = new Color	(1,		0.8f,	0); // < 50%
			_mappedColors[4] = new Color	(1,		0.6f,	0); // < 40%
			_mappedColors[3] = new Color	(1,		0.4f,	0); // < 30%
			_mappedColors[2] = new Color	(1,		0.2f,	0); // < 20%
			_mappedColors[1] = new Color	(1,		0,		0); // < 10%; red

			_mappedColors[0] = new Color	(0,		0,		0); // no path; black
		}

		public Color GetColorForValue(int pathCost)
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

			var path = map.pathGrid.pathGrid[index];
			_nextColor = GetColorForValue(path);
			return true;
		}
		#endregion
	}
}
