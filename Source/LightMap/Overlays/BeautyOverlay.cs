using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RimWorld;
using Verse;
using System.Runtime.InteropServices;

namespace LightMap.Overlays
{
	/*
	 * Todo
	 * - toggle between tile base beauty or 
	 */

	public class BeautyOverlay : OverlayBase
	{
		public BeautyOverlay()
		{
			CreateMappedColors();

			if (false)
			{
				_beautyCalculation = (root, map) => BeautyUtility.CellBeauty(root, map);
			}
			else
			{
				_beautyCalculation = (root, map) =>
				{
					float beauty = 0f;
					int count = 0;
					BeautyUtility.FillBeautyRelevantCells(root, map);
					foreach (var cell in BeautyUtility.beautyRelevantCells)
					{
						beauty += BeautyUtility.CellBeauty(cell, map, _countedThingList);
						count++;
					}
					_countedThingList.Clear();
					return count == 0 ? float.MinValue : beauty / count;
				};
			}

			Update(true);
		}

		#region FIELDS
		private Color[] _mappedColors = null;
		
		private readonly float _beautyFactor = 1f / 1f;
		private readonly List<Thing> _countedThingList = new List<Thing>();

		private readonly Func<IntVec3, Map, float> _beautyCalculation = null;
		#endregion

		#region PROPERTIES
		#endregion

		#region PRIVATE METHODS
		private void CreateMappedColors()
		{
			_mappedColors = new Color[22];

			_mappedColors[21] = new Color(0f, 0f, 0f, 0f);

			var pink = new Color(1f, 0f, 1f);
			var deg = (240f / (_mappedColors.Length - 1));
			for (int i = 0; i < 21; i++)
				_mappedColors[i] = pink.ChangeHue(i * deg);
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

			int index = (int)(value * _beautyFactor) + 10;
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
		#endregion
	}
}
