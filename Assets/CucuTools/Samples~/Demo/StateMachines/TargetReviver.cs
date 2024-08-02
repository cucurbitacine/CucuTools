using UnityEngine;

namespace Samples.Demo.StateMachines
{
    public class TargetReviver : MonoBehaviour
    {
        public float reviveDuration = 2f;
    
        public void Revive()
        {
            gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            Invoke(nameof(Revive), reviveDuration);
        }
    }
}
