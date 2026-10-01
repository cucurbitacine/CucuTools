using UnityEngine;
using UnityEngine.SceneManagement;

namespace CucuTools.LevelSystem
{
    [CreateAssetMenu(menuName = "CucuTools/Level System/Level Scene Asset", fileName = "LevelScene", order = 0)]
    public class LevelSceneAsset : ScriptableObject
    {
        [SerializeField] private int sceneBuildIndex;
        
        [Space]
        [SerializeField] private LoadSceneParameters loadSceneParameters;

        public int GetSceneBuildIndex()
        {
            return sceneBuildIndex;
        }

        public LoadSceneParameters GetLoadSceneParameters()
        {
            return loadSceneParameters;
        }
    }
}