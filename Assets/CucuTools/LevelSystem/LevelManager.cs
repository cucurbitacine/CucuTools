using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CucuTools.LevelSystem
{
    public sealed class LevelManager
    {
        public static LevelManager Instance { get; private set; }

        static LevelManager()
        {
            Instance = new LevelManager();
        }

        public event Action<LevelController> LevelLoaded;

        public Scene GetActiveScene()
        {
            return SceneManager.GetActiveScene();
        }

        public AsyncOperation LoadSceneAsync(int sceneBuildIndex, LoadSceneParameters sceneParameters, LevelParameters levelParameters)
        {
            levelParameters.PreviousSceneId = GetActiveScene().buildIndex;
            
            var loadingScene = SceneManager.LoadSceneAsync(sceneBuildIndex, sceneParameters);

            if (loadingScene != null)
            {
                loadingScene.completed += loading =>
                {
                    var scene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
                    OnSceneLoaded(scene, levelParameters);
                };
            }
            
            return loadingScene;
        }
        
        public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneParameters sceneParameters, LevelParameters levelParameters)
        {
            levelParameters.PreviousSceneId = GetActiveScene().buildIndex;
            
            var loadingScene = SceneManager.LoadSceneAsync(sceneName, sceneParameters);

            if (loadingScene != null)
            {
                loadingScene.completed += loading =>
                {
                    var scene = SceneManager.GetSceneByName(sceneName);
                    OnSceneLoaded(scene, levelParameters);
                };
            }
            
            return loadingScene;
        }
        
        private void OnSceneLoaded(Scene scene, LevelParameters levelParameters)
        {
            foreach (var gameObject in scene.GetRootGameObjects())
            {
                if (gameObject.TryGetComponent(out LevelController level))
                {
                    OnLevelLoaded(level, levelParameters);
                    return;
                }
            }
        }
        
        private void OnLevelLoaded(LevelController level, LevelParameters levelParameters)
        {
            level.SetLevelParameters(levelParameters);
            
            LevelLoaded?.Invoke(level);
        }
    }

    public static class LevelManagerExt
    {
        // SCENE NAME
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, string sceneName, LoadSceneParameters sceneParameters)
        {
            return manager.LoadSceneAsync(sceneName, sceneParameters, default);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, string sceneName, LevelParameters levelParameters)
        {
            return manager.LoadSceneAsync(sceneName, default, levelParameters);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, string sceneName)
        {
            return manager.LoadSceneAsync(sceneName, default, default);
        }
        
        // BUILD INDEX
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, int sceneBuildIndex, LoadSceneParameters sceneParameters)
        {
            return manager.LoadSceneAsync(sceneBuildIndex, sceneParameters, default);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, int sceneBuildIndex, LevelParameters levelParameters)
        {
            return manager.LoadSceneAsync(sceneBuildIndex, default, levelParameters);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, int sceneBuildIndex)
        {
            return manager.LoadSceneAsync(sceneBuildIndex, default, default);
        }
        
        // LEVEL SCENE
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, LevelSceneAsset levelScene, LoadSceneParameters sceneParameters, LevelParameters levelParameters)
        {
            return manager.LoadSceneAsync(levelScene.GetSceneBuildIndex(), sceneParameters, levelParameters);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, LevelSceneAsset levelScene, LoadSceneParameters sceneParameters)
        {
            return manager.LoadSceneAsync(levelScene.GetSceneBuildIndex(), sceneParameters, default);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, LevelSceneAsset levelScene, LevelParameters levelParameters)
        {
            return manager.LoadSceneAsync(levelScene.GetSceneBuildIndex(), levelScene.GetLoadSceneParameters(), levelParameters);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, LevelSceneAsset levelScene)
        {
            return manager.LoadSceneAsync(levelScene.GetSceneBuildIndex(), levelScene.GetLoadSceneParameters(), default);
        }
    }
}