using CucuTools.Scenes;
using UnityEngine;

namespace Examples.MainMenu
{
    [CreateAssetMenu(menuName = "Examples/Create Scene Reference", fileName = "Scene Reference", order = 0)]
    public class SceneReference : ScriptableObject
    {
        public string displayName = string.Empty;
        
        [Multiline]
        public string description = string.Empty;
        
        [Space]
        public SceneLoader loader;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(displayName) && loader != null)
            {
                displayName = loader.sceneName;
            } 
        }
    }
}