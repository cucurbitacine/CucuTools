using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class PlayerInput2D : Player2DInput<PlayerPlatformController>
    {
        public bool active = true;
        public float horizontal = 0f;

        private void Update()
        {
            if (!active) return;
            
            horizontal = Input.GetAxisRaw("Horizontal");
            
            player.Move(horizontal);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.Jump();
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