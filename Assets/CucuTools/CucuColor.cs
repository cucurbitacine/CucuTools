using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public struct CucuColor
    {
        #region Evaluate

        /// <summary>
        /// Evaluating color from array of colors
        /// </summary>
        /// <param name="value">Value from 0 by 1</param>
        /// <param name="colors">Array of colors</param>
        /// <returns>Evaluated color</returns>
        public static Color Evaluate(float value, params Color[] colors)
        {
            if (colors.Length == 0) return Color.black.Alpha(0f);
            if (colors.Length == 1) return colors[0];

            value = Mathf.Clamp01(value);

            var step = 1f / (colors.Length - 1);

            for (var i = 0; i < colors.Length - 1; i++)
            {
                var t = step * i;
                if (t <= value && value <= t + step)
                {
                    return Color.Lerp(colors[i], colors[i + 1], Mathf.Clamp01((value - t) / step));
                }
            }

            return colors[colors.Length - 1];
        }

        #endregion

        #region Gradients & Palettes

        private const int GradientMaxAmountColor = 8;
        
        /// <summary>
        /// Create <see cref="Gradient"/> based on array of colors
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Gradient CreateGradient(params Color[] colors)
        {
            var amount = Mathf.Min(colors.Length, GradientMaxAmountColor);
            
            var colorKeys = new GradientColorKey[amount];
            var alphaKeys = new GradientAlphaKey[amount];

            var deltaTime = 1f / (amount - 1);
            for (var i = 0; i < amount; i++)
            {
                var time = i * deltaTime;

                var color = Evaluate(time, colors);
                        
                colorKeys[i].time = time;
                colorKeys[i].color = color;

                alphaKeys[i].time = time;
                alphaKeys[i].alpha = color.a;
            }

            return new Gradient
            {
                mode = GradientMode.Blend,
                colorKeys = colorKeys,
                alphaKeys = alphaKeys,
            };
        }

        /// <summary>
        /// Try to fill array of <see cref="Color"/> using colors from <see cref="Gradient"/>
        /// </summary>
        /// <param name="gradient">Colors source</param>
        /// <param name="colors">Filled array</param>
        /// <returns>Amount colors. Minimum: colors length or gradient length</returns>
        public static int GetColorsNonAlloc(Gradient gradient, Color[] colors)
        {
            var amount = Mathf.Min(colors.Length, gradient.colorKeys.Length);
            
            for (var i = 0; i < amount; i++)
            {
                var color = gradient.colorKeys[i].color;
                color.a = gradient.alphaKeys[i].alpha;
                
                colors[i] = color;
            }

            return amount;
        }
        
        /// <summary>
        /// Fill array of <see cref="Color"/> using <see cref="Gradient"/>.<see cref="Gradient.Evaluate"/> 
        /// </summary>
        /// <param name="gradient">Colors source</param>
        /// <param name="colors">Filled array</param>
        public static void EvaluateNonAlloc(Gradient gradient, Color[] colors)
        {
            var amount = colors.Length;

            var deltaTime = 1f / (amount - 1);
            for (var i = 0; i < amount; i++)
            {
                var time = i * deltaTime;

                colors[i] = gradient.Evaluate(time);
            }
        }
        
        public static readonly Color[] Rainbow = new Color[]
        {
            new Color(1.000f, 0.000f, 0.000f, 1.000f),
            new Color(1.000f, 0.500f, 0.000f, 1.000f),
            new Color(1.000f, 1.000f, 0.000f, 1.000f),
            new Color(0.000f, 1.000f, 0.000f, 1.000f),
            new Color(0.000f, 1.000f, 1.000f, 1.000f),
            new Color(0.000f, 0.000f, 1.000f, 1.000f),
            new Color(1.000f, 0.000f, 1.000f, 1.000f),
        };

        public static readonly Color[] Jet = new Color[]
        {
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
        };

        public static readonly Color[] Hot = new Color[]
        {
            Color.black,
            Color.red,
            Color.yellow,
            Color.white,
        };

        public static readonly Color[] Cold = new Color[]
        {
            Color.black,
            Color.blue,
            Color.cyan,
            Color.white,
        };
        
        public static readonly Color[] HotCold = new Color[]
        {
            Color.cyan,
            Color.blue,
            Color.black,
            Color.red,
            Color.yellow,
        };
        
        public static readonly Color[] BlackAndWhite = new Color[]
        {
            Color.black,
            Color.white,
        };

        public static readonly Dictionary<ColorPaletteType, Color[]> Palettes =
            new Dictionary<ColorPaletteType, Color[]>
            {
                { ColorPaletteType.Rainbow, Rainbow },
                { ColorPaletteType.Jet, Jet },
                { ColorPaletteType.Hot, Hot },
                { ColorPaletteType.Cold, Cold },
                { ColorPaletteType.HotCold, HotCold },
                { ColorPaletteType.BlackAndWhite, BlackAndWhite },
            };
        
        #endregion

        #region UV

        public static Color ColorUV(float u, float v, Color color00, Color color10, Color color11, Color color01)
        {
            return Color.Lerp(
                Color.Lerp(
                    Color.Lerp(color00, color10, u), Color.Lerp(color01, color11, u), v),
                Color.Lerp(
                    Color.Lerp(color00, color01, v), Color.Lerp(color10, color11, v), u),
                0.5f);
        }
        
        public static Color ColorUV(Vector2 uv, Color color00, Color color10, Color color11, Color color01)
        {
            return ColorUV(uv.x, uv.y, color00, color10, color11, color01);
        }

        #endregion
    }

    public static class CucuColorExtensions
    {
        public static Color R(this Color color, float r)
        {
            return new Color(Mathf.Clamp01(r), color.g, color.b, color.a);
        }
        
        public static Color G(this Color color, float g)
        {
            return new Color(color.r, Mathf.Clamp01(g), color.b, color.a);
        }
        
        public static Color B(this Color color, float b)
        {
            return new Color(color.r, color.g, Mathf.Clamp01(b), color.a);
        }
        
        public static Color Alpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha));
        }
        
        public static Color Hue(this Color color, float hue)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            
            return Color.HSVToRGB(Mathf.Repeat(hue, 1f), s, v);
        }
        
        public static Color Saturation(this Color color, float saturation)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            
            return Color.HSVToRGB(h, Mathf.Clamp01(saturation), v);
        }

        public static Color Value(this Color color, float value)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            
            return Color.HSVToRGB(h, s, Mathf.Clamp01(value));
        }

        public static Color LerpTo(this Color color, Color target, float t)
        {
            return Color.Lerp(color, target, t);
        }
        
        public static Color Evaluate(this Color[] colors, float value)
        {
            return CucuColor.Evaluate(value, colors);
        }
        
        public static Color Evaluate(this IEnumerable<Color> colors, float value)
        {
            return colors.ToArray().Evaluate(value);
        }

        public static Color AsColor(this Vector3 vector3, float alpha = 1f)
        {
            return new Color(vector3.x, vector3.y, vector3.z, alpha);
        }

        public static Color AsColor(this Vector4 vector4)
        {
            return new Color(vector4.x, vector4.y, vector4.z, vector4.w);
        }

        public static Vector3 AsVector3(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public static Vector4 AsVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }
        
        public static Color[] GetColors(this Gradient gradient, int amount)
        {
            var colors = new Color[amount];

            CucuColor.EvaluateNonAlloc(gradient, colors);
            
            return colors;
        }
        
        public static Color[] GetColors(this Gradient gradient)
        {
            return gradient.GetColors(gradient.colorKeys.Length);
        }
        
        public static Gradient CreateGradient(this IEnumerable<Color> colors)
        {
            return CucuColor.CreateGradient(colors.ToArray());
        }

        public static Color EvaluateColor(this Vector2 uv, Color color00, Color color10, Color color11, Color color01)
        {
            return CucuColor.ColorUV(uv, color00, color10, color11, color01);
        }
    }
    
    /// <summary>
    /// Color map list
    /// </summary>
    public enum ColorPaletteType
    {
        Rainbow,
        Jet,
        Hot,
        Cold,
        HotCold,
        BlackAndWhite,
    }
}