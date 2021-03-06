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
		public virtual void Update(bool update)
		{
			Drawer.MarkForDraw();
			
			if (update)
				Drawer.SetDirty();

			Drawer.CellBoolDrawerUpdate();
		}
		#endregion
	}
}
