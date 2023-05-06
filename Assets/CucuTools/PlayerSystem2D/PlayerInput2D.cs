using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class PlayerInput2D : CucuBehaviour
    {
        public bool active = true;
        
        [Space]
        public float move = 0f;
        public bool run = false;
        public bool sneak = false;
        public bool jump = false;
        public bool down = false;
        public bool dodgeLeft = false;
        public bool dodgeRight = false;
        public bool freeze = false;
  
        [Space]
        public PlayerController2D player;
        
        [Space]
        public SpeedController2D speedController;
        public JumpController2D jumpController;
        public DodgeController2D dodgeController;

        private void UpdateInput()
        {
            move = Input.GetAxisRaw("Horizontal");
            run = Input.GetKey(KeyCode.LeftShift);
            
            jump = Input.GetKeyDown(KeyCode.Space);
            down = Input.GetKeyDown(KeyCode.S);
            
            freeze = Input.GetKeyDown(KeyCode.F);

            dodgeLeft = Input.GetKeyDown(KeyCode.Q);
            dodgeRight = Input.GetKeyDown(KeyCode.E);

            sneak = Input.GetKeyDown(KeyCode.C) ? !sneak : sneak;

            if (run || jump || down || dodgeLeft || dodgeRight)
            {
                sneak = false;
            }
        }

        private void UpdatePlayer()
        {
            player.sleep = dodgeController && dodgeController.dodging;

            if (speedController)
            {
                if (!player.grounded)
                {
                    sneak = false;
                }
            
                speedController.running = run;
                speedController.sneaking = sneak;
            
                speedController.Move(move);
            }
            else
            {
                player.Move(move);
            }
            
            if (jump)
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

            if (down)
            {
                player.Down();
            }

            if (dodgeLeft)
            {
                if (dodgeController)
                {
                    dodgeController.Dodge(-player.playerRight);
                }
            }
            else if (dodgeRight)
            {
                if (dodgeController)
                {
                    dodgeController.Dodge(player.playerRight);
                }
            }
            
            if (freeze)
            {
                player.freeze = !player.freeze;
            } 
        }
        
        private void Update()
        {
            UpdateInput();

            if (active && player) UpdatePlayer();
        }
    }
}