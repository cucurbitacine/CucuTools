using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class JumpController2D : CucuBehaviour
    {
        public Collider2D jumpWall = default;
        
        [Space]
        [Min(0)]
        public float castRadius = 0.25f;
        public LayerMask wallLayer = default;
        
        [Space]
        public PlayerController2D player2d = default;

        private Vector2 castDirection => -player2d.move * player2d.playerRight;
        private Vector2 castPoint => player2d.playerPoint +
                                     player2d.playerNormal * (player2d.playerHeight * 0.5f) +
                                     castDirection * (player2d.playerWidth * 0.5f);
        
        public void Jump()
        {
            if (IsWall())
            {
                WallJump();
                
                player2d.ResetAirJump();
            }
            else
            {
                PlayerJump();
            }
        }

        public void WallJump()
        {
            // TODO change angle of jumping. now it is 45 degree 
            
            var moveDirection = (player2d.move * player2d.playerRight).normalized;
            var jumpDirection = (moveDirection + player2d.playerNormal).normalized;
            var jumpSpeed = 2 * Mathf.Sqrt(player2d.jumpHeight * player2d.gravityPower);
            var jumpVelocity = jumpSpeed * jumpDirection;

            if (jumpWall && jumpWall.attachedRigidbody)
            {
                jumpVelocity += jumpWall.attachedRigidbody.velocity;
            }
            
            var jumpImpulse = jumpVelocity * player2d.rigidbody2d.mass;

            var fallingVelocity = (Vector2)Vector3.Project(player2d.rigidbody2d.velocity, player2d.gravityDirection);
            player2d.rigidbody2d.velocity -= fallingVelocity;
            
            player2d.rigidbody2d.AddForce(jumpImpulse, ForceMode2D.Impulse);
        }

        public void PlayerJump()
        {
            player2d.Jump();
        }

        private bool IsWall()
        {
            if (player2d.grounded) return false;

            if (!player2d.moving) return false;

            return jumpWall = Physics2D.OverlapCircle(castPoint, castRadius, wallLayer);
        }

        private void OnDrawGizmos()
        {
            if (player2d)
            {
                var isWall = Physics2D.OverlapCircle(castPoint, castRadius, wallLayer);

                var color = isWall ? Color.green : Color.red;

                if (player2d.grounded || !player2d.moving)
                {
                    color = Color.Lerp(color, Color.grey, 0.9f);
                }

                Gizmos.color = color;
                Gizmos.DrawWireSphere(castPoint, castRadius);
            }
        }
    }
}