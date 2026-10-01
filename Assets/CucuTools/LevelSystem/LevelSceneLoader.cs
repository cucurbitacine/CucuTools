using UnityEngine;

namespace CucuTools.LevelSystem
{
    public class LevelSceneLoader : MonoBehaviour
    {
        [SerializeField] private LevelSceneAsset levelScene;

        [ContextMenu(nameof(LoadLevelScene))]
        public async void LoadLevelScene()
        {
            await LoadLevelSceneAsync();
        }

        public AsyncOperation LoadLevelSceneAsync()
        {
            return LevelManager.Instance.LoadSceneAsync(levelScene);
        }
        
        public void ChangeLevelScene(LevelSceneAsset newLevelScene)
        {
            levelScene = newLevelScene;
        }

        public LevelSceneAsset GetLevelScene()
        {
            return levelScene;
        }
    }
}