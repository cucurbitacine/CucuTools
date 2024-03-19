using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CucuTools.Pools
{
    public class PoolManager : Singleton<PoolManager>
    {
        private readonly List<PoolObjectProfile> profiles = new List<PoolObjectProfile>();

        public GameObject Create(GameObject prefab)
        {
            if (prefab == null) throw new NullReferenceException();
            
            var prefabId = prefab.GetInstanceID();
            
            var activeScene = SceneManager.GetActiveScene();
            var sceneId = activeScene.handle;
            
            foreach (var profile in profiles)
            {
                if (profile.available && profile.released && profile.typeId == prefabId)
                {
                    if (profile.groupId != sceneId)
                    {
                        if (profile.gameObject.transform.parent)
                        {
                            profile.gameObject.transform.SetParent(null);
                        }
                        SceneManager.MoveGameObjectToScene(profile.gameObject, activeScene);
                        profile.groupId = sceneId;
                    }

                    if (profile.poolObject.releaseOnDisable && !profile.gameObject.activeSelf)
                    {
                        profile.gameObject.SetActive(true);
                    }
                    
                    profile.released = false;
                    return profile.gameObject;
                }
            }

            var createdObject = Instantiate(prefab);
            var poolObject = createdObject.GetComponent<PoolObject>();
            if (poolObject == null) poolObject = createdObject.AddComponent<PoolObject>();

            var poolObjectProfile = new PoolObjectProfile()
            {
                poolObject = poolObject,
                typeId = prefabId,
                groupId = sceneId,
                disposed = false,
                released = false,
            };

            poolObject.profile = poolObjectProfile;

            profiles.Add(poolObjectProfile);
            
            return createdObject;
        }
        
        public T Create<T>(T prefab) where T : Component
        {
            return Create(prefab.gameObject).GetComponent<T>();
        }

        public int Count(Func<PoolObjectProfile, bool> predicate)
        {
            return profiles.Count(predicate);
        }
        
        public int Count(GameObject prefab)
        {
            if (prefab == null) throw new NullReferenceException();
            var prefabId = prefab.GetInstanceID();
            return Count(p => p.typeId == prefabId);
        }

        public int Count<T>(T prefab) where T : Component
        {
            return Count(prefab.gameObject);
        }
        
        public int DestroyReleasedObjects()
        {
            var count = 0;
            foreach (var profile in profiles.Where(profile => profile.available && profile.released))
            {
                count++;
                profile.available = false;
                Destroy(profile.gameObject);
            }

            return count;
        }
        
        public int RemoveDisposedObjects()
        {
            return profiles.RemoveAll(p => p.disposed);
        }
        
        public int RemoveGroupObjects(int groupId)
        {
            return profiles.RemoveAll(p => p.groupId == groupId);
        }
        
        private void SceneUnloaded(Scene scene)
        {
            RemoveGroupObjects(scene.handle);

            RemoveDisposedObjects();
        }
        
        private void OnEnable()
        {
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= SceneUnloaded;
        }
    }
}