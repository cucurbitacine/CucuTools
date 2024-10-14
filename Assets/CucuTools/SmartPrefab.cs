using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools
{
    [DisallowMultipleComponent]
    public sealed class SmartPrefab : MonoBehaviour
    {
        /// <summary>
        /// Keeps Id of own prefab.
        /// Id is set after Instantiating and before <see cref="Initialized"/> is invoked.
        /// Id is taken from <see cref="UnityEngine.Object.GetInstanceID"/>.
        /// Id equals zero by default.
        /// </summary>
        public int PrefabId { get; private set; } = 0;

        /// <summary>
        /// Invokes after OnEnable
        /// </summary>
        public event Action Initialized;
        
        /// <summary>
        /// Invokes before OnDisable
        /// </summary>
        public event Action Uninitialized;

        private void Initialize()
        {
            Initialized?.Invoke();
        }

        private void Uninitialize()
        {
            Uninitialized?.Invoke();
        }
        
        private void OnDestroy()
        {
            HandleDestroy(this);
        }

        #region Static Pool Logic
        
        private static readonly Dictionary<int, SmartPool<SmartPrefab>> Pools = new Dictionary<int, SmartPool<SmartPrefab>>();
        
        public static bool AddToPool(SmartPrefab smartObject)
        {
            if (smartObject.PrefabId != 0)
            {
                var pool = GetPool(smartObject.PrefabId);

                return pool.Add(smartObject);
            }

            return false;
        }

        public static bool RemoveFromPool(SmartPrefab smartObject)
        {
            if (smartObject.PrefabId != 0)
            {
                var pool = GetPool(smartObject.PrefabId);

                return pool.Remove(smartObject);
            }

            return false;
        }

        public static GameObject SmartInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!prefab.TryGetComponent<SmartPrefab>(out _))
            {
                // This is not a Smart Object

                return Instantiate(prefab, position, rotation, parent);
            }

            var pool = GetPool(prefab.GetInstanceID());

            if (pool.TryPeek(out var peeked))
            {
                // There are at least one Smart Object
                // Return it as new
                
                return InstantiateFromPool(peeked, position, rotation, parent);
            }

            // Pool is empty
            // Return just created object

            return InstantiateNew(prefab, position, rotation, parent);
        }

        public static void SmartDestroy(SmartPrefab smartObject)
        {
            if (smartObject.PrefabId != 0)
            {
                // Remove its parent and hide it

                smartObject.Uninitialize();
                
                smartObject.transform.SetParent(null, true);
                smartObject.gameObject.SetActive(false);

                AddToPool(smartObject);
            }
            else
            {
                // It has no Prefab, so it must be destroyed 
                
                Destroy(smartObject.gameObject);
            }
        }
        
        public static void SmartDestroy(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<SmartPrefab>(out var smartObject))
            {
                // If it is a smart object, it is possible reuse it
                
                SmartDestroy(smartObject);
            }
            else
            {
                // It is not a smart object, or it has no prefab (already has been on a scene)

                Destroy(gameObject);
            }
        }

        public static GameObject SmartInstantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return SmartInstantiate(prefab, position, rotation, null);
        }

        public static GameObject SmartInstantiate(GameObject prefab)
        {
            return SmartInstantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        public static T SmartInstantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
        {
            return SmartInstantiate(prefab.gameObject, position, rotation, parent).GetComponent<T>();
        }

        public static T SmartInstantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return SmartInstantiate(prefab, position, rotation, null);
        }

        public static T SmartInstantiate<T>(T prefab) where T : Component
        {
            return SmartInstantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        private static GameObject InstantiateFromPool(SmartPrefab smartObject, Vector3 position, Quaternion rotation, Transform parent)
        {
            RemoveFromPool(smartObject);
            
            smartObject.transform.SetPositionAndRotation(position, rotation);
            smartObject.transform.SetParent(parent, true);
            smartObject.gameObject.SetActive(true);

            smartObject.Initialize();
            
            return smartObject.gameObject;
        }

        private static GameObject InstantiateNew(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var gameObject = Instantiate(prefab, position, rotation, parent);

            if (gameObject.TryGetComponent<SmartPrefab>(out var smartObject))
            {
                smartObject.PrefabId = prefab.GetInstanceID();

                smartObject.Initialize();
            }

            return gameObject;
        }

        private static SmartPool<SmartPrefab> GetPool(int prefabId)
        {
            if (Pools.TryGetValue(prefabId, out var pool)) return pool;

            pool = new SmartPool<SmartPrefab>(prefabId);

            Pools.Add(prefabId, pool);

            return pool;
        }
        
        private static void HandleDestroy(SmartPrefab smartObject)
        {
            RemoveFromPool(smartObject);
        }

        #endregion
    }

    public class SmartPool<T> where T : class
    {
        public const int DefaultCapacity = 32;

        public readonly int Id;

        private readonly List<T> list = new List<T>();

        public int Count => list.Count;

        public int Capacity { get; private set; }

        public SmartPool(int id, int capacity = DefaultCapacity)
        {
            Id = id;
            Capacity = capacity;
        }

        public bool TryPeek(out T next)
        {
            next = Count > 0 ? list[0] : null;

            return next != null;
        }

        public bool Contains(T smart)
        {
            return list.Contains(smart);
        }

        public bool Add(T smart)
        {
            if (Contains(smart)) return false;

            if (Count >= Capacity) return false;

            list.Add(smart);

            return true;
        }

        public bool Remove(T smart)
        {
            return list.Remove(smart);
        }
    }
}