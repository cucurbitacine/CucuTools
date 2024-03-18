using System;
using CucuTools;
using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Samples.Demo.FX
{
    public class DevSceneLoader : Singleton<DevSceneLoader>
    {
        [Header("Scene Loader")]
        public string sceneName;
        public LoadSceneMode loadSceneMode;

        [Space]
        [ReadOnlyField]
        public string lastLoadedSceneName;
        public Scene lastLoadedScene;
        
        [DrawButton]
        public void Load()
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
        }
        
        [DrawButton]
        public void ActiveLastLoadedScene()
        {
            SceneManager.SetActiveScene(lastLoadedScene);
        }
        
        [DrawButton]
        public void Unload()
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        
        public void SceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            lastLoadedScene = scene;
            lastLoadedSceneName = lastLoadedScene.name;
            
            Debug.Log($"[{scene.handle}] {scene.name} was loaded{((int)sceneMode < 2 ? $" like {sceneMode}" : "")}.");
        }

        public void SceneUnloaded(Scene scene)
        {
            Debug.Log($"[{scene.handle}] {scene.name} was unloaded.");
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            SceneManager.sceneUnloaded -= SceneUnloaded;
        }

        private void OnGUI()
        {
            sceneName = GUILayout.TextField(sceneName);

            loadSceneMode = (LoadSceneMode)GUILayout.Toolbar((int)loadSceneMode, Enum.GetNames(typeof(LoadSceneMode)));
            
            if (GUILayout.Button("Load"))
            {
                Load();
            }
            
            if (GUILayout.Button("Unload"))
            {
                Unload();
            }
            
            if (GUILayout.Button($"Set Active: {lastLoadedScene.name}"))
            {
                ActiveLastLoadedScene();
            }
        }
    }
}