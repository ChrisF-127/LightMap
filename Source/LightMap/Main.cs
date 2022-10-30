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
		public SettingHandle<bool> LightMapIconButtonVisible;
		private SettingHandle<bool> _lightMapShowRoofedOnly;
		public SettingHandle<bool> MovementSpeedMapIconButtonVisible;
		public SettingHandle<bool> BeautyMapIconButtonVisible;
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

			if (_lightMap == null)
				_lightMap = new LightOverlay();
			_lightMap.Update(tick, delay, ShowLightMap);

			if (_pathMap == null)
				_pathMap = new PathOverlay();
			_pathMap.Update(tick, delay, ShowPathMap);

			if (_beautyMap == null)
				_beautyMap = new BeautyOverlay(_beautyMapUseAverage);
			_beautyMap.Update(tick, delay, ShowBeautyMap);
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
				"SY_LM.Opacity".Translate(),
				"SY_LM.TooltipOpacity".Translate(),
				30,
                Validators.IntRangeValidator(1, 100));
			_opacity.ValueChanged += val =>
				ResetMaps();

			_updateDelay = Settings.GetHandle(
				"updateDelay",
                "SY_LM.UpdateDelay".Translate(),
				"SY_LM.TooltipUpdateDelay".Translate(),
				100,
                Validators.IntRangeValidator(1, 10000));


			LightMapIconButtonVisible = Settings.GetHandle(
				"lightMapIconButtonVisible",
				"SY_LM.LightMapIconButtonVisible".Translate(),
				"SY_LM.TooltipMapIconButtonVisible".Translate(),
				true);

			_lightMapShowRoofedOnly = Settings.GetHandle(
				"lightMapShowRoofedOnly",
				"SY_LM.LightMapRoofedAreasOnly".Translate(),
				"SY_LM.TooltipLightMapRoofedAreasOnly".Translate(),
				false);
			_lightMapShowRoofedOnly.ValueChanged += val =>
				ResetMaps();


			MovementSpeedMapIconButtonVisible = Settings.GetHandle(
				"movementSpeedMapIconButtonVisible",
				"SY_LM.MovementSpeedMapIconButtonVisible".Translate(),
				"SY_LM.TooltipMapIconButtonVisible".Translate(),
				false);


			BeautyMapIconButtonVisible = Settings.GetHandle(
				"beautyMapIconButtonVisible",
				"SY_LM.BeautyMapIconButtonVisible".Translate(),
				"SY_LM.TooltipMapIconButtonVisible".Translate(),
				false);

			_beautyMapUseAverage = Settings.GetHandle(
				"beautyMapUseAverage",
				"SY_LM.BeautyMapAverageBeauty".Translate(),
				"SY_LM.TooltipBeautyMapAverageBeauty".Translate(),
				false);
			_beautyMapUseAverage.ValueChanged += val =>
				ResetMaps();
		}
		#endregion
	}
}
