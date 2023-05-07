using System;
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

        private void LateUpdate()
        {
            if (player)
            {
                animator.SetBool(Moving, player.moving);
                animator.SetBool(Falling, player.falling);
            }
        }
    }
}
