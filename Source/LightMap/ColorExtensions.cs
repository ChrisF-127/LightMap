using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LightMap
{
	public static class ColorExtensions
	{
		public static Color ChangeLightness(this Color color, float coeff)
		{
			ColorToHSV(color, out float h, out float s, out float v);
			return HSVToColor(color, h, s, v * coeff);
		}

		public static Color ChangeHue(this Color color, float hueChangeInDegree)
		{
			ColorToHSV(color, out float h, out float s, out float v);
			h += hueChangeInDegree;
			return HSVToColor(color, h, s, v);
		}

		public static void ColorToHSV(this Color color, out float h, out float s, out float v)
		{
			if (color == null)
				throw new Exception("Parameter error!");

			float r = color.r;
			float g = color.g;
			float b = color.b;

			float min = Min(r, g, b);
			float max = Max(r, g, b);
			float delta = max - min;

			v = max;
			if (delta < 0.00001f)
			{
				s = 0f;
				h = 0f;
				return;
			}

			if (max > 0.0f)
			{
				s = (delta / max);
			}
			else
			{
				s = 0.0f;
				h = 0.0f;
				return;
			}

			if (r >= max)
				h = (g - b) / delta;
			else if (g >= max)
				h = 2.0f + (b - r) / delta;
			else
				h = 4.0f + (r - g) / delta;

			h *= 60.0f;

			if (h < 0.0f)
				h += 360.0f;
		}

		public static Color HSVToColor(this Color color, float h, float s, float v)
		{
			if (color == null)
				throw new Exception("Parameter error!");

			s = Limit(ref s, 0, 1);
			v = Limit(ref v, 0, 1);

			float r, g, b;

			if (s <= 0.0f)
			{
				r = v;
				g = v;
				b = v;
				goto OUT;
			}

			Degree(ref h);

			h /= 60.0f;

			Int32 i = (Int32)h;
			float ff = h - i;
			float v0 = v * (1.0f - s);
			float v1 = v * (1.0f - (s * ff));
			float v2 = v * (1.0f - (s * (1.0f - ff)));

			switch (i)
			{
				case 0:
					r = v;
					g = v2;
					b = v0;
					break;
				case 1:
					r = v1;
					g = v;
					b = v0;
					break;
				case 2:
					r = v0;
					g = v;
					b = v2;
					break;

				case 3:
					r = v0;
					g = v1;
					b = v;
					break;
				case 4:
					r = v2;
					g = v0;
					b = v;
					break;
				case 5:
				default:
					r = v;
					g = v0;
					b = v1;
					break;
			}

			OUT:
			color.r = r;
			color.g = g;
			color.b = b;
			return color;
		}

		private static float Degree(ref float degree)
		{
			while (degree < 0)
				degree += 360f;
			while (degree >= 360f)
				degree -= 360f;
			return degree;
		}
		private static float Limit(ref float input, float min, float max) => input = (input > min ? (input < max ? input : max) : min);
		private static float Min(params float[] input) => input.Min();
		private static float Max(params float[] input) => input.Max();
	}
}
