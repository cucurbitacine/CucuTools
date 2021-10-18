using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public static class Cucu
    {
        /// <summary>
        /// CucuTools
        /// </summary>
        public const string Root = "CucuTools";
        
        /// <summary>
        /// CucuTools/
        /// </summary>
        public const string CreateAsset = Root + "/";

        /// <summary>
        /// GameObject/
        /// </summary>
        public const string GameObject = "GameObject/";
        
        /// <summary>
        /// GameObject/CucuTools/
        /// </summary>
        public const string CreateGameObject = GameObject + CreateAsset;
        
        /// <summary>
        /// CucuTools/
        /// </summary>
        public const string AddComponent = CreateAsset;
        
        /// <summary>
        /// Tools/CucuTools/
        /// </summary>
        public const string Tools = "Tools/" + CreateAsset;
        
        public const string BlendGroup = "Blend/";
        public const string WorkflowGroup = "Workflow/";
        public const string InteractableGroup = "Interactable/";
        public const string Serialization = "Serialization/";

        #region Math

        public static float[] LinSpace(float origin, float target, int count)
        {
            var isForward = count >= 0;

            count = Mathf.Abs(count);

            var result = new float[count];

            switch (count)
            {
                case 0:
                    return result;
                case 1:
                    result[0] = isForward ? origin : target;
                    return result;
                case 2:
                    result[0] = isForward ? origin : target;
                    result[1] = isForward ? target : origin;
                    return result;
                default:
                {
                    result[0] = isForward ? origin : target;
                    result[count - 1] = isForward ? target : origin;
                    var step = (isForward ? 1 : -1) * (target - origin) / (count - 1);
                    for (var i = 1; i < count - 1; i++) result[i] = result[0] + step * i;
                    return result;
                }
            }
        }

        public static float[] LinSpace(int count)
        {
            return LinSpace(0f, 1f, count);
        }

        public static void IndexesOfBorder(out int left, out int right, float value, params float[] values)
        {
            left = -1;
            right = -1;

            for (int i = 0; i < values.Length; i++)
            {
                if (left < 0 && values[values.Length - 1 - i] <= value) left = values.Length - 1 - i;
                if (right < 0 && value <= values[i]) right = i;

                if (left >= 0 && right >= 0) return;
            }
        }

        public static void IndexesOfBorder<T>(out int left, out int right, float value, params T[] values)
            where T : IComparable<float>
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

        public static void IndexesOfBorder<T>(out int left, out int right, float value, IEnumerable<T> values)
            where T : IComparable<float>
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

        public static Vector3 Scale(this Vector3 vector3, float x, float y, float z)
        {
            return Vector3.Scale(vector3, new Vector3(x, y, z));
        }
        
        #endregion
        
        public static void Sync(this Rigidbody rigidbody, Transform target,  bool syncPosition = true, bool syncRotation = true, float maxVelocity = 500f, float syncWeight = 1f, float? deltaTime = null)
        {
            if (deltaTime == null) deltaTime = Time.fixedDeltaTime;
            
            if (syncPosition)
            {
                var dPos = target.position - rigidbody.transform.position;
                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.ClampMagnitude(dPos / deltaTime.Value, maxVelocity),
                    syncWeight);
            }

            if (syncRotation)
            {
                var from = rigidbody.transform.rotation;
                var to = target.rotation;
                var conj = new Quaternion(-from.x, -from.y, -from.z, from.w);
                var dq = new Quaternion((to.x - from.x) * 2.0f, 2.0f * (to.y - from.y), 2.0f * (to.z - from.z),
                    2.0f * (to.w - from.w));
                var c = dq * conj;
                var dRot = new Vector3(c.x, c.y, c.z);

                rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, dRot / deltaTime.Value, syncWeight);
            }
        }

        #region CommandArgs

        public static string[] GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }
        
        public static bool ContainsArg(string arg)
        {
            return GetCommandLineArgs().Any(x => x.StartsWith(arg));
        }
        
        public static string GetArg(string arg)
        {
            if (!ContainsArg(arg)) return string.Empty;

            return GetCommandLineArgs().First(x => x.Contains(arg));
        }

        public static string GetArg(string arg, string seporator)
        {
            return GetArg($"{arg}{seporator}").Replace($"{arg}{seporator}", "");
        }

        #endregion
    }
}
