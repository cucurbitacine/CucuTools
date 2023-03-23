using CucuTools.Injects;

namespace CucuTools.Scenes
{
    /// <summary>
    /// Base behaviour for scene controller
    /// </summary>
    public abstract class SceneController : InjectMonoBehaviour
    {
        protected override void BeforeInject()
        {
        }

        protected override void OnAwake()
        {
        }
    }
}