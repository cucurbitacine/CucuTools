using System.Collections;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    public class CameraController : MonoBehaviour, IContextable
    {
        public virtual Camera CameraMain => Camera.main;

        public virtual void Init(ContextContainer context)
        {
        }
        
        public virtual IEnumerator EnableCamera()
        {
            yield break;
        }

        public virtual void DisableCamera()
        {
        }
    }
}