using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class GravityController2D : CucuBehaviour
    {
        public bool autoChangeGravity = false;
        public float gravityChangeSpeed = 10;
        [Range(-180, 180)]
        public float gravityAngle = 0;
        
        [Space]
        public float normalChangeRate = 4f;
        
        [Space]
        public PlayerController2D player2d;
        
        [DrawButton()]
        public void ChangeGravity()
        {
            Physics2D.gravity = Quaternion.Euler(0, 0, gravityAngle) * Vector2.down * Physics2D.gravity.magnitude;
            player2d.SetNormal(-Physics2D.gravity.normalized);
        }
        
        private void FixedUpdate()
        {
            if (autoChangeGravity)
            {
                gravityAngle = Mathf.Repeat(gravityAngle + gravityChangeSpeed * Time.deltaTime + 180f, 360) - 180f;
                
                Physics2D.gravity = Quaternion.Euler(0, 0, gravityAngle) * Vector2.down * Physics2D.gravity.magnitude;

                if (player2d)
                {
                    var playerNormal = Vector2.Lerp(player2d.playerNormal, -Physics2D.gravity.normalized, normalChangeRate * Time.fixedDeltaTime);
                    player2d.SetNormal(playerNormal);
                }
            }
        }
    }
}