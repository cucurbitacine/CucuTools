using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Samples.Playground2D.Scripts.Tools
{
    public class PlayerInput2D : ToolController2D
    {
        [Header("Input")]
        public bool active = true;
        
        [Space]
        public float move = 0f;
        public bool run = false;
        public bool jump = false;
        public bool down = false;
        public bool dodgeLeft = false;
        public bool dodgeRight = false;
        public bool freeze = false;

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
        }

        private void UpdatePlayer()
        {
            if (speedController)
            {
                speedController.running = run;
            
                speedController.Move(move);
            }
            else
            {
                player2d.Move(move);
            }
            
            if (jump)
            {
                if (jumpController)
                {
                    jumpController.Jump();
                }
                else
                {
                    player2d.Jump();    
                }
            }

            if (down)
            {
                player2d.Down();
            }

            if (dodgeLeft)
            {
                if (dodgeController)
                {
                    dodgeController.Dodge(-player2d.playerRight);
                }
            }
            else if (dodgeRight)
            {
                if (dodgeController)
                {
                    dodgeController.Dodge(player2d.playerRight);
                }
            }
            
            if (freeze)
            {
                player2d.ChangeFreeze();
            } 
        }

        private void Start()
        {
            foreach (var tool in GetComponents<ToolController2D>())
            {
                if (tool != this) tool.player2d = player2d;
            }
        }

        private void Update()
        {
            UpdateInput();

            if (active && player2d) UpdatePlayer();
        }
    }
}