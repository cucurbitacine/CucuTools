using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Samples.Playground2D.Scripts
{
    public class FlipBody2D : MonoBehaviour
    {
        public PlayerController2D player;

        public bool lookingAtRight = true; 
        
        public void Flip()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        
            lookingAtRight = !lookingAtRight;
        }
    
        private void Update()
        {
            if (player && player.isMoving)
            {
                if (player.move > 0 && !lookingAtRight)
                {
                    Flip();
                }
                else if (player.move < 0 && lookingAtRight)
                {
                    Flip();
                }
            }
        }
    }
}
