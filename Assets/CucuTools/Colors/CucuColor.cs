using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace CucuTools.Colors
{
    /// <summary>
    /// Basic logic for working with color
    /// </summary>
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

        /// <summary>
        /// Lerping color.
        /// Wrapper around Lerp function of <see cref="Color"/>
        /// </summary>
        /// <param name="origin">From</param>
        /// <param name="target">To</param>
        /// <param name="value">Lerp value</param>
        /// <returns></returns>
        public static Color Lerp(Color origin, Color target, float value)
        {
            return Color.Lerp(origin, target, value);
        }

        /// <summary>
        /// Blending color in queue of colors. Lerping only between two neighbor colors
        /// </summary>
        /// <param name="value">Blend value</param>
        /// <param name="colors">Colors</param>
        /// <returns>Color</returns>
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
                var linSpace = Cucu.LinSpace(8);
                colors = linSpace.Select(t => Lerp(t, colors)).ToArray();
            }

            var times = Cucu.LinSpace(colors.Length);

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

        /// <summary>
        /// Map of palettes
        /// </summary>
        public static readonly Dictionary<CucuColorMap, Gradient> GradientSample =
            new Dictionary<CucuColorMap, Gradient>
            {
                { CucuColorMap.Rainbow, Rainbow },
                { CucuColorMap.Jet, Jet },
                { CucuColorMap.Hot, Hot },
                { CucuColorMap.BlackToWhite, BlackToWhite },
                { CucuColorMap.WhiteToBlack, WhiteToBlack }
            };

        /// <summary>
        /// Rainbow palette
        /// </summary>
        public static Gradient Rainbow => Colors2Gradient(
            Color.red,
            Color.red.LerpTo(Color.yellow),
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            "CC00FF".ToColor()
        );

        /// <summary>
        /// Jet palette
        /// </summary>
        public static Gradient Jet => CucuColor.Colors2Gradient(
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

        /// <summary>
        /// Hot palette
        /// </summary>
        public static Gradient Hot => Colors2Gradient(Color.black, Color.red, Color.yellow, Color.white);

        /// <summary>
        /// Black to white palette
        /// </summary>
        public static Gradient BlackToWhite => Colors2Gradient(Color.black, Color.white);


        /// <summary>
        /// White to black palette
        /// </summary>
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
    }
}