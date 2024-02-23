using System.Collections.Generic;
using UnityEngine;

namespace CucuTools
{
    public static class CucuGizmos
    {
        #region Lines

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

        #endregion
        
        #region Spheres

        public static void DrawSpheres(IEnumerable<Vector3> centers, float radius = 1f)
        {
            foreach (var center in centers)
            {
                Gizmos.DrawSphere(center, radius);
            }
        }
        
        public static void DrawWireSpheres(IEnumerable<Vector3> centers, float radius = 1f)
        {
            foreach (var center in centers)
            {
                Gizmos.DrawWireSphere(center, radius);
            }
        }
        
        public static void DrawSpheres(float radius, params Vector3[] centers)
        {
            DrawSpheres(centers, radius);
        }
        
        public static void DrawWireSpheres(float radius, params Vector3[] centers)
        {
            DrawWireSpheres(centers, radius);
        }
        
        #endregion
        
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

                if (i < resolution)
                {
                    Gizmos.DrawLine(center, point);
                }
                
                lastPoint = point;
            }
        }

        private static readonly float[] GizmosDistance = new float[Bezier.DefaultResolution];
        
        public static void DrawBezier(Bezier bezier)
        {
            if (bezier.isEnd) return;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(bezier.pointIn, 0.05f);
            Gizmos.DrawWireSphere(bezier.pointOut, 0.05f);
/*
            // It is very noisy
            if (!bezier.isEnd)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(bezier.pointTangentIn, 0.05f);
                Gizmos.DrawLine(bezier.pointIn, bezier.pointTangentIn);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(bezier.pointTangentOut, 0.05f);
                Gizmos.DrawLine(bezier.pointOut, bezier.pointTangentOut);
            }
*/
            Gizmos.color = Color.green;
            CucuMath.LinSpace(0, bezier.GetLength(), GizmosDistance);

            var lastPoint = bezier.Evaluate(GizmosDistance[0]);

            for (var i = 1; i < GizmosDistance.Length; i++)
            {
                var point = bezier.Evaluate(GizmosDistance[i]);

                Gizmos.DrawLine(lastPoint, point);

                lastPoint = point;
            }
        }
    }
}