using UnityEngine;
using UnityEngine.EventSystems;

namespace Samples.DamageSystem.Scripts
{
    public class DamageSceneController : MonoBehaviour
    {
        public PlayerDamageSource playerDamageSource;

        private void MouseDown()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            playerDamageSource.Attack();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) MouseDown();
        }
    }
}
