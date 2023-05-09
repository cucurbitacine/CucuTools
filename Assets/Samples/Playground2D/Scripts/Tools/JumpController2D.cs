using UnityEngine;

namespace Samples.Playground2D.Scripts.Tools
{
    public class JumpController2D : ToolController2D
    {
        [Header("Jump")]
        public Collider2D lastWall = default;
        
        [Space]
        [Min(0)] public float jumpDistance = 3;
        [Range(0, 90)] public float jumpAngle = 60;
        
        [Space]
        public LayerMask wallLayer = default;
        [Min(0)] public float castRadius = 0.25f;
        
        private Vector2 castDirection => -player2d.move * player2d.playerRight;
        private Vector2 castPoint => player2d.playerPoint +
                                     player2d.playerNormal * (player2d.playerHeight * 0.5f) +
                                     castDirection * (player2d.playerWidth * 0.5f);
        
        public void Jump()
        {
            lastWall = GetWall();
            
            if (lastWall)
            {
                player2d.ResetAirJump();
                
                WallJump();
            }
            else
            {
                PlayerJump();
            }
        }

        private Quaternion jumpDirectionRotator => Quaternion.Euler(0, 0, Mathf.Sign(player2d.move) * jumpAngle);
        private Vector2 moveDirection => (player2d.move * player2d.playerRight).normalized;
        private Vector2 jumpDirection => (jumpDirectionRotator * moveDirection).normalized;
        
        public void WallJump()
        {
            var jumpSpeed = Mathf.Sqrt(2 * jumpDistance * player2d.gravityPower);
            var jumpVelocity = jumpSpeed * jumpDirection;

            if (lastWall && lastWall.attachedRigidbody)
            {
                jumpVelocity += lastWall.attachedRigidbody.velocity;
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

        private Collider2D GetWall()
        {
            if (player2d.grounded) return null;

            if (!player2d.moving) return null;

            return Physics2D.OverlapCircle(castPoint, castRadius, wallLayer);
        }

        private void OnDrawGizmos()
        {
            if (player2d)
            {
                Gizmos.DrawLine(castPoint, castPoint + jumpDirection);
                
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