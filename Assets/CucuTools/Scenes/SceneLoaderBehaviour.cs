using UnityEngine;
using UnityEngine.SceneManagement;

namespace CucuTools.Scenes
{
    public class SceneLoaderBehaviour : MonoBehaviour
    {
        public SceneLoader loader = new SceneLoader();
        
        public void AddArg(CucuArg arg)
        {
            loader.args.Add(arg);
        }

        public void RemoveArg(CucuArg arg)
        {
            loader.args.Remove(arg);
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
            loader.args.Clear();
        }
        
        public void LoadSingleScene()
        {
            loader.LoadSingleScene();
        }

        public void LoadAdditiveScene()
        {
            loader.LoadAdditiveScene();
        }

        public void LoadSingleSceneAsync()
        {
            loader.LoadSingleSceneAsync();
        }

        public void LoadAdditiveSceneAsync()
        {
            loader.LoadAdditiveSceneAsync();
        }

        public void LoadScene(LoadSceneMode mode)
        {
            loader.LoadScene(mode);
        }

        public void LoadSceneAsync(LoadSceneMode mode)
        {
            loader.LoadSceneAsync(mode);
        }
    }
}