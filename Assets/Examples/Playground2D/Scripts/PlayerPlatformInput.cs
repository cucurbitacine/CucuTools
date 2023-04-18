using System;
using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Examples.Playground2D.Scripts
{
    public class PlayerPlatformInput : Player2DInput<PlayerPlatformController>
    {
        public Camera cam = null;
        public float camDamp = 4f;
        public bool active = true;

        [Space]
        [Min(1f)] public float runSpeedModificator = 2;
        
        [Space]
        public InputData2D data = default;

        public Vector2 gravity => gravityPower * gravityDirection.normalized;

        public bool changeGravity = true;
        public float speedChange = 0f;
        public Vector2 gravityDirection = Vector2.down;
        public float gravityPower = 9.81f;
        
        [Range(-180, 180)]
        public float gravityRotation = 0;

        private void UpdateGravity()
        {
            gravityDirection = Quaternion.Euler(0, 0, gravityRotation) * Vector2.down;

            player.settings.gravity = gravity;
        }
        
        private void UpdateInput()
        {
            data.move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            data.move = Vector2.ClampMagnitude(data.move, 1);

            data.run = Input.GetKey(KeyCode.LeftShift);
            data.jump = Input.GetKeyDown(KeyCode.Space);

            player.settings.moveSpeedModificator = data.run ? runSpeedModificator : 1f;
            
            player.Move(data.move);

            if (data.jump) player.Jump();
        }

        private void Awake()
        {
            if (cam == null) cam = Camera.main;
        }

        private void Update()
        {
            if (changeGravity)
            {
                var rota = Mathf.Repeat(gravityRotation + 180 + speedChange * Time.deltaTime, 360);
                gravityRotation = rota - 180;
                
                UpdateGravity();
            }
            
            if (active)
            {
                UpdateInput();
            }
            else
            {
                player.Stop();
            }

            var pos = (Vector3)player.position + Vector3.forward * cam.transform.position.z;
            cam.transform.position = Vector3.Lerp(cam.transform.position, pos, camDamp * Time.deltaTime);
            
            var rot = Quaternion.LookRotation(Vector3.forward, -gravity.normalized);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, rot, camDamp * Time.deltaTime);
        }
    }
    
    [Serializable]
    public struct InputData2D
    {
        public Vector2 move;
        
        [Space] 
        public bool run;
        public bool jump;
    }
}