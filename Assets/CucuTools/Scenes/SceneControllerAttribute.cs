using System;

namespace CucuTools.Scenes
{
    /// <summary>
    /// Attribute for class of scene controller.
    /// Set scene name
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SceneControllerAttribute : Attribute
    {
        public string SceneName { get; }
        
        public SceneControllerAttribute()
        {
            SceneName = string.Empty;
        }
        
        public SceneControllerAttribute(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}