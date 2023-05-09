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

        [Header("Body")]
        [Min(0)] public float playerWidth = 1;
        [Min(0)] public float playerHeight = 2;
        
        [Header("Movement")]
        [Min(0)] public float speedMax = 5;
        [Min(0)] public float accelerationMax = 100;

        [Header("Jump")]
        [Min(0)] public float jumpHeight = 2f;
        [Min(0)] public int jumpsInAir = 1;
        [Min(0)] public float jumpTimeout = 0.2f;
        
        [Header("Gravity")]
        public Vector2 gravity = Vector2.down * 9.81f;
        public float gravityScale = 5f;
        
        [Header("Ground")]
        public LayerMask groundLayer = default;
        [Min(0)] public float groundDistanceCheck = 0.2f;
        [Range(0, 90)] public float angleMaxSlope = 50;
        [Min(0)] public float groundIgnoreDuration = 0.2f;
        
        [Header("Friction")]
        public PhysicsMaterial2D idleMat = default;
        public PhysicsMaterial2D moveMat = default;

        public const float MoveTolerance = 0.001f;

        public const float IdleFrictionDefault = 1000f;
        public const float IdleBouncinessDefault = 0f;
        
        public const float MoveFrictionDefault = 0f;
        public const float MoveBouncinessDefault = 0f;
        
        private bool _wasGrounded = false;
        private int _jumpsInAir = 0;
        private float _jumpTimeoutDelta = 0f;

        private Vector2 castPoint => playerPoint - castDirection * castRadius;
        private Vector2 castDirection => -playerNormal;
        private float castRadius => (capsule2d ? capsule2d.size.x * 0.5f : 0.5f) - 0.001f;
        private float castDistance => groundDistanceCheck;

        public float move { get; private set; }
        public Vector2 velocity { get; private set; }
        public Vector2 groundVelocity { get; private set; }
        
        public Vector2 playerPoint => transform.position;
        public Vector2 playerNormal => transform.up;
        public Vector2 playerRight => -Vector2.Perpendicular(playerNormal);
        
        public Vector2 playerGravity => gravity * gravityScale;
        
        public Vector2 gravityDirection => gravity.normalized;
        public float gravityPower => playerGravity.magnitude;
        
        public RaycastHit2D groundHit2d { get; private set; }
        public Rigidbody2D rigidbody2d { get; private set; }
        public CapsuleCollider2D capsule2d { get; private set; }

        #region Public API

        public void Move(float move)
        {
            if (Mathf.Abs(move) > MoveTolerance)
            {
                this.move = move;
                
                moving = true;
            }
            else
            {
                this.move = 0f;
                
                moving = false;
            }
        }

        public void Stop()
        {
            Move(0);
        }
        
        public void Jump()
        {
            var canJump = rigidbody2d.bodyType == RigidbodyType2D.Dynamic &&
                0 <= _jumpTimeoutDelta || _jumpsInAir < jumpsInAir;

            if (canJump)
            {
                jumping = true;
                if (_jumpTimeoutDelta < 0) _jumpsInAir++;
                
                var jumpDirection = playerNormal;
                var jumpSpeed = Mathf.Sqrt(2 * gravityPower * jumpHeight);
                var jumpVelocity = jumpSpeed * jumpDirection;

                if (grounded)
                {
                    jumpVelocity += (Vector2)Vector3.Project(groundVelocity, jumpDirection);
                }
                
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
                Ignore(groundHit2d.collider, groundIgnoreDuration);
            }
        }

        public void ResetAirJump()
        {
            _jumpsInAir = 0;
        }

        #endregion

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

            if (grounded)
            {
                if (groundHit2d.collider.attachedRigidbody)
                {
                    groundVelocity = groundHit2d.collider.attachedRigidbody.GetPointVelocity(groundHit2d.point);
                }
                else
                {
                    groundVelocity = Vector2.zero;
                }
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

            if (grounded)
            {
                velocity += groundVelocity;
            }
            else
            {
                velocity += (Vector2)Vector3.Project(groundVelocity, velocity.normalized);
            }

            falling = !grounded && Vector2.Dot(Vector3.Project(rigidbody2d.velocity, gravityDirection), gravityDirection) > 0;
        }

        private void UpdateRigidbody()
        {
            if (grounded && !moving)
            {
                rigidbody2d.sharedMaterial = idleMat;
            }
            else
            {
                rigidbody2d.sharedMaterial = moveMat;
            }
        }

        private void UpdateForcing(float deltaTime)
        {
            if (rigidbody2d.bodyType != RigidbodyType2D.Dynamic) return;
            
            rigidbody2d.AddForce(playerGravity * rigidbody2d.mass);
            
            if (moving)
            {
                var prevVelocity = (Vector2)Vector3.Project(rigidbody2d.velocity, velocity);
                
                var acceleration = (velocity - prevVelocity) / deltaTime;
                acceleration = Vector2.ClampMagnitude(acceleration, accelerationMax);
                
                rigidbody2d.AddForce(rigidbody2d.mass * acceleration);
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
            
            rigidbody2d.gravityScale = 0f;
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
            if (idleMat == null)
            {
                idleMat = new PhysicsMaterial2D();
                idleMat.name = "idle";
                idleMat.friction = IdleFrictionDefault;
                idleMat.bounciness = IdleBouncinessDefault;
            }

            if (moveMat == null)
            {
                moveMat = new PhysicsMaterial2D();
                moveMat.name = "moving";
                moveMat.friction = MoveFrictionDefault;
                moveMat.bounciness = MoveBouncinessDefault;
            }
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
            UpdateRigidbody();
            
            UpdateForcing(Time.fixedDeltaTime);
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

    public static class PlayerController2DExtensions
    {
        public static void SetNormal(this PlayerController2D player2d, Vector2 normal)
        {
            player2d.rigidbody2d.rotation = Vector2.SignedAngle(Vector2.up, normal);
        }
        
        public static void SetNormal(this PlayerController2D player2d, Vector2 normal, float angleTolerance)
        {
            if (Vector2.Angle(player2d.playerNormal, normal) >= angleTolerance)
            {
                player2d.SetNormal(normal);
            }
        }

        public static void NormalByGravity(this PlayerController2D player2d)
        {
            player2d.SetNormal(-player2d.gravityDirection);
        }
        
        public static void NormalByGravity(this PlayerController2D player2d, float angleTolerance)
        {
            player2d.SetNormal(-player2d.gravityDirection, angleTolerance);
        }

        public static void Move(this PlayerController2D player2d, float move, float speed)
        {
            player2d.speedMax = speed;
            player2d.Move(move);
        }
        
        public static bool IsStatic(this PlayerController2D player2d)
        {
            return player2d.rigidbody2d.bodyType == RigidbodyType2D.Static;
        }

        public static void SetStatic(this PlayerController2D player2d, bool value)
        {
            player2d.rigidbody2d.bodyType = value ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        }
        
        public static void ChangeFreeze(this PlayerController2D player2d)
        {
            player2d.SetStatic(!player2d.IsStatic());
        }
    }
}