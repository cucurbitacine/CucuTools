using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public static class CucuColor
    {
        public static readonly Color Color00 = new Color(0.45f, 0.35f, 0.65f);
        public static readonly Color Color01 = new Color(0.00f, 0.70f, 0.70f);
        public static readonly Color Color11 = new Color(1.00f, 0.90f, 0.15f);
        public static readonly Color Color10 = new Color(0.95f, 0.35f, 0.25f);

        public static Color Hue(Color color, float hue)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            
            return Color.HSVToRGB(Mathf.Repeat(hue, 1f), s, v);
        }
        
        public static Color Saturation(Color color, float saturation)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            
            return Color.HSVToRGB(h, Mathf.Clamp01(saturation), v);
        }

        public static Color Value(Color color, float value)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            
            return Color.HSVToRGB(h, s, Mathf.Clamp01(value));
        }
        
        public static Color Alpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha));
        }

        public static string Color2Hex(Color color)
        {
            var result = "";
            for (var i = 0; i < 4; i++)
            {
                var val = (int)Mathf.Clamp(color[i] * 255, 0, 255);
                var hex = val.ToString("X");
                if (hex.Length < 2) hex = "0" + hex;
                result += hex;
            }

            return result;
        }

        public static bool TryGetColorFromHex(string hex, out Color color)
        {
            color = Color.black;

            if (string.IsNullOrWhiteSpace(hex) || (hex.Length != 6 && hex.Length != 8))
            {
                return false;
            }

            var intR = 0;
            var intG = 0;
            var intB = 0;
            var intA = 0;

            try
            {
                intR = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                intG = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                intB = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                intA = hex.Length == 8
                    ? int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber)
                    : 255;
            }
            catch
            {
                return false;
            }

            color = new Color(intR / 255f, intG / 255f, intB / 255f, intA / 255f);
            return true;
        }

        public static Color Hex2Color(string hex)
        {
            return TryGetColorFromHex(hex, out var color) ? color : Color.black;
        }

        #region Lerp & Blend

        public static Color Lerp(float value, params Color[] colors)
        {
            if (colors.Length == 0) return Alpha(Color.black, 0f);
            if (colors.Length == 1) return colors[0];

            value = Mathf.Clamp01(value);

            var dt = 1f / (colors.Length - 1);

            for (var i = 0; i < colors.Length - 1; i++)
            {
                var t = dt * i;
                if (t <= value && value <= t + dt)
                {
                    return colors[i].LerpTo(colors[i + 1], Mathf.Clamp01((value - t) / dt));
                }
            }

            return colors[colors.Length - 1];
        }

        #endregion

        #region Gradients

        public static Gradient Colors2Gradient(params Color[] colors)
        {
            if (colors.Length > 8)
            {
                var linSpace = CucuMath.LinSpace(8);
                colors = linSpace.Select(t => Lerp(t, colors)).ToArray();
            }

            var times = CucuMath.LinSpace(colors.Length);

            return new Gradient
            {
                mode = GradientMode.Blend,
                colorKeys = times.Select((t, i) => new GradientColorKey(colors[i], t)).ToArray(),
                alphaKeys = times.Select((t, i) => new GradientAlphaKey(colors[i].a, t)).ToArray()
            };
        }

        public static Color[] Gradient2Colors(Gradient gradient)
        {
            return gradient.colorKeys
                .Select((c, i) => new Color(c.color.r, c.color.g, c.color.b, gradient.alphaKeys[i].alpha))
                .ToArray();
        }

        public static readonly Dictionary<CucuColorMap, Gradient> GradientSample =
            new Dictionary<CucuColorMap, Gradient>
            {
                { CucuColorMap.Rainbow, Rainbow },
                { CucuColorMap.Jet, Jet },
                { CucuColorMap.Hot, Hot },
                { CucuColorMap.BlackToWhite, BlackToWhite },
                { CucuColorMap.WhiteToBlack, WhiteToBlack }
            };

        public static Gradient Rainbow => Colors2Gradient(
            Color.red,
            Color.red.LerpTo(Color.yellow),
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            "CC00FF".ToColor()
        );

        public static Gradient Jet => Colors2Gradient(
            new Color(0.000f, 0.000f, 0.666f, 1.000f),
            new Color(0.000f, 0.000f, 1.000f, 1.000f),
            new Color(0.000f, 0.333f, 1.000f, 1.000f),
            new Color(0.000f, 0.666f, 1.000f, 1.000f),
            new Color(0.000f, 1.000f, 1.000f, 1.000f),
            new Color(0.500f, 1.000f, 0.500f, 1.000f),
            new Color(1.000f, 1.000f, 0.000f, 1.000f),
            new Color(1.000f, 0.666f, 0.000f, 1.000f),
            new Color(1.000f, 0.333f, 0.000f, 1.000f),
            new Color(1.000f, 0.000f, 0.000f, 1.000f),
            new Color(0.666f, 0.000f, 0.000f, 1.000f)
        );

        public static Gradient Hot => Colors2Gradient(Color.black, Color.red, Color.yellow, Color.white);
        public static Gradient BlackToWhite => Colors2Gradient(Color.black, Color.white);
        public static Gradient WhiteToBlack => Colors2Gradient(Color.white, Color.black);

        #endregion

        #region UV

        public static Color ColorUV(Vector2 uv, Color color00, Color color10, Color color11, Color color01)
        {
            return Color.Lerp(
                Color.Lerp(
                    Color.Lerp(color00, color10, uv.x), Color.Lerp(color01, color11, uv.x), uv.y),
                Color.Lerp(
                    Color.Lerp(color00, color01, uv.y), Color.Lerp(color10, color11, uv.y), uv.x),
                0.5f);
        }

        public static Color ColorUV(Vector2 uv)
        {
            return ColorUV(uv, Color00, Color10, Color11, Color01);
        }

        #endregion

        #region Extensions

        public static string ToHex(this Color color)
        {
            return Color2Hex(color);
        }

        public static Color ToColor(this string hex)
        {
            return Hex2Color(hex);
        }

        public static Color LerpTo(this Color color, Color target, float t = 0.5f)
        {
            return Color.Lerp(color, target, t);
        }

        public static Color LerpColor(this float value, params Color[] colors)
        {
            return Lerp(value, colors);
        }

        public static Color LerpColor(this IEnumerable<Color> colors, float value)
        {
            return value.LerpColor(colors.ToArray());
        }

        public static Color SetHue(this Color color, float hue)
        {
            return Hue(color, hue);
        }
        
        public static Color SetSaturation(this Color color, float saturation)
        {
            return Saturation(color, saturation);
        }

        public static Color SetValue(this Color color, float value)
        {
            return Value(color, value);
        }
        
        public static Color SetAlpha(this Color color, float value)
        {
            return Alpha(color, value);
        }

        public static Color ToColor(this Vector3 vector3, float alpha = 1f)
        {
            return new Color(vector3.x, vector3.y, vector3.z, alpha);
        }

        public static Color ToColor(this Vector4 vector4)
        {
            return new Color(vector4.x, vector4.y, vector4.z, vector4.w);
        }

        public static Vector3 ToVector3(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }

        public static Gradient ToGradient(this IEnumerable<Color> colors)
        {
            return Colors2Gradient(colors.ToArray());
        }

        public static Color[] ToColors(this Gradient gradient)
        {
            return Gradient2Colors(gradient);
        }

        public static Color ToColorUV(this Vector2 uv, Color color00, Color color10, Color color11, Color color01)
        {
            return ColorUV(uv, color00, color10, color11, color01);
        }

        public static Color ToColorUV(this Vector2 uv)
        {
            return ColorUV(uv, CucuColor.Color00, CucuColor.Color10, CucuColor.Color11, CucuColor.Color01);
        }

        #endregion
    }
    
    /// <summary>
    /// Color map list
    /// </summary>
    public enum CucuColorMap
    {
        Rainbow,
        Jet,
        Hot,
        BlackToWhite,
        WhiteToBlack
    }
}