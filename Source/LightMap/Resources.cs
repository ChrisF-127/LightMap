using UnityEngine;
using Verse;

namespace LightMap
{
    [StaticConstructorOnStartup]
    public class Resources
    {
        public static Texture2D IconLight = ContentFinder<Texture2D>.Get("LightMap");
		public static Texture2D IconPath = ContentFinder<Texture2D>.Get("PathMap");
		public static Texture2D IconBeauty = ContentFinder<Texture2D>.Get("BeautyMap");
	}
}