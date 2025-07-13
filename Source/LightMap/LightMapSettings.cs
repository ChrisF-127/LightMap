using LightMap.Overlays;
using RimWorld.Planet;
using SyControlsBuilder;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace LightMap
{
	public class LightMapSettings : ModSettings
	{
		#region CONSTANTS
		public const int Default_OverlayOpacity = 30;
		public const int Default_UpdateDelay = 100;

		public const bool Default_LightMapIconButtonVisible = true;
		public const bool Default_LightMapShowRoofedOnly = false;

		public const bool Default_MovementSpeedMapIconButtonVisible = false;

		public const bool Default_BeautyMapIconButtonVisible = false;
		public const bool Default_BeautyMapUseAverage = false;
		#endregion

		#region PROPERTIES
		private int _overlayOpacity = Default_OverlayOpacity;
		public int OverlayOpacity
		{
			get => _overlayOpacity;
			set => Util.SetValue(ref _overlayOpacity, value, LightMap.Instance.ResetMaps);
		}
		public int UpdateDelay { get; set; } = Default_UpdateDelay;

		public bool LightMapIconButtonVisible { get; set; } = Default_LightMapIconButtonVisible;
		private bool _lightMapShowRoofedOnly = Default_LightMapShowRoofedOnly;
		public bool LightMapShowRoofedOnly
		{
			get => _lightMapShowRoofedOnly;
			set => Util.SetValue(ref _lightMapShowRoofedOnly, value, LightMap.Instance.ResetMaps);
		}

		public bool MovementSpeedMapIconButtonVisible { get; set; } = Default_MovementSpeedMapIconButtonVisible;

		public bool BeautyMapIconButtonVisible { get; set; } = Default_BeautyMapIconButtonVisible;
		private bool _beautyMapUseAverage = Default_BeautyMapUseAverage;
		public bool BeautyMapUseAverage
		{
			get => _beautyMapUseAverage;
			set => Util.SetValue(ref _beautyMapUseAverage, value, LightMap.Instance.ResetMaps);
		}
		#endregion

		#region PUBLIC METHODS
		public void DoSettingsWindowContents(Rect inRect)
		{
			var width = inRect.width;
			var offsetY = 0.0f;

			ControlsBuilder.Begin(inRect);
			try
			{
				OverlayOpacity = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_LM.Opacity".Translate(),
					"SY_LM.TooltipOpacity".Translate(),
					OverlayOpacity,
					Default_OverlayOpacity,
					nameof(OverlayOpacity),
					1,
					100,
					unit: "%");
				UpdateDelay = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_LM.UpdateDelay".Translate(),
					"SY_LM.TooltipUpdateDelay".Translate(),
					UpdateDelay,
					Default_UpdateDelay,
					nameof(UpdateDelay),
					1,
					10000,
					unit: "ticks");

				LightMapIconButtonVisible = ControlsBuilder.CreateCheckbox(
					ref offsetY,
					width,
					"SY_LM.LightMapIconButtonVisible".Translate(),
					"SY_LM.TooltipMapIconButtonVisible".Translate(),
					LightMapIconButtonVisible,
					Default_LightMapIconButtonVisible);
				LightMapShowRoofedOnly = ControlsBuilder.CreateCheckbox(
					ref offsetY,
					width,
					"SY_LM.LightMapRoofedAreasOnly".Translate(),
					"SY_LM.TooltipLightMapRoofedAreasOnly".Translate(),
					LightMapShowRoofedOnly,
					Default_LightMapShowRoofedOnly);

				MovementSpeedMapIconButtonVisible = ControlsBuilder.CreateCheckbox(
					ref offsetY,
					width,
					"SY_LM.MovementSpeedMapIconButtonVisible".Translate(),
					"SY_LM.TooltipMapIconButtonVisible".Translate(),
					MovementSpeedMapIconButtonVisible,
					Default_MovementSpeedMapIconButtonVisible);

				BeautyMapIconButtonVisible = ControlsBuilder.CreateCheckbox(
					ref offsetY,
					width,
					"SY_LM.BeautyMapIconButtonVisible".Translate(),
					"SY_LM.TooltipMapIconButtonVisible".Translate(),
					BeautyMapIconButtonVisible,
					Default_BeautyMapIconButtonVisible);
				BeautyMapUseAverage = ControlsBuilder.CreateCheckbox(
					ref offsetY,
					width,
					"SY_LM.BeautyMapAverageBeauty".Translate(),
					"SY_LM.TooltipBeautyMapAverageBeauty".Translate(),
					BeautyMapUseAverage,
					Default_BeautyMapUseAverage);
			}
			finally
			{
				ControlsBuilder.End(offsetY);
			}
		}
		#endregion

		#region OVERRIDES
		public override void ExposeData()
		{
			base.ExposeData();

			bool boolValue;
			int intValue;

			intValue = OverlayOpacity;
			Scribe_Values.Look(ref intValue, nameof(OverlayOpacity), Default_OverlayOpacity);
			OverlayOpacity = intValue;
			intValue = UpdateDelay;
			Scribe_Values.Look(ref intValue, nameof(UpdateDelay), Default_UpdateDelay);
			UpdateDelay = intValue;

			boolValue = LightMapIconButtonVisible;
			Scribe_Values.Look(ref boolValue, nameof(LightMapIconButtonVisible), Default_LightMapIconButtonVisible);
			LightMapIconButtonVisible = boolValue;
			boolValue = LightMapShowRoofedOnly;
			Scribe_Values.Look(ref boolValue, nameof(LightMapShowRoofedOnly), Default_LightMapShowRoofedOnly);
			LightMapShowRoofedOnly = boolValue;

			boolValue = MovementSpeedMapIconButtonVisible;
			Scribe_Values.Look(ref boolValue, nameof(MovementSpeedMapIconButtonVisible), Default_MovementSpeedMapIconButtonVisible);
			MovementSpeedMapIconButtonVisible = boolValue;

			boolValue = BeautyMapIconButtonVisible;
			Scribe_Values.Look(ref boolValue, nameof(BeautyMapIconButtonVisible), Default_BeautyMapIconButtonVisible);
			BeautyMapIconButtonVisible = boolValue;
			boolValue = BeautyMapUseAverage;
			Scribe_Values.Look(ref boolValue, nameof(BeautyMapUseAverage), Default_BeautyMapUseAverage);
			BeautyMapUseAverage = boolValue;
		}
		#endregion
	}
}
