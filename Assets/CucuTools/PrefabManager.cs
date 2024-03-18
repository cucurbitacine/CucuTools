using System;
using System.Collections.Generic;
using CucuTools.Attributes;
using CucuTools.Others;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CucuTools
{
    public class PrefabManager : Singleton<PrefabManager>
    {
        [Header("Prefab Manager")]
        public bool debugLog = false;
        
        public readonly List<CloneProfile> profiles = new List<CloneProfile>();
            
        public GameObject Create(GameObject prefab)
        {
            var activeScene = SceneManager.GetActiveScene();

            foreach (var profile in profiles)
            {
                if (profile.available && profile.free && profile.prefab == prefab)
                {
                    if (debugLog)
                    {
                        Debug.Log($"[{name}] :: Found \"{profile.gameObject.name}\" in ({profile.scene.name}) scene.");   
                    }
                    
                    if (profile.scene != activeScene)
                    {
                        SceneManager.MoveGameObjectToScene(profile.gameObject, activeScene);
                        profile.scene = activeScene;
                        
                        if (debugLog)
                        {
                            Debug.Log($"[{name}] :: \"{profile.gameObject.name}\" was moved to ({profile.scene.name}) scene.");   
                        }
                    }

                    profile.free = false;
                    if (profile.clone is AutoClone autoClone && autoClone.autoFree)
                    {
                        profile.gameObject.SetActive(true);
                    }
                    return profile.gameObject;
                }
            }

            var go = Instantiate(prefab);
            var clone = go.GetComponent<CloneBehaviour>();
            if (clone == null) clone = go.AddComponent<AutoClone>();

            var newProfile = new CloneProfile()
            {
                available = true,
                free = false,
                prefab = prefab,
                clone = clone,
                scene = activeScene,
            };

            clone.profile = newProfile;

            profiles.Add(newProfile);
            
            if (debugLog)
            {
                Debug.Log($"[{name}] :: Created \"{newProfile.gameObject.name}\" in ({newProfile.scene.name}) scene.");
            }
            
            return newProfile.gameObject;
        }
        
        public T Create<T>(T prefab) where T : Component
        {
            return Create(prefab.gameObject).GetComponent<T>();
        }

        [DrawButton]
        public int Cleanup()
        {
            return profiles.RemoveAll(p => !p.available);
        }
        
        private void SceneUnloaded(Scene scene)
        {
            var count = profiles.RemoveAll(p => p.scene == scene);

            if (debugLog && count > 0)
            {
                Debug.Log($"[{name}] :: {count} irrelevant profiles were removed in ({scene.name}) scene");
            }

            count = Cleanup();

            if (debugLog && count > 0)
            {
                Debug.Log($"[{name}] :: {count} invalid profiles were removed");
            }
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
    
    [DisallowMultipleComponent]
    public abstract class CloneBehaviour : CucuBehaviour
    {
        public CloneProfile profile { get; set; }

        public void Available(bool value)
        {
            if (profile != null && profile.available != value)
            {
                profile.available = value;
            }
        }

        public void Free(bool value = true)
        {
            if (profile != null && profile.free != value)
            {
                profile.free = value;
            }
        }

        private void Awake()
        {
            Available(true);
        }
        
        private void OnDestroy()
        {
            Available(false);
        }
    }
    
    [Serializable]
    public class CloneProfile
    {
        public bool available;
        public bool free;
        public CloneBehaviour clone;
        
        [Space]
        public GameObject prefab;
        public Scene scene;

        public GameObject gameObject => clone.gameObject;
    }
}