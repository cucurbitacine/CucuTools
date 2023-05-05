using System.Collections;
using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class PlayerController2D : CucuBehaviour
    {
        [Header("Info")]
        public bool isMoving = false;
        public bool grounded = false;
        public bool jumping = false;
        public bool freeze = false;
        [Range(-1, 1)]
        public float move = 0f;
        public Vector2 velocity = default;

        [Header("Settings")]
        public float speedMax = 8;
        public float accelerationMax = 64;
        public float jumpHeight = 2f;
        public int jumpsInAir = 1;
        public LayerMask groundLayer = default;
        public float groundDistanceCheck = 0.2f;
        [Range(0, 90)]
        public float angleMaxSlope = 50f;
        
        [Header("Debug")]
        public bool skipInertion = false;
        [Min(0)] public float idleFriction = 16;
        [Min(0)] public float moveFriction = 0;
        
        private bool _wasGrounded = false;
        private int _jumpsInAir = 0;
        
        private PhysicsMaterial2D _idleMat = default;
        private PhysicsMaterial2D _moveMat = default;

        private Vector2 castPoint => playerPoint - castDirection * castRadius;
        private Vector2 castDirection => gravityDirection;
        private float castRadius => collider2d ? collider2d.bounds.size.x * 0.5f - 0.001f : 0.5f;
        private float castDistance => groundDistanceCheck;

        public Vector2 gravity => Physics2D.gravity * (rigidbody2d ? rigidbody2d.gravityScale : 1f);
        public Vector2 gravityDirection => gravity.normalized;
        public float gravityPower => gravity.magnitude;
        
        public Vector2 playerPoint => transform.position;
        public Vector2 playerNormal => transform.up;
        public Vector2 playerRight => -Vector2.Perpendicular(playerNormal);
        
        public RaycastHit2D groundHit2d { get; private set; }
        public Rigidbody2D rigidbody2d { get; private set; }
        public Collider2D collider2d { get; private set; }
        
        public void Move(float move)
        {
            this.move = move;
            
            isMoving = Mathf.Abs(this.move) > 0.001f;
        }

        public void Stop()
        {
            Move(0);
        }
        
        public void Jump()
        {
            var canJump = !freeze && grounded || _jumpsInAir < jumpsInAir;

            if (canJump)
            {
                jumping = true;
                if (!grounded) _jumpsInAir++;
                
                var jumpDirection = playerNormal;
                var jumpSpeed = Mathf.Sqrt(2 * gravityPower * jumpHeight);
                var jumpVelocity = jumpSpeed * jumpDirection;
                var jumpImpulse = jumpVelocity * rigidbody2d.mass;

                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0f);
                rigidbody2d.AddForce(jumpImpulse, ForceMode2D.Impulse);
            }
        }

        public void Down()
        {
            if (grounded && groundHit2d.collider.usedByEffector)
            {
                Ignore(groundHit2d.collider, 0.1f);
            }
        }
        
        private Vector2 GetAcceleration(float deltaTime)
        {
            var prevVelocity = (Vector2)Vector3.Project(rigidbody2d.velocity, velocity);

            var acceleration = (velocity - prevVelocity) / deltaTime;
            acceleration = Vector2.ClampMagnitude(acceleration, accelerationMax);

            return acceleration;
        }

        private void UpdateGround()
        {
            _wasGrounded = grounded;
            
            groundHit2d = Physics2D.CircleCast(castPoint, castRadius, castDirection, castDistance, groundLayer);
            
            grounded = groundHit2d;
            
            if (grounded)
            {
                var angle = Vector2.Angle(-gravityDirection, groundHit2d.normal);
                grounded = angle < angleMaxSlope;
            }

            if (grounded && !_wasGrounded)
            {
                Landed();
            }
            else if (!grounded && _wasGrounded)
            {
                Jumped();
            }
        }
        
        private void UpdateVelocity()
        {
            var moveRight = playerRight;
            
            if (grounded && !jumping)
            {
                moveRight = -Vector2.Perpendicular(groundHit2d.normal).normalized;
            }

            if (isMoving)
            {
                velocity = moveRight * (move * speedMax);
            }
            else
            {
                velocity = Vector2.zero;
            }
            
            var point = grounded ? groundHit2d.point : (Vector2)transform.position;
            Debug.DrawLine(point, point + velocity / speedMax, Color.blue);
        }

        private void UpdateRigidbody(float deltaTime)
        {
            rigidbody2d.bodyType = freeze ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
            if (freeze) return;
            
            if (isMoving)
            {
                rigidbody2d.AddForce(rigidbody2d.mass * GetAcceleration(deltaTime));
            }

            _idleMat.friction = idleFriction;
            _moveMat.friction = moveFriction;
                
            if (grounded && !isMoving)
            {
                rigidbody2d.sharedMaterial = _idleMat;
            }
            else
            {
                rigidbody2d.sharedMaterial = _moveMat;
            }
        }
        
        private void Jumped()
        {
        }

        private void Landed()
        {
            jumping = false;
            _jumpsInAir = 0;
        }
        
        private void Ignore(Collider2D target, float duration)
        {
            StartCoroutine(_Ignoring(target, duration));
        }

        private IEnumerator _Ignoring(Collider2D target, float duration)
        {
            Physics2D.IgnoreCollision(collider2d, target, true);

            yield return new WaitForSeconds(duration);

            while (collider2d.bounds.Intersects(target.bounds))
            {
                yield return new WaitForFixedUpdate();
            }
            
            Physics2D.IgnoreCollision(collider2d, target, false);
        }
        
        private void SetupRigidbody()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            if (rigidbody2d == null) rigidbody2d = gameObject.AddComponent<Rigidbody2D>();
        }
        
        private void SetupCollider()
        {
            collider2d = GetComponent<Collider2D>();
            if (collider2d == null)
            {
                var capsule = gameObject.AddComponent<CapsuleCollider2D>();
                capsule.size = new Vector2(1, 2);
                capsule.offset = new Vector2(0, 1);
                collider2d = capsule;
            }
        }
        
        private void SetupMaterial()
        {
            _idleMat = new PhysicsMaterial2D();
            _moveMat = new PhysicsMaterial2D();

            _idleMat.name = "idle";
            _idleMat.friction = idleFriction;
            _idleMat.bounciness = 0f;
            
            _moveMat.name = "moving";
            _moveMat.friction = moveFriction;
            _moveMat.bounciness = 0f;
        }
        
        private void Awake()
        {
            SetupRigidbody();
            
            SetupCollider();
            
            SetupMaterial();
        }

        private void Update()
        {
            UpdateGround();

            UpdateVelocity();
        }

        private void FixedUpdate()
        {
            UpdateRigidbody(Time.fixedDeltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(castPoint, castRadius);
            Gizmos.DrawWireSphere(castPoint + castDirection * castDistance, castRadius);
        }
    }
}