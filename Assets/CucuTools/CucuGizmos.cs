using UnityEngine;

namespace CucuTools
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

        public static void DrawWireCube(Vector3 origin, Vector3? size = null, Quaternion? rotation = null)
        {
            size ??= Vector3.one;
            rotation ??= Quaternion.identity;

            var a = -size.Value * 0.5f;
            var b = a + Vector3.up * size.Value.y;
            var c = b + Vector3.forward * size.Value.z;
            var d = c - Vector3.up * size.Value.y;
            var e = a + Vector3.right * size.Value.x;
            var f = b + Vector3.right * size.Value.x;
            var g = c + Vector3.right * size.Value.x;
            var h = d + Vector3.right * size.Value.x;

            a = rotation.Value * a + origin; 
            b = rotation.Value * b + origin; 
            c = rotation.Value * c + origin; 
            d = rotation.Value * d + origin; 
            e = rotation.Value * e + origin; 
            f = rotation.Value * f + origin; 
            g = rotation.Value * g + origin; 
            h = rotation.Value * h + origin; 
            
            DrawLineLooped(a, b, c, d);
            DrawLineLooped(e, f, g, h);
            DrawLineStrip(a, e);
            DrawLineStrip(b, f);
            DrawLineStrip(c, g);
            DrawLineStrip(d, h);
        }
        
        public static void DrawCircle(Vector3 center, Vector3? normal = null, float radius = 0.5f, int resolution = 36)
        {
            normal ??= Vector3.up;
            
            var rot = Quaternion.FromToRotation(Vector3.up, normal.Value);

            var lastPoint = Vector3.zero;
            var dPhi = Mathf.PI * 2 / resolution;
            for (var i = 0; i < resolution + 1; i++)
            {
                var phi = i * dPhi;
                var point = center + (rot * new Vector3(Mathf.Cos(phi), 0, Mathf.Sin(phi)) * radius);

                if (i > 0)
                {
                    Gizmos.DrawLine(lastPoint, point);
                }
                
                lastPoint = point;
            }
        }
    }
}