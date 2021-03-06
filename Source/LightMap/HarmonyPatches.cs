using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace LightMap
{
	[HarmonyPatch(typeof(MapInterface), nameof(MapInterface.MapInterfaceUpdate))]
	internal static class MapInterface_MapInterfaceUpdate
	{
		[HarmonyPostfix]
		static void Postfix()
		{
			if (Find.CurrentMap == null 
				|| WorldRendererUtility.WorldRenderedNow)
				return;

			Main.Instance.UpdateMaps();
		}
	}

	[HarmonyPatch(typeof(MapInterface), nameof(MapInterface.Notify_SwitchedMap))]
	internal static class MapInterface_Notify_SwitchedMap
	{
		[HarmonyPostfix]
		static void Postfix()
		{
			Main.Instance.ResetMaps();
		}
	}


	[HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
	internal static class PlaySettings_DoPlaySettingsGlobalControls
	{
		[HarmonyPostfix]
		static void PostFix(WidgetRow row, bool worldView)
		{
			if (worldView || row == null)
				return;

			if (Resources.IconLight != null)
				row.ToggleableIcon(
					ref Main.Instance.ShowLightMap, 
					Resources.IconLight, 
					"Show Light Map", // TODO translatable string
					SoundDefOf.Mouseover_ButtonToggle);

			if (Resources.IconPath != null)
				row.ToggleableIcon(
					ref Main.Instance.ShowPathMap, 
					Resources.IconPath, 
					"Show Movement Speed Map", // TODO translatable string
					SoundDefOf.Mouseover_ButtonToggle);

			if (Resources.IconBeauty != null)
				row.ToggleableIcon(
					ref Main.Instance.ShowBeautyMap,
					Resources.IconBeauty,
					"Show Beauty Map", // TODO translatable string
					SoundDefOf.Mouseover_ButtonToggle);
		}
	}



	// Was testing to see if I could make pathCost accept negative values, 
	//  but sadly the pathCost also resets to 0+ when there's any object, including filth, on the map tile.
	//[HarmonyPatch(typeof(SnowUtility), nameof(SnowUtility.MovementTicksAddOn))]
	//internal static class SnowUtility_Patch
	//{
	//	[HarmonyPostfix]
	//	static void PostFix(SnowCategory category, ref int __result)
	//	{
	//		if (__result <= 0)
	//			__result = -100;
	//	}
	//}
}