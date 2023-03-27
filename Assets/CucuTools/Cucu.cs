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
        public const string DamageGroup = "Damage System/";

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
              
        public static void SyncRotation(Rigidbody rigid, Quaternion rot, Vector3 maxAngVel)
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
            return Cucu.ContainsLayer(layerMask, layerNumber);
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

        public static Vector2 OnlyXZ(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }
        
        public static Vector3 ToXZ(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }
        
        #endregion
    }
}
