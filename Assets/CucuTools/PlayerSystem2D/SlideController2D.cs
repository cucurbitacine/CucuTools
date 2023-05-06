using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class SlideController2D : CucuBehaviour
    {
        public Collider2D lastWall = default;
        
        [Space]
        [Range(0, 1)]
        public float slideWeight = 1f;
        
        [Space]
        public LayerMask wallLayer = default;
        [Min(0)]
        public float castRadius = 0.25f;
        
        
        [Space]
        public PlayerController2D player2d = default;

        private Vector2 castDirection => player2d.move * player2d.playerRight;
        private Vector2 castPoint => player2d.playerPoint +
                                     player2d.playerNormal * (player2d.playerHeight * 0.5f) +
                                     castDirection * (player2d.playerWidth * 0.5f);

        private Collider2D GetWall()
        {
            if (player2d.grounded) return null;

            if (!player2d.moving) return null;

            return Physics2D.OverlapCircle(castPoint, castRadius, wallLayer);
        }

        private void Slide()
        {
            var projectVelocity = (Vector2)Vector3.Project(player2d.rigidbody2d.velocity, player2d.gravityDirection);
            var slideVelocity = -projectVelocity;

            if (lastWall.attachedRigidbody)
            {
                slideVelocity += lastWall.attachedRigidbody.velocity;
            }
                
            var slideImpulse = slideVelocity * player2d.rigidbody2d.mass;
                
            player2d.rigidbody2d.AddForce(slideWeight * slideImpulse, ForceMode2D.Impulse);
        }
        
        private void FixedUpdate()
        {
            if (player2d.falling)
            {
                lastWall = GetWall();
                
                if (lastWall)
                {
                    Slide();
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (player2d)
            {
                var isWall = Physics2D.OverlapCircle(castPoint, castRadius, wallLayer);

                var color = isWall ? Color.green : Color.red;

                if (player2d.grounded || !player2d.moving || !player2d.falling)
                {
                    color = Color.Lerp(color, Color.grey, 0.9f);
                }

                Gizmos.color = color;
                Gizmos.DrawWireSphere(castPoint, castRadius);
            }
        }
    }
}