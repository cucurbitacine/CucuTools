using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts
{
    [DisallowMultipleComponent]
    public class Movement : MonoBehaviour
    {
        [Min(0f)] public float speedMax = 5f;
        [Min(0f)] public float jumpHeight = 2f;

        private Rigidbody2D rigid2d;

        private void Move()
        {
            var velocity = new Vector2
            {
                x = Input.GetAxisRaw("Horizontal") * speedMax,
                y = rigid2d.velocity.y
            };

            rigid2d.velocity = velocity;
        }
        
        private void Jump()
        {
            rigid2d.velocity = new Vector2
            {
                x = Input.GetAxisRaw("Horizontal") * speedMax,
                y = 0f,
            };

            var gravity = rigid2d.gravityScale * Physics2D.gravity.magnitude;
            var jumpImpulse = rigid2d.mass * Mathf.Sqrt(2 * gravity * jumpHeight);

            rigid2d.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
        }
        
        private void Awake()
        {
            rigid2d = GetComponent<Rigidbody2D>();

            if (rigid2d.sharedMaterial == null)
            {
                rigid2d.sharedMaterial = new PhysicsMaterial2D()
                {
                    name = $"{name}_{nameof(PhysicsMaterial2D)}",
                    bounciness = 0f,
                    friction = 0f,
                };
            }
        }

        private void Update()
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
    }
}