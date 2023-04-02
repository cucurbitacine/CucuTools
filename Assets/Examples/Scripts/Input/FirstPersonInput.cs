using CucuTools.PlayerSystem.Impl;
using UnityEngine;

namespace Examples.Scripts.Input
{
    public class FirstPersonInput : RigidPersonInput<FirstPersonRigidController>
    {
        [Header("First Person Settings")]
        [Min(0)] public float fovChangeRate = 8;
        
        protected override void UpdatePerson(float deltaTime)
        {
            person.Move(move);

            person.View(Vector2.Scale(view, viewSens));
        
            if (jump) person.Jump();

            person.settings.moveSpeedModificator = run ? runScale : 1f;
        }

        protected override void UpdateCamera(float deltaTime)
        {
            cam.transform.position = person.head.position;
            cam.transform.rotation = person.head.rotation;

            var fov = fovIdle;
            if (aim) fov += fovAimOffset;
            if (run && person.info.moving) fov += fovRunOffset;
            
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, fovChangeRate * deltaTime);
        }

        private void OnValidate()
        {
            if (cam == null) cam = Camera.main;

            if (gameObject.activeInHierarchy && cam != null && person != null)
            {
                cam.transform.position = person.head.position;
                cam.transform.rotation = person.head.rotation;
            }
        }
    }
}