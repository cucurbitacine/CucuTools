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

        public AsyncOperation LoadSceneAsync(int sceneBuildIndex, LoadSceneParameters sceneParameters, LevelParams levelParams)
        {
            if (levelParams == null) levelParams = new LevelParams();
            levelParams.PreviousSceneId = GetActiveScene().buildIndex;
            
            var loadingScene = SceneManager.LoadSceneAsync(sceneBuildIndex, sceneParameters);

            if (loadingScene != null)
            {
                loadingScene.completed += loading =>
                {
                    var scene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
                    OnSceneLoaded(scene, levelParams);
                };
            }
            
            return loadingScene;
        }
        
        public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneParameters sceneParameters, LevelParams levelParams)
        {
            if (levelParams == null) levelParams = new LevelParams();
            levelParams.PreviousSceneId = GetActiveScene().buildIndex;
            
            var loadingScene = SceneManager.LoadSceneAsync(sceneName, sceneParameters);

            if (loadingScene != null)
            {
                loadingScene.completed += loading =>
                {
                    var scene = SceneManager.GetSceneByName(sceneName);
                    OnSceneLoaded(scene, levelParams);
                };
            }
            
            return loadingScene;
        }
        
        private void OnSceneLoaded(Scene scene, LevelParams levelParams)
        {
            foreach (var gameObject in scene.GetRootGameObjects())
            {
                if (gameObject.TryGetComponent(out LevelController level))
                {
                    OnLevelLoaded(level, levelParams);
                    return;
                }
            }
        }
        
        private void OnLevelLoaded(LevelController level, LevelParams levelParams)
        {
            level.SetParams(levelParams);
            
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
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, string sceneName, LevelParams levelParams)
        {
            return manager.LoadSceneAsync(sceneName, default, levelParams);
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
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, int sceneBuildIndex, LevelParams levelParams)
        {
            return manager.LoadSceneAsync(sceneBuildIndex, default, levelParams);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, int sceneBuildIndex)
        {
            return manager.LoadSceneAsync(sceneBuildIndex, default, default);
        }
        
        // LEVEL SCENE
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, LevelSceneAsset levelScene, LoadSceneParameters sceneParameters, LevelParams levelParams)
        {
            return manager.LoadSceneAsync(levelScene.GetSceneBuildIndex(), sceneParameters, levelParams);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, LevelSceneAsset levelScene, LoadSceneParameters sceneParameters)
        {
            return manager.LoadSceneAsync(levelScene.GetSceneBuildIndex(), sceneParameters, null);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, LevelSceneAsset levelScene, LevelParams levelParams)
        {
            return manager.LoadSceneAsync(levelScene.GetSceneBuildIndex(), levelScene.GetLoadSceneParameters(), levelParams);
        }
        
        public static AsyncOperation LoadSceneAsync(this LevelManager manager, LevelSceneAsset levelScene)
        {
            return manager.LoadSceneAsync(levelScene.GetSceneBuildIndex(), levelScene.GetLoadSceneParameters(), null);
        }
    }
}