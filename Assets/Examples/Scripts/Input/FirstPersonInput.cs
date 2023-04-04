using CucuTools.PlayerSystem.Impl;
using UnityEngine;

namespace Examples.Scripts.Input
{
    public class FirstPersonInput : RigidPersonInput<FirstPersonRigidController>
    {
        [Header("First Person Settings")]
        [Min(0)]
        public float fovChangeRate = 8;

        public Vector3 camOffset = Vector3.zero;
        
        protected override void UpdatePerson(float deltaTime)
        {
            person.Move(data.move);

            person.View(Vector2.Scale(data.view, viewSens));
        
            if (data.jump) person.Jump();

            person.settings.moveSpeedModificator = data.run ? runScale : 1f;
        }

        protected override void UpdateCamera(float deltaTime)
        {
            cam.transform.rotation = person.head.rotation;
            cam.transform.position = person.head.TransformPoint(camOffset);
            
            var fov = fovIdle;
            if (data.aim) fov += fovAimOffset;
            if (data.run && person.info.moving) fov += fovRunOffset;
            
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, fovChangeRate * deltaTime);
        }

        protected override void Awake()
        {
            base.Awake();
            
            vision.eyes = cam.transform;
            touch.origin = vision.eyes;
        }

        private void OnValidate()
        {
            if (cam == null) cam = Camera.main;

            if (gameObject.activeInHierarchy && cam != null && person != null)
            {
                cam.transform.rotation = person.head.rotation;
                cam.transform.position = person.head.TransformPoint(camOffset);
            }
        }
    }
}