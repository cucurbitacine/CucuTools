using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CucuTools
{
    public static class Cucu
    {
        public const string Root = "CucuTools";
        public const string CreateAsset = Root + "/";
        public const string GameObject = "GameObject/";
        public const string CreateGameObject = GameObject + CreateAsset;
        public const string AddComponent = CreateAsset;
        public const string Tools = "Tools/" + CreateAsset;
        public const string MultiLangGroup = "Multi Lang System/";
        public const string StateMachineGroup = "State Machine/";
        public const string SurfaceGroup = "Surfaces/";
        public const string SerializationGroup = "Serialization/";

        #region Math

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

        #endregion

        #region Physics

        public static void SyncRigid(Rigidbody rigid, Vector3 pos, Quaternion rot,  float maxVel = 512f, float maxAngVel = 512f)
        {
            SyncPosition(rigid, pos, maxVel);
            SyncRotation(rigid, rot, maxAngVel);
        }

        public static void SyncPosition(Rigidbody rigid, Vector3 pos, float maxVel = 512f)
        {
            rigid.velocity = CalcVelocity(rigid.position, pos, maxVel);
        }
        
        public static void SyncRotation(Rigidbody rigid, Quaternion rot, float maxAngVel = 512f)
        {
            rigid.angularVelocity = CalcAngularVelocity(rigid.rotation, rot, maxAngVel);
        }
                
        private static Vector3 CalcVelocity(Vector3 from, Vector3 to, float maxVelocity = 512f)
        {
            var dPos = to - from;
            return Vector3.ClampMagnitude(dPos / Time.fixedDeltaTime, maxVelocity);
        }
        
        private static Vector3 CalcAngularVelocity(Quaternion from, Quaternion to, float maxAngVel = 512f)
        {
            var rotate = to * Quaternion.Inverse(from);
            rotate.ToAngleAxis(out var angle, out var axis);
            var dRot = axis * (angle * Mathf.Deg2Rad);

            return Vector3.ClampMagnitude(dRot / Time.fixedDeltaTime, maxAngVel);
        }
        
        public static void SyncRotation(Rigidbody rigid, Quaternion rot, Vector3 maxAngVel)
        {
            rigid.angularVelocity = CalcAngularVelocity(rigid.rotation, rot, maxAngVel);
        }

        private static Vector3 CalcAngularVelocity(Quaternion from, Quaternion to, Vector3 maxAngVel)
        {
            var rotate = to * Quaternion.Inverse(from);
            rotate.ToAngleAxis(out var angle, out var axis);
            var dRot = axis * (angle * Mathf.Deg2Rad);

            var angVel = dRot / Time.fixedDeltaTime;

            for (var i = 0; i < 3; i++)
            {
                angVel[i] = Mathf.Sign(angVel[i]) * Mathf.Clamp(Mathf.Abs(angVel[i]), 0, Mathf.Abs(maxAngVel[i]));
            }
            
            return angVel;
        }
        
        #endregion

        #region Others

                public static T Clone<T>(T input) where T : class, new()
        {
            var output = new T();

            var outputType = output.GetType();
            var inputType = input.GetType();
            
            const BindingFlags flags = BindingFlags.Instance |
                                       BindingFlags.Public |
                                       BindingFlags.NonPublic;
            
            var outputFields = outputType.GetFields(flags);
            var inputFields = inputType.GetFields(flags);

            foreach (var outputField in outputFields)
            {
                var field = inputFields.FirstOrDefault(f => f.Name == outputField.Name);
                if (field == null) continue;
                field.SetValue(output, field.GetValue(input));
            }

            var outputProperties = outputType.GetProperties(flags);
            var inputProperties = inputType.GetProperties(flags);

            foreach (var outputProperty in outputProperties)
            {
                if (!outputProperty.CanRead || !outputProperty.CanWrite) continue;
                var property = inputProperties.FirstOrDefault(f => f.Name == outputProperty.Name);
                if (property == null) continue;
                property.SetValue(output, property.GetValue(input));
            }
            
            return output;
        }

        public static bool ContainsLayer(LayerMask layerMask, int layerNumber)
        {
            return (layerMask.value & (1 << layerNumber)) > 0;
        }

        public static Bounds GetBounds(GameObject gameObject)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>().ToArray();

            if (renderers.Length == 0) return default;

            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                var temp = renderer.bounds;

                temp.size = Vector3.Scale(temp.size, renderer.transform.lossyScale);

                bounds.Encapsulate(temp);
            }

            return bounds;
        }

        #endregion
        
        #region Extensions

        public static bool Contains(this LayerMask layerMask, int layerNumber)
        {
            return ContainsLayer(layerMask, layerNumber);
        }
        
        public static Vector3 Scale(this Vector3 vector3, float x, float y, float z)
        {
            return Vector3.Scale(vector3, new Vector3(x, y, z));
        }

        public static Vector3 ToWorldPoint(this Vector3 localPoint, Transform transform)
        {
            return transform.TransformPoint(localPoint);
        } 
        
        public static Vector3 ToLocalPoint(this Vector3 worldPoint, Transform transform)
        {
            return transform.InverseTransformPoint(worldPoint);
        } 
        
        public static Vector3 ToWorldDirection(this Vector3 localDirection, Transform transform)
        {
            return transform.TransformDirection(localDirection);
        } 
        
        public static Vector3 ToLocalDirection(this Vector3 worldDirection, Transform transform)
        {
            return transform.InverseTransformDirection(worldDirection);
        } 
        
        public static Vector3 ToWorldVector(this Vector3 localVector, Transform transform)
        {
            return transform.TransformVector(localVector);
        } 
        
        public static Vector3 ToLocalVector(this Vector3 worldVector, Transform transform)
        {
            return transform.InverseTransformVector(worldVector);
        } 
        
        #endregion
    }
}
