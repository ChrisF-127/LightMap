using SyControlsBuilder;
using UnityEngine;
using Verse;

namespace LightMap
{
	public class LightMapSettings : ModSettings
	{
		#region CONSTANTS
		public const int LightMapGradientSteps = 3;
		public const int MovementSpeedMapGradientSteps = 3;
		public const int BeautyMapGradientSteps = 2;

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
			set => Util.SetValue(ref _overlayOpacity, value, v => LightMap.Instance.ResetMaps());
		}
		public int UpdateDelay { get; set; } = Default_UpdateDelay;


		public bool LightMapIconButtonVisible { get; set; } = Default_LightMapIconButtonVisible;

		public ValueSetting<float>[] LightMapGradientHue { get; } = new ValueSetting<float>[LightMapGradientSteps];

		private bool _lightMapShowRoofedOnly = Default_LightMapShowRoofedOnly;
		public bool LightMapShowRoofedOnly
		{
			get => _lightMapShowRoofedOnly;
			set => Util.SetValue(ref _lightMapShowRoofedOnly, value, v => LightMap.Instance.ResetMaps());
		}


		public bool MovementSpeedMapIconButtonVisible { get; set; } = Default_MovementSpeedMapIconButtonVisible;

		public ValueSetting<float>[] MovementSpeedMapGradientHue { get; } = new ValueSetting<float>[MovementSpeedMapGradientSteps];


		public bool BeautyMapIconButtonVisible { get; set; } = Default_BeautyMapIconButtonVisible;

		public ValueSetting<float>[] BeautyMapGradientHue { get; } = new ValueSetting<float>[BeautyMapGradientSteps];

		private bool _beautyMapUseAverage = Default_BeautyMapUseAverage;
		public bool BeautyMapUseAverage
		{
			get => _beautyMapUseAverage;
			set => Util.SetValue(ref _beautyMapUseAverage, value, v => LightMap.Instance.ResetMaps());
		}
		#endregion

		#region CONSTRUCTORS
		public LightMapSettings()
		{
			//   0° = red
			//  60° = yellow
			// 120° = green
			// 180° = cyan
			// 240° = blue
			// 300° = magenta

			LightMapGradientHue[0] = new ValueSetting<float>(nameof(LightMapGradientHue) + "_dark",   "", "",   0f,   0f, reset); // default: red
			LightMapGradientHue[1] = new ValueSetting<float>(nameof(LightMapGradientHue) + "_lit",	  "", "", 120f, 120f, reset); // default: green
			LightMapGradientHue[2] = new ValueSetting<float>(nameof(LightMapGradientHue) + "_bright", "", "", 180f, 180f, reset); // default: cyan

			MovementSpeedMapGradientHue[0] = new ValueSetting<float>(nameof(MovementSpeedMapGradientHue) + "_min",  "", "",   0f,   0f, reset); // default: red
			MovementSpeedMapGradientHue[1] = new ValueSetting<float>(nameof(MovementSpeedMapGradientHue) + "_max",  "", "", 120f, 120f, reset); // default: green
			MovementSpeedMapGradientHue[2] = new ValueSetting<float>(nameof(MovementSpeedMapGradientHue) + "_over", "", "", 180f, 180f, reset); // default: cyan

			BeautyMapGradientHue[0] = new ValueSetting<float>(nameof(BeautyMapGradientHue) + "_min", "", "", 300f, 300f, reset); // default: magenta
			BeautyMapGradientHue[1] = new ValueSetting<float>(nameof(BeautyMapGradientHue) + "_max", "", "", 540f, 540f, reset); // default: cyan

			void reset(float v) =>
				LightMap.Instance.ResetMaps();
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
				ControlsBuilder.CreateMultiNumeric(
					ref offsetY,
					width,
					"SY_LM.LightMapGradientHue".Translate(),
					"SY_LM.TooltipLightMapGradientHue".Translate() + "\n\n" + "SY_LM.TooltipHue".Translate(),
					LightMapGradientHue,
					nameof(LightMapGradientHue),
					unit: "°");
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
				ControlsBuilder.CreateMultiNumeric(
					ref offsetY,
					width,
					"SY_LM.MovementSpeedMapGradientHue".Translate(),
					"SY_LM.TooltipMovementSpeedMapGradientHue".Translate() + "\n\n" + "SY_LM.TooltipHue".Translate(),
					MovementSpeedMapGradientHue,
					nameof(MovementSpeedMapGradientHue),
					unit: "°");

				BeautyMapIconButtonVisible = ControlsBuilder.CreateCheckbox(
					ref offsetY,
					width,
					"SY_LM.BeautyMapIconButtonVisible".Translate(),
					"SY_LM.TooltipMapIconButtonVisible".Translate(),
					BeautyMapIconButtonVisible,
					Default_BeautyMapIconButtonVisible);
				ControlsBuilder.CreateMultiNumeric(
					ref offsetY,
					width,
					"SY_LM.BeautyMapGradientHue".Translate(),
					"SY_LM.TooltipBeautyMapGradientHue".Translate() + "\n\n" + "SY_LM.TooltipHue".Translate(),
					BeautyMapGradientHue,
					nameof(BeautyMapGradientHue),
					unit: "°");
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
			float floatValue;
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

			for (int i = 0; i < LightMapGradientSteps; i++)
			{
				var hue = LightMapGradientHue[i];
				floatValue = hue.Value;
				Scribe_Values.Look(ref floatValue, hue.Name, hue.DefaultValue);
				hue.Value = floatValue;
			}

			boolValue = MovementSpeedMapIconButtonVisible;
			Scribe_Values.Look(ref boolValue, nameof(MovementSpeedMapIconButtonVisible), Default_MovementSpeedMapIconButtonVisible);
			MovementSpeedMapIconButtonVisible = boolValue;

			for (int i = 0; i < MovementSpeedMapGradientSteps; i++)
			{
				var hue = MovementSpeedMapGradientHue[i];
				floatValue = hue.Value;
				Scribe_Values.Look(ref floatValue, hue.Name, hue.DefaultValue);
				hue.Value = floatValue;
			}

			boolValue = BeautyMapIconButtonVisible;
			Scribe_Values.Look(ref boolValue, nameof(BeautyMapIconButtonVisible), Default_BeautyMapIconButtonVisible);
			BeautyMapIconButtonVisible = boolValue;
			boolValue = BeautyMapUseAverage;
			Scribe_Values.Look(ref boolValue, nameof(BeautyMapUseAverage), Default_BeautyMapUseAverage);
			BeautyMapUseAverage = boolValue;

			for (int i = 0; i < BeautyMapGradientSteps; i++)
			{
				var hue = BeautyMapGradientHue[i];
				floatValue = hue.Value;
				Scribe_Values.Look(ref floatValue, hue.Name, hue.DefaultValue);
				hue.Value = floatValue;
			}
		}
		#endregion
	}
}
