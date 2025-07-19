using System.Collections.Generic;
using UnityEngine;
using RimWorld;
using RimWorld.Planet;
using Verse;
using LightMap.Overlays;

namespace LightMap
{
    public class LightMap : Mod
	{
		#region PROPERTIES
		public static LightMap Instance { get; private set; }
		public static LightMapSettings Settings { get; private set; }
		#endregion

		#region FIELDS
		public bool ShowLightMap;
		private LightOverlay _lightMap;

		public bool ShowPathMap;
		private PathOverlay _pathMap;

		public bool ShowBeautyMap;
		private BeautyOverlay _beautyMap;
		#endregion

		#region CONSTRUCTORS
		public LightMap(ModContentPack content) : base(content)
		{
			Instance = this;

			LongEventHandler.ExecuteWhenFinished(Initialize);
		}
		#endregion

		#region PUBLIC METHODS
		public void OnGUI()
		{
			if (Current.ProgramState != ProgramState.Playing
				|| Find.CurrentMap == null
				|| !WorldRendererUtility.DrawingMap)
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

		public void WorldLoaded()
		{
			ResetMaps();
		}

		public void UpdateMaps()
		{
			if (Current.ProgramState != ProgramState.Playing
				|| Find.CurrentMap == null
				|| !WorldRendererUtility.DrawingMap)
				return;

			var tick = Find.TickManager.TicksGame;
			var delay = Settings.UpdateDelay;

			if (_lightMap == null)
				_lightMap = new LightOverlay();
			_lightMap.Update(tick, delay, ShowLightMap);

			if (_pathMap == null)
				_pathMap = new PathOverlay();
			_pathMap.Update(tick, delay, ShowPathMap);

			if (_beautyMap == null)
				_beautyMap = new BeautyOverlay(Settings.BeautyMapUseAverage);
			_beautyMap.Update(tick, delay, ShowBeautyMap);
		}

		public void ResetMaps()
		{
			_lightMap = null;
			_pathMap = null;
			_beautyMap = null;
		}
		#endregion

		#region OVERRIDES
		public override string SettingsCategory() =>
			"Light Map";

		public override void DoSettingsWindowContents(Rect inRect)
		{
			base.DoSettingsWindowContents(inRect);

			Settings.DoSettingsWindowContents(inRect);
		}
		#endregion

		#region PRIVATE METHODS
		private void Initialize()
		{
			Settings = GetSettings<LightMapSettings>();
		}
		#endregion
	}
}
