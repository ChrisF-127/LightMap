using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace LightMap.Overlays
{
	public abstract class OverlayBase : ICellBoolGiver
	{
		#region FIELDS
		protected CellBoolDrawer _drawerInt = null;
		protected Color _nextColor;

		protected int _nextUpdateTick = 0;
		#endregion

		#region PROPERTIES
		public virtual Color Color => Color.white;

		public virtual CellBoolDrawer Drawer
		{
			get
			{
				if (_drawerInt == null)
				{
					var map = Find.CurrentMap;
					_drawerInt = new CellBoolDrawer(this, map.Size.x, map.Size.z, Main.Instance.GetConfiguredOpacity());
				}
				return _drawerInt;
			}
		}
		#endregion

		#region INTERFACES
		public abstract bool GetCellBool(int index);
		
		public virtual Color GetCellExtraColor(int index) =>
			_nextColor;
		#endregion

		#region PUBLIC METHODS
		public virtual void Update(int tick, int delay)
		{
			Drawer.MarkForDraw();
			
			if (tick >= _nextUpdateTick)
			{
				Drawer.SetDirty();

				_nextUpdateTick = tick + delay;
			}

			Drawer.CellBoolDrawerUpdate();
		}
		#endregion
	}
}
