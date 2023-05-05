using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class PlayerInput2D : CucuBehaviour
    {
        public bool active = true;
        public float move = 0f;

        [Space]
        public PlayerController2D player;
        
        private void Update()
        {
            if (!active) return;
            
            move = Input.GetAxisRaw("Horizontal");
            
            player.Move(move);
            
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