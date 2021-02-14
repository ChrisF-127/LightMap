using System.Collections.Generic;
using UnityEngine;
using RimWorld;
using RimWorld.Planet;
using Verse;
using HugsLib.Settings;
using HugsLib.Utils;

namespace LightMap
{
    public class Main : HugsLib.ModBase
    {
        public Main()
        {
            Instance = this;
        }

		#region FIELDS
		public bool ShowLightMap;
		private LightMap _lightMap;

		public bool ShowPathMap;
		private PathMap _pathMap;

		private SettingHandle<int> _opacity;
		private SettingHandle<int> _updateDelay;
		private SettingHandle<bool> _lightMapShowRoofedOnly;
		#endregion

		#region PROPERTIES
		internal static Main Instance { get; private set; }

		public override string ModIdentifier => "LightMap";
		#endregion

		#region PUBLIC METHODS
		public void UpdateMaps()
        {
            if (_lightMap == null)
                _lightMap = new LightMap();
			if (_pathMap == null)
				_pathMap = new PathMap();

			_lightMap.Update(_updateDelay);
			_pathMap.Update(_updateDelay);
		}

		public void ResetMaps()
		{
			_lightMap = null;
			_pathMap = null;
		}

		public float GetConfiguredOpacity() => 
			_opacity * 0.01f;

		public bool GetConfiguredShowRoofedOnly() => 
			_lightMapShowRoofedOnly;
		#endregion

		#region INTERFACES
		public override void OnGUI()
        {
            if (Current.ProgramState != ProgramState.Playing 
				|| Find.CurrentMap == null 
				|| WorldRendererUtility.WorldRenderedNow 
				|| _lightMap == null
				|| _pathMap == null)
                return;

			if (Event.current.type != EventType.KeyDown 
				|| Event.current.keyCode == KeyCode.None)
                return;

            if (LightMapKeyBingings.ToggleLightMap.JustPressed)
            {
                if (WorldRendererUtility.WorldRenderedNow)
                    return;
                ShowLightMap = !ShowLightMap;
			}

			if (LightMapKeyBingings.TogglePathMap.JustPressed)
			{
				if (WorldRendererUtility.WorldRenderedNow)
					return;
				ShowPathMap = !ShowPathMap;
			}
		}

        public override void WorldLoaded()
		{
			ResetMaps();
        }

		public override void DefsLoaded()
        {
            _opacity = Settings.GetHandle(
                "opacity", 
				"Opacity", // TODO translatable string
				"Set the overlay opacity", // TODO translatable string
				30,
                Validators.IntRangeValidator(1, 100));
			_opacity.OnValueChanged = val =>
			{
				_lightMap?.Reset();
				_pathMap?.Reset();
			};

			_updateDelay = Settings.GetHandle(
				"updateDelay",
                "Update delay", // TODO translatable string
				"Update interval for the overlay", // TODO translatable string
				100,
                Validators.IntRangeValidator(1, 9999));

			_lightMapShowRoofedOnly = Settings.GetHandle(
				"lightMapShowRoofedOnly",
				"Light Map: roofed areas only", // TODO translatable string
				"Only show brightness overlay for roofed areas", // TODO translatable string
				true);
			_lightMapShowRoofedOnly.OnValueChanged = val =>
				_lightMap?.Reset();
		}
		#endregion
	}
}
