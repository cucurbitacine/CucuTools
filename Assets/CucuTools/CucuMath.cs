using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public static class CucuMath
    {
        public static float[] LinSpace(float a, float b, int resolution)
        {
            var isForward = resolution >= 0;

            resolution = Mathf.Abs(resolution);

            var result = new float[resolution];

            switch (resolution)
            {
                case 0:
                    return result;
                case 1:
                    result[0] = isForward ? a : b;
                    return result;
                case 2:
                    result[0] = isForward ? a : b;
                    result[1] = isForward ? b : a;
                    return result;
                default:
                {
                    result[0] = isForward ? a : b;
                    result[resolution - 1] = isForward ? b : a;
                    var step = (isForward ? 1 : -1) * (b - a) / (resolution - 1);
                    for (var i = 1; i < resolution - 1; i++) result[i] = result[0] + step * i;
                    return result;
                }
            }
        }

        public static float[] LinSpace(int resolution)
        {
            return LinSpace(0f, 1f, resolution);
        }

        public static void IndexesOfBorder<T>(out int left, out int right, T value, params T[] values)
            where T : IComparable
        {
            left = -1;
            right = -1;

            for (int i = 0; i < values.Length; i++)
            {
                if (left < 0 && values[values.Length - 1 - i].CompareTo(value) <= 0) left = values.Length - 1 - i;
                if (right < 0 && values[i].CompareTo(value) > 0) right = i;

                if (left >= 0 && right >= 0) return;
            }
        }
        
        public static void IndexesOfBorder<T>(out int left, out int right, T value, List<T> values)
            where T : IComparable
        {
            left = -1;
            right = -1;

            for (int i = 0; i < values.Count; i++)
            {
                if (left < 0 && values[values.Count - 1 - i].CompareTo(value) <= 0) left = values.Count - 1 - i;
                if (right < 0 && values[i].CompareTo(value) > 0) right = i;

                if (left >= 0 && right >= 0) return;
            }
        }
        
        public static void IndexesOfBorder(out int left, out int right, float value, params float[] values)
        {
            IndexesOfBorder<float>(out left, out right, value, values);
        }

        public static void IndexesOfBorder(out int left, out int right, int value, params int[] values)
        {
            IndexesOfBorder<int>(out left, out right, value, values);
        }
        
        public static void IndexesOfBorder<T>(out int left, out int right, T value, IEnumerable<T> values)
            where T : IComparable
        {
            IndexesOfBorder<T>(out left, out right, value, values.ToArray());
        }

        public static Vector3 BezierPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var t3 = t * t * t;
            var t2 = t * t;

            return p0 * (-t3 + 3 * t2 - 3 * t + 1) +
                   p1 * (3 * t3 - 6 * t2 + 3 * t) +
                   p2 * (-3 * t3 + 3 * t2) +
                   p3 * t3;
        }
        
        public static Vector3 BezierVelocity(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var t2 = t * t;

            return p0 * (-3 * t2 + 6 * t - 3) +
                   p1 * (9 * t2 - 12 * t + 3) +
                   p2 * (-9 * t2 + 6 * t) +
                   p3 * 3 * t2;
        }

        public static Vector3 BezierAcceleration(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return p0 * (-6 * t + 6) +
                   p1 * (18 * t - 12) +
                   p2 * (-18 * t + 6) +
                   p3 * 6 * t;
        }

        public static float BezierLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int resolution = 32)
        {
            var res = 0f;
            Vector3 prev = default;
            for (var i = 0; i < resolution; i++)
            {
                var t = (float)i / (resolution - 1);

                var curr = BezierPosition(t, p0, p1, p2, p3);
                if (i > 0) res += Vector3.Distance(prev, curr);
                prev = curr;
            }

            return res;
        }
        
        public static Vector3 BezierPosition(float t, Vector3[] p)
        {
            return BezierPosition(t, p[0], p[1], p[2], p[3]);
        }

        public static Vector3 BezierVelocity(float t, Vector3[] p)
        {
            return BezierVelocity(t, p[0], p[1], p[2], p[3]);
        }

        public static Vector3 BezierAcceleration(float t, Vector3[] p)
        {
            return BezierAcceleration(t, p[0], p[1], p[2], p[3]);
        }

        public static float BezierLength(Vector3[] p, int resolution = 32)
        {
            return BezierLength(p[0], p[1], p[2], p[3], resolution);
        }
    }
}