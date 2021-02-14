using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace LightMap
{
	public abstract class MapOverlayBase : ICellBoolGiver
	{
		#region FIELDS
		protected CellBoolDrawer _drawerInt = null;
		protected int _nextUpdateTick = 0;
		protected Color _nextColor;
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

		public virtual bool ShowMap { get; }
		#endregion

		#region INTERFACES
		public virtual bool GetCellBool(int index) =>
			throw new NotImplementedException();
		
		public virtual Color GetCellExtraColor(int index) =>
			_nextColor;
		#endregion

		#region PUBLIC METHODS
		public virtual void Update(int updateDelay)
		{
			if (ShowMap)
			{
				Drawer.MarkForDraw();

				var tick = Find.TickManager.TicksGame;
				if (tick >= _nextUpdateTick)
				{
					Drawer.SetDirty();
					_nextUpdateTick = tick + updateDelay;
				}

				Drawer.CellBoolDrawerUpdate();
			}
		}

		public virtual void Reset()
		{
			_drawerInt = null;
			_nextUpdateTick = 0;
		}
		#endregion
	}
}
