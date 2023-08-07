using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Samples.Playground2D.Scripts
{
    public class FlipBody2D : MonoBehaviour
    {
        public bool lookingAtRight = true;
        
        [Space]
        public PlayerController2D player;
        public Animator animator;
        
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Falling = Animator.StringToHash("Falling");

        public void Flip()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        
            lookingAtRight = !lookingAtRight;
        }
    
        private void Update()
        {
            if (player && player.moving)
            {
                if (player.moveInput > 0 && !lookingAtRight)
                {
                    Flip();
                }
                else if (player.moveInput < 0 && lookingAtRight)
                {
                    Flip();
                }
            }
        }

        public float fallingTimeout = 0.5f;
        public float fallingTimeoutDelta;
        
        private void LateUpdate()
        {
            if (player && animator)
            {
                animator.SetBool(Moving, player.moving);
                
                if (player.falling)
                {
                    if (0 <= fallingTimeoutDelta)
                    {
                        fallingTimeoutDelta -= Time.deltaTime;
                    }
                    else
                    {
                        animator.SetBool(Falling, player.falling);
                    }
                }
                else
                {
                    fallingTimeoutDelta = fallingTimeout;
                    animator.SetBool(Falling, player.falling);
                }
            }
        }
    }
}
