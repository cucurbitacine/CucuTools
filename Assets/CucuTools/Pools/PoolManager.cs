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

        public void Create(GameObject prefab, out PoolObject poolObject)
        {
            if (prefab == null) throw new NullReferenceException();
            
            var activeScene = SceneManager.GetActiveScene();
            
            var prefabId = prefab.GetInstanceID();
            var sceneId = activeScene.handle;

            if (TryGetReleasedObject(prefabId, activeScene, out var releasedProfile))
            {
                poolObject = releasedProfile.poolObject;
            }
            else
            {
                var createdObject = Instantiate(prefab);
                
                poolObject = createdObject.GetComponent<PoolObject>();
                
                if (poolObject == null)
                {
                    poolObject = createdObject.AddComponent<PoolObject>();
                }
                
                AddPoolObject(poolObject, prefabId, sceneId);
            }
        }
        
        public GameObject Create(GameObject prefab)
        {
            Create(prefab, out var poolObject);
            
            return poolObject.gameObject;
        }
        
        public T Create<T>(T prefab) where T : Component
        {
            return Create(prefab.gameObject).GetComponent<T>();
        }

        public bool Add(GameObject prefab, GameObject clone)
        {
            if (prefab == null) throw new NullReferenceException();
            if (clone == null) throw new NullReferenceException();
            
            var poolObject = clone.GetComponent<PoolObject>();

            if (poolObject)
            {
                if (profiles.Any(p => p.available && p.poolObject == poolObject))
                {
                    return false;
                }
            }
            else
            {
                poolObject = clone.AddComponent<PoolObject>();
            }

            var activeScene = SceneManager.GetActiveScene();
            
            var prefabId = prefab.GetInstanceID();
            var sceneId = activeScene.handle;
            
            AddPoolObject(poolObject, prefabId, sceneId);
            return true;
        }

        public bool Add<T>(T prefab, T clone) where T : Component
        {
            return Add(prefab.gameObject, clone.gameObject);
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

        private bool TryGetReleasedObject(int prefabId, Scene scene, out PoolObjectProfile profileReleased)
        {
            var sceneId = scene.handle;
            
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
                        SceneManager.MoveGameObjectToScene(profile.gameObject, scene);
                        profile.groupId = sceneId;
                    }

                    if (profile.poolObject.releaseOnDisable && !profile.gameObject.activeSelf)
                    {
                        profile.gameObject.SetActive(true);
                    }
                    
                    profile.released = false;
                    
                    profileReleased = profile;
                    return true;
                }
            }

            profileReleased = null;
            return false;
        }
        
        private void AddPoolObject(PoolObject poolObject, int prefabId, int sceneId)
        {
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