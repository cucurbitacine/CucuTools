using System.Collections;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    public class CameraController : MonoBehaviour
    {
        public virtual Camera CameraMain => Camera.main;

        public virtual IEnumerator EnableCamera(ContextContainer levelContext)
        {
            yield break;
        }

        public virtual void DisableCamera()
        {
        }
    }
}