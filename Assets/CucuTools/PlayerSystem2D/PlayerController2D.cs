using System.Collections;
using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class PlayerController2D : CucuBehaviour
    {
        [Header("Info")]
        public bool grounded = false;
        public bool moving = false;
        public bool jumping = false;
        public bool falling = false;
        public bool freeze = false;
        [Range(-1, 1)]
        public float move = 0f;
        public Vector2 velocity = default;
        
        [Header("Movement")]
        [Min(0)] public float speedMax = 8;
        [Min(0)] public float accelerationMax = 64;

        [Header("Jump")]
        [Min(0)] public float jumpHeight = 2f;
        [Min(0)] public int jumpsInAir = 1;
        [Min(0)] public float jumpTimeout = 0.2f;
        
        [Header("Ground")]
        public LayerMask groundLayer = default;
        [Min(0)] public float groundDistanceCheck = 0.2f;
        [Range(0, 90)] public float angleMaxSlope = 50f;
        public bool autoUpdateNormal = false;
        
        [Header("Friction")]
        [Min(0)] public float idleFriction = 16;
        [Min(0)] public float moveFriction = 0;
        
        [Header("Body")]
        [Min(0)] public float playerWidth = 1f;
        [Min(0)] public float playerHeight = 2f;

        private bool _wasGrounded = false;
        private int _jumpsInAir = 0;
        private float _jumpTimeoutDelta = 0f;
        
        private PhysicsMaterial2D _idleMat = default;
        private PhysicsMaterial2D _moveMat = default;

        private Vector2 castPoint => playerPoint - castDirection * castRadius;
        private Vector2 castDirection => -playerNormal;
        private float castRadius => (capsule2d ? capsule2d.size.x * 0.5f : 0.5f) - 0.001f;
        private float castDistance => groundDistanceCheck;

        public Vector2 gravity => Physics2D.gravity * (rigidbody2d ? rigidbody2d.gravityScale : 1f);
        public Vector2 gravityDirection => Physics2D.gravity.normalized;
        public float gravityPower => gravity.magnitude;
        
        public Vector2 playerPoint => transform.position;
        public Vector2 playerNormal => transform.up;
        public Vector2 playerRight => -Vector2.Perpendicular(playerNormal);
        
        public RaycastHit2D groundHit2d { get; private set; }
        public Rigidbody2D rigidbody2d { get; private set; }
        public CapsuleCollider2D capsule2d { get; private set; }
        
        public void Move(float move)
        {
            this.move = move;
            
            moving = Mathf.Abs(this.move) > 0.001f;
        }

        public void Stop()
        {
            Move(0);
        }
        
        public void Jump()
        {
            var canJump = !freeze && (0 <= _jumpTimeoutDelta || _jumpsInAir < jumpsInAir);

            if (canJump)
            {
                jumping = true;
                if (_jumpTimeoutDelta < 0) _jumpsInAir++;
                
                var jumpDirection = playerNormal;
                var jumpSpeed = Mathf.Sqrt(2 * gravityPower * jumpHeight);
                var jumpVelocity = jumpSpeed * jumpDirection;
                var jumpImpulse = jumpVelocity * rigidbody2d.mass;

                var fallingVelocity = (Vector2)Vector3.Project(rigidbody2d.velocity, gravityDirection);
                rigidbody2d.velocity -= fallingVelocity;
                
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

        public void ResetAirJump()
        {
            _jumpsInAir = 0;
        }

        private void UpdateCollider()
        {
            playerHeight = Mathf.Max(playerWidth, playerHeight);
            
            capsule2d.size = new Vector2(playerWidth, playerHeight);
            capsule2d.offset = new Vector2(0, playerHeight * 0.5f);
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

        private void UpdateJumping()
        {
            if (grounded)
            {
                _jumpTimeoutDelta = jumpTimeout;
            }
            else
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }

        private void UpdateVelocity()
        {
            var moveRight = playerRight;
            
            if (grounded && !jumping)
            {
                moveRight = -Vector2.Perpendicular(groundHit2d.normal).normalized;
            }

            if (moving)
            {
                velocity = moveRight * (move * speedMax);
            }
            else
            {
                velocity = Vector2.zero;
            }

            falling = !grounded && Vector2.Dot(Vector3.Project(rigidbody2d.velocity, gravityDirection), gravityDirection) > 0;
        }

        private void UpdateRigidbody(float deltaTime)
        {
            rigidbody2d.bodyType = freeze ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
            
            if (freeze) return;

            if (autoUpdateNormal)
            {
                rigidbody2d.rotation = Vector2.SignedAngle(Vector2.down, gravityDirection);
            }
            
            if (moving)
            {
                var prevVelocity = (Vector2)Vector3.Project(rigidbody2d.velocity, velocity);

                var acceleration = (velocity - prevVelocity) / deltaTime;
                acceleration = Vector2.ClampMagnitude(acceleration, accelerationMax);
                
                rigidbody2d.AddForce(rigidbody2d.mass * acceleration);
            }

            _idleMat.friction = idleFriction;
            _moveMat.friction = moveFriction;
                
            if (grounded && !moving)
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
            
            ResetAirJump();
        }
        
        private void Ignore(Collider2D target, float duration)
        {
            StartCoroutine(_Ignoring(target, duration));
        }

        private IEnumerator _Ignoring(Collider2D target, float duration)
        {
            Physics2D.IgnoreCollision(capsule2d, target, true);

            yield return new WaitForSeconds(duration);

            while (capsule2d.bounds.Intersects(target.bounds))
            {
                yield return new WaitForFixedUpdate();
            }
            
            Physics2D.IgnoreCollision(capsule2d, target, false);
        }
        
        private void SetupRigidbody()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            
            if (rigidbody2d == null)
            {
                rigidbody2d = gameObject.AddComponent<Rigidbody2D>();
            }
        }
        
        private void SetupCollider()
        {
            capsule2d = GetComponent<CapsuleCollider2D>();
            
            if (capsule2d == null)
            {
                capsule2d = gameObject.AddComponent<CapsuleCollider2D>();
            }
            
            UpdateCollider();
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
            UpdateCollider();
            
            UpdateGround();

            UpdateJumping();
            
            UpdateVelocity();
        }

        private void FixedUpdate()
        {
            UpdateRigidbody(Time.fixedDeltaTime);
        }

        private void OnValidate()
        {
            if (capsule2d == null) capsule2d = GetComponent<CapsuleCollider2D>();
            if (capsule2d) UpdateCollider();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(castPoint, castRadius);
            Gizmos.DrawWireSphere(castPoint + castDirection * castDistance, castRadius);

            var point = grounded ? groundHit2d.point : (Vector2)transform.position;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(point, point + velocity / speedMax);
        }
    }
}