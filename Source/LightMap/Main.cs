using System.Collections.Generic;
using UnityEngine;
using RimWorld;
using RimWorld.Planet;
using Verse;
using HugsLib.Settings;
using HugsLib.Utils;
using LightMap.Overlays;

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
		private LightOverlay _lightMap;

		public bool ShowPathMap;
		private PathOverlay _pathMap;

		public bool ShowBeautyMap;
		private BeautyOverlay _beautyMap;

		private SettingHandle<int> _opacity;
		private SettingHandle<int> _updateDelay;
		private SettingHandle<bool> _lightMapShowRoofedOnly;
		private SettingHandle<bool> _beautyMapUseAverage;
		#endregion

		#region PROPERTIES
		internal static Main Instance { get; private set; }

		public override string ModIdentifier => "LightMap";
		#endregion

		#region PUBLIC METHODS
		public void UpdateMaps()
		{
			var tick = Find.TickManager.TicksGame;
			var delay = _updateDelay;

			if (ShowLightMap)
			{
				if (_lightMap == null)
					_lightMap = new LightOverlay();
				_lightMap.Update(tick, delay);
			}

			if (ShowPathMap)
			{
				if (_pathMap == null)
					_pathMap = new PathOverlay();
				_pathMap.Update(tick, delay);
			}

			if (ShowBeautyMap)
			{
				if (_beautyMap == null)
					_beautyMap = new BeautyOverlay(_beautyMapUseAverage);
				_beautyMap.Update(tick, delay);
			}
		}

		public void ResetMaps()
		{
			_lightMap = null;
			_pathMap = null;
			_beautyMap = null;
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
				|| WorldRendererUtility.WorldRenderedNow)
                return;

			if (Event.current.type != EventType.KeyDown 
				|| Event.current.keyCode == KeyCode.None)
                return;

            if (LightMapKeyBingings.ToggleLightMap.JustPressed)
                ShowLightMap = !ShowLightMap;
			if (LightMapKeyBingings.TogglePathMap.JustPressed)
				ShowPathMap = !ShowPathMap;
			if (LightMapKeyBingings.ToggleBeautyMap.JustPressed)
				ShowBeautyMap = !ShowBeautyMap;
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
				ResetMaps();

			_updateDelay = Settings.GetHandle(
				"updateDelay",
                "Update delay", // TODO translatable string
				"Update interval for the overlay", // TODO translatable string
				100,
                Validators.IntRangeValidator(1, 10000));

			_lightMapShowRoofedOnly = Settings.GetHandle(
				"lightMapShowRoofedOnly",
				"Light Map: roofed areas only", // TODO translatable string
				"Only show brightness overlay for roofed areas", // TODO translatable string
				false);
			_lightMapShowRoofedOnly.OnValueChanged = val =>
				ResetMaps();

			_beautyMapUseAverage = Settings.GetHandle(
				"beautyMapUseAverage",
				"Beauty Map: use average beauty; WARNING!", // TODO translatable string
				">> WARNING <<" +
					"\nThis mode is VERY VERY SLOW, but calculates the average beauty per tile as used for the beauty need of pawns." +
					"\n" +
					"\nYou will not be able to unpause the game while this overlay is active!", // TODO translatable string
				false);
			_beautyMapUseAverage.OnValueChanged = val =>
				ResetMaps();
		}
		#endregion
	}
}
