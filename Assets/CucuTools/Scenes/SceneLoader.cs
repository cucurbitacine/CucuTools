using System;
using System.Collections.Generic;
using CucuTools.Attributes;
using CucuTools.Injects;

namespace CucuTools.Scenes
{
    [Serializable]
    public class SceneLoader
    {
        [CucuScene]
        public string sceneName = string.Empty;
        
        private readonly List<CucuArg> _args = new List<CucuArg>();
        
        public void AddArg(CucuArg arg)
        {
            _args.Add(arg);  
        }
        
        public void RemoveArg(CucuArg arg)
        {
            _args.Remove(arg); 
        }
        
        public void AddArgs(params CucuArg[] args)
        {
            foreach (var arg in args)
                AddArg(arg);
        }

        public void RemoveArgs(params CucuArg[] args)
        {
            foreach (var arg in args)
                RemoveArg(arg);   
        }

        public void ClearArgs()
        {
            _args.Clear();
        }
        
        public void LoadSingleScene()
        {
            LoadSingleScene(_args.ToArray());
        }
        
        public void LoadAdditiveScene()
        {
            LoadAdditiveScene(_args.ToArray());
        }
        
        public void LoadSingleSceneAsync()
        {
            LoadSingleSceneAsync(_args.ToArray());
        }
        
        public void LoadAdditiveSceneAsync()
        {
            LoadAdditiveSceneAsync(_args.ToArray());
        }
        
        public void LoadSingleScene(CucuArg[] args)
        {
            CucuSceneManager.LoadSingleScene(sceneName, args);
        }

        public void LoadAdditiveScene(CucuArg[] args)
        {
            CucuSceneManager.LoadAdditiveScene(sceneName, args);
        }
        
        public void LoadSingleSceneAsync(CucuArg[] args)
        {
            CucuSceneManager.LoadSingleSceneAsync(sceneName, args);
        }
        
        public void LoadAdditiveSceneAsync(CucuArg[] args)
        {
            CucuSceneManager.LoadAdditiveSceneAsync(sceneName, args);
        }
    }
}
