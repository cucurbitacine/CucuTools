using UnityEngine;

namespace CucuTools.Utilities
{
    public static class CucuGizmos
    {
        public static void DrawLineStrip(bool looped, params Vector3[] corners)
        {
            Gizmos.DrawLineStrip(corners, looped);
        }

        public static void DrawLineStrip(params Vector3[] corners)
        {
            DrawLineStrip(false, corners);
        }

        public static void DrawLineLooped(params Vector3[] corners)
        {
            DrawLineStrip(true, corners);
        }

        public static void DrawWireCube(Vector3 origin, Vector3 size, Quaternion rotation)
        {
            var a = -size * 0.5f;
            var b = a + Vector3.up * size.y;
            var c = b + Vector3.forward * size.z;
            var d = c - Vector3.up * size.y;
            var e = a + Vector3.right * size.x;
            var f = b + Vector3.right * size.x;
            var g = c + Vector3.right * size.x;
            var h = d + Vector3.right * size.x;

            a = rotation * a + origin;
            b = rotation * b + origin;
            c = rotation * c + origin;
            d = rotation * d + origin;
            e = rotation * e + origin;
            f = rotation * f + origin;
            g = rotation * g + origin;
            h = rotation * h + origin;

            DrawLineLooped(a, b, c, d);
            DrawLineLooped(e, f, g, h);
            DrawLineStrip(a, e);
            DrawLineStrip(b, f);
            DrawLineStrip(c, g);
            DrawLineStrip(d, h);
        }

        public static void DrawCircle(Vector3 center, Vector3 normal, float radius = 0.5f, int resolution = 36)
        {
            var rotation = Quaternion.FromToRotation(Vector3.up, normal);

            var prevPoint = Vector3.zero;
            var deltaRad = Mathf.PI * 2 / resolution;
            for (var i = 0; i < resolution + 1; i++)
            {
                var rad = i * deltaRad;
                var point = center + (rotation * new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius);
                if (i > 0)
                {
                    Gizmos.DrawLine(prevPoint, point);
                }
                prevPoint = point;
            }
        }

        public static void DrawCircle(Vector3 center, float radius = 0.5f, int resolution = 36)
        {
            DrawCircle(center, Vector3.up, radius, resolution);
        }
    }
}