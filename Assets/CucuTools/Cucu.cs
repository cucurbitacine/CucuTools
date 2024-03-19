using System.Collections;
using CucuTools.Async;
using CucuTools.FX;
using CucuTools.Pools;
using UnityEngine;

namespace CucuTools
{
    public static class Cucu
    {
        public const string Root = "CucuTools";
        public const string CreateAsset = Root + "/";
        public const string DamageGroup = "Damage System/";

        #region Others

        public static bool ContainsLayer(LayerMask layerMask, int layerNumber)
        {
            return (layerMask.value & (1 << layerNumber)) > 0;
        }

        #endregion

        #region Coroutines

        public static Coroutine StartCoroutine(IEnumerator enumerator)
        {
            return CucuCoroutine.Start(enumerator);
        }
        
        public static void StopCoroutine(Coroutine coroutine)
        {
            CucuCoroutine.Stop(coroutine);
        }

        #endregion

        #region Prefab Manager

        public static PoolManager PoolManager => PoolManager.singleton;
        
        public static GameObject Instantiate(GameObject prefab)
        {
            return PoolManager.Create(prefab);
        }
        
        public static T Instantiate<T>(T prefab) where T : Component
        {
            return PoolManager.Create<T>(prefab);
        }

        #endregion
        
        #region Extensions

        public static bool Contains(this LayerMask layerMask, int layerNumber)
        {
            return ContainsLayer(layerMask, layerNumber);
        }
        
        public static bool Contains(this int layerNumber, LayerMask layerMask)
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
