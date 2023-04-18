using UnityEngine;

namespace Examples.Scripts.Input
{
    public class FirstPersonInput : BasePersonInput
    {
        [Header("First Person Settings")]
        [Min(0)]
        public float fovChangeRate = 8;

        public Vector3 camOffset = Vector3.zero;
        
        protected override void UpdatePerson(float deltaTime)
        {
            player.Move(data.move);

            player.View(Vector2.Scale(data.view, viewSens));
        
            if (data.jump) player.Jump();

            player.settings.moveSpeedModificator = data.run ? runScale : 1f;
        }

        protected override void UpdateCamera(float deltaTime)
        {
            cam.transform.rotation = player.head.rotation;
            cam.transform.position = player.head.TransformPoint(camOffset);
            
            var fov = fovIdle;
            if (data.aim) fov += fovAimOffset;
            if (data.run && player.info.moving) fov += fovRunOffset;
            
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

            if (gameObject.activeInHierarchy && cam != null && player != null)
            {
                cam.transform.rotation = player.head.rotation;
                cam.transform.position = player.head.TransformPoint(camOffset);
            }
        }
    }
}