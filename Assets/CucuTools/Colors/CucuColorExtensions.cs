using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.Colors
{
    /// <summary>
    /// Color extensions
    /// </summary>
    public static class CucuColorExtensions
    {
        /// <summary>
        /// Get string color like FFFFFF
        /// Like 
        /// </summary>
        /// <param name="color"></param>
        /// <returns>Hex string</returns>
        public static string ToHex(this Color color)
        {
            return CucuColor.Color2Hex(color);
        }

        /// <summary>
        /// Get color from hex string
        /// </summary>
        /// <param name="hex">String hex color</param>
        /// <returns>Color</returns>
        public static Color ToColor(this string hex)
        {
            return CucuColor.Hex2Color(hex);
        }

        /// <summary>
        /// Lerp color to <param name="target"></param>
        /// </summary>
        /// <param name="color">Origin</param>
        /// <param name="target">Target</param>
        /// <param name="t">Lerp value</param>
        /// <returns>Color</returns>
        public static Color LerpTo(this Color color, Color target, float t = 0.5f)
        {
            return CucuColor.Lerp(color, target, t);
        }

        /// <summary>
        /// Lerping color in queue colors
        /// </summary>
        /// <param name="value">Lerp value</param>
        /// <param name="colors">Colors</param>
        /// <returns>Color</returns>
        public static Color LerpColor(this float value, params Color[] colors)
        {
            return CucuColor.Lerp(value, colors);
        }

        /// <summary>
        /// Blending color in queue colors
        /// </summary>
        /// <param name="colors">Colors</param>
        /// <param name="value">Blend value</param>
        /// <returns>Color</returns>
        public static Color LerpColor(this IEnumerable<Color> colors, float value)
        {
            return value.LerpColor(colors.ToArray());
        }

        public static Color SetHue(this Color color, float hue)
        {
            return CucuColor.Hue(color, hue);
        }
        
        public static Color SetSaturation(this Color color, float saturation)
        {
            return CucuColor.Saturation(color, saturation);
        }

        public static Color SetValue(this Color color, float value)
        {
            return CucuColor.Value(color, value);
        }
        
        /// <summary>
        /// Set color alpha
        /// </summary>
        /// <param name="color">Color</param>
        /// <param name="value">Alpha</param>
        /// <returns>Color</returns>
        public static Color SetAlpha(this Color color, float value)
        {
            return CucuColor.Alpha(color, value);
        }

        /// <summary>
        /// Convert vector3 to color
        /// </summary>
        /// <param name="vector3">Vector3</param>
        /// <param name="alpha">Alpha value</param>
        /// <returns>Color</returns>
        public static Color ToColor(this Vector3 vector3, float alpha = 1f)
        {
            return new Color(vector3.x, vector3.y, vector3.z, alpha);
        }

        /// <summary>
        /// Convert vector4 to color
        /// </summary>
        /// <param name="vector4">Vector4</param>
        /// <returns>Color</returns>
        public static Color ToColor(this Vector4 vector4)
        {
            return new Color(vector4.x, vector4.y, vector4.z, vector4.w);
        }

        /// <summary>
        /// Convert color to vector3
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>Vector3</returns>
        public static Vector3 ToVector3(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        /// <summary>
        /// Convert color to vector4
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>Vector4</returns>
        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }

        public static Gradient ToGradient(this IEnumerable<Color> colors)
        {
            return CucuColor.Colors2Gradient(colors.ToArray());
        }

        public static Color[] ToColors(this Gradient gradient)
        {
            return CucuColor.Gradient2Colors(gradient);
        }

        public static Color ColorUV(this Vector2 uv, Color color00, Color color10, Color color11, Color color01)
        {
            return CucuColor.ColorUV(uv, color00, color10, color11, color01);
        }

        public static Color ColorUV(this Vector2 uv)
        {
            return ColorUV(uv, CucuColor.Color00, CucuColor.Color10, CucuColor.Color11, CucuColor.Color01);
        }
    }
}