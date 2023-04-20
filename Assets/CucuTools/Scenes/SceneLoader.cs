using System;
using System.Collections.Generic;
using CucuTools.Attributes;
using UnityEngine.SceneManagement;

namespace CucuTools.Scenes
{
    [Serializable]
    public class SceneLoader 
    {
        [SceneSelect]
        public string sceneName = string.Empty;

        public readonly List<CucuArg> args = new List<CucuArg>();

        public void LoadSingleScene()
        {
            CucuSceneManager.LoadSingleScene(sceneName, args.ToArray());
        }

        public void LoadAdditiveScene()
        {
            CucuSceneManager.LoadAdditiveScene(sceneName, args.ToArray());
        }

        public void LoadSingleSceneAsync()
        {
            CucuSceneManager.LoadSingleSceneAsync(sceneName, args.ToArray());
        }

        public void LoadAdditiveSceneAsync()
        {
            CucuSceneManager.LoadAdditiveSceneAsync(sceneName, args.ToArray());
        }

        public void LoadScene(LoadSceneMode mode)
        {
            switch (mode)
            {
                case LoadSceneMode.Single:
                    LoadSingleScene();
                    break;
                case LoadSceneMode.Additive:
                    LoadAdditiveScene();
                    break;
            }
        }

        public void LoadSceneAsync(LoadSceneMode mode)
        {
            switch (mode)
            {
                case LoadSceneMode.Single:
                    LoadSingleSceneAsync();
                    break;
                case LoadSceneMode.Additive:
                    LoadAdditiveSceneAsync();
                    break;
            }
        }
    }
}