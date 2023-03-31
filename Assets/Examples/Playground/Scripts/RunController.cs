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
        public PlayerInput input;
        
        [Space]
        [Min(0f)]
        public float runFieldOfView = 70;
        public float fovDamp = 4;
        public Camera playerCamera;
        
        public PersonController Person => input.person;

        private float _fieldOfView;
        
        private void Start()
        {
            _fieldOfView = playerCamera.fieldOfView;
        }

        private void Update()
        {
            Person.settings.moveSpeed = Person.settings.moveSpeedMax;

            if (input.run)
            {
                Person.settings.moveSpeed = runSpeedMax;
            }
            else if (input.sneak)
            {
                Person.settings.moveSpeed = sneakSpeedMax;
            }

            var fov = input.run && input.person.info.moving ? runFieldOfView : _fieldOfView;
            
            fov = input.sneak ? _fieldOfView - (runFieldOfView - _fieldOfView) : fov;
            
            
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, fovDamp * Time.deltaTime);
        }
    }
}
