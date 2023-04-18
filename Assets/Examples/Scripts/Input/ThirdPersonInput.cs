using UnityEngine;

namespace Examples.Scripts.Input
{
    public class ThirdPersonInput : BasePersonInput
    {
        [Header("Third Person Settings")]
        [Space] public float shoulder = 0.5f;
        [Min(0f)] public float height = 3f;
        [Min(0f)] public float distance = 4f;
        [Range(0, 90)] public float topClamp = 70.0f;
        [Range(-90, 0)] public float bottomClamp = -30.0f;
        [Min(0f)] public float camPosChangeRate = 8;
        [Min(0)] public float camRotChangeRate = 8;
        
        private float _camTargetYaw;
        private float _camTargetPitch;
        
        protected override void UpdatePerson(float deltaTime)
        {
            var moveDir = Vector3.ProjectOnPlane(cam.transform.TransformDirection(data.move), playerTyped.normal).normalized;
            player.MoveInDirection(moveDir);
            
            var lookDir = data.aim || data.dragging ? cam.transform.forward : Vector3.ProjectOnPlane(moveDir, playerTyped.normal);
            //var lookDir = cam.transform.forward;
            player.LookInDirection(lookDir);
            
            if (data.jump) player.Jump();

            player.settings.moveSpeedModificator = data.run ? runScale : 1f;

            if (drag) drag.inputDragging = data.dragging;
        }

        private Quaternion GetCamRotation()
        {
            _camTargetYaw = ClampAngle(_camTargetYaw, float.MinValue, float.MaxValue);
            _camTargetPitch = ClampAngle(_camTargetPitch, bottomClamp, topClamp);

            return Quaternion.Euler(_camTargetPitch, _camTargetYaw, 0.0f);
        }

        private Vector3 GetCamPosition()
        {
            var pos = player.position - cam.transform.forward * distance + playerTyped.normal * height;
            
            var right = -Vector3.Cross(Vector3.ProjectOnPlane(player.position - pos, playerTyped.normal),
                playerTyped.normal).normalized;

            pos += right * shoulder;

            var headPos = player.head.position;
            var direction = pos - headPos;
            if (Physics.Raycast(headPos, direction, out var hit, direction.magnitude, player.ground.layerGround))
            {
                pos = hit.point + hit.normal * 0.01f;
            }
            
            return pos;
        }
        
        protected override void UpdateCamera(float deltaTime)
        {
            _camTargetYaw += data.view.x * viewSens.x;
            _camTargetPitch += -data.view.y * viewSens.y;

            var targetRotation = GetCamRotation();
            targetRotation = Quaternion.Lerp(cam.transform.rotation, targetRotation, camRotChangeRate * deltaTime);
            cam.transform.rotation = targetRotation;

            var targetPosition = GetCamPosition();
            targetPosition.y = Mathf.Lerp(cam.transform.position.y, targetPosition.y, camPosChangeRate * deltaTime);
            cam.transform.position = targetPosition;
            
            var fov = fovIdle;
            if (data.aim) fov += fovAimOffset;
            if (data.run && player.info.moving) fov += fovRunOffset;
            
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, camRotChangeRate * deltaTime);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        protected override void Awake()
        {
            base.Awake();
            
            vision.eyes = cam.transform;
            touch.origin = player.head;
            
            _camTargetYaw = vision.eyes.rotation.eulerAngles.y;
        }
        
        private void OnValidate()
        {
            if (cam == null) cam = Camera.main;

            if (gameObject.activeInHierarchy && cam != null && player != null && !Application.isPlaying)
            {
                cam.transform.rotation = GetCamRotation();
                cam.transform.position = GetCamPosition();
            }
        }
    }
}