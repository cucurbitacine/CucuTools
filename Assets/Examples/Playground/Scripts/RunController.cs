using CucuTools.PlayerSystem;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class RunController : MonoBehaviour
    {
        [Min(0f)]
        public float runSpeedMax = 4f;
        [Min(0f)]
        public float sneakSpeedMax = 0.5f;
        public PlayerRigidInput input;
        
        [Space]
        [Min(0f)]
        public float runFieldOfView = 70;
        public float fovDamp = 4;
        public Camera playerCamera;
        
        public PlayerController player => input.player;

        private float _fieldOfView;
        
        private void Start()
        {
            _fieldOfView = playerCamera.fieldOfView;
        }

        private void Update()
        {
            player.settings.speed = player.settings.speedMax;

            if (input.run)
            {
                player.settings.speed = runSpeedMax;
            }
            else if (input.sneak)
            {
                player.settings.speed = sneakSpeedMax;
            }

            var fov = input.run && input.player.info.isMoving ? runFieldOfView : _fieldOfView;
            
            fov = input.sneak ? _fieldOfView - (runFieldOfView - _fieldOfView) : fov;
            
            
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, fovDamp * Time.deltaTime);
        }
    }
}
