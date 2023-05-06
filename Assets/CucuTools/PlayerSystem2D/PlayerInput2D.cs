using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class PlayerInput2D : CucuBehaviour
    {
        public bool active = true;
        public float move = 0f;

        [Header("Gravity")]
        public bool changeGravity = false;
        public bool autoChangeGravity = false;
        public float gravityChangeSpeed = 10;
        [Range(-180, 180)]
        public float gravityAngle = 0;
        
        [Space]
        public PlayerController2D player;
        public JumpController2D jumpController;
        
        private void Update()
        {
            if (changeGravity)
            {
                if (autoChangeGravity)
                {
                    gravityAngle = Mathf.Repeat(gravityAngle + gravityChangeSpeed * Time.deltaTime + 180f, 360) - 180f;
                }
                
                Physics2D.gravity = Quaternion.Euler(0, 0, gravityAngle) * Vector2.down * Physics2D.gravity.magnitude;
            }
            
            if (!active) return;

            if (!player) return;

            player.autoUpdateNormal = changeGravity;
            
            move = Input.GetAxisRaw("Horizontal");
            
            player.Move(move);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpController)
                {
                    jumpController.Jump();
                }
                else
                {
                    player.Jump();    
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                player.Down();
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                player.freeze = !player.freeze;
            } 
        }
    }
}