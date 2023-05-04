using System.Collections;
using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class PlayerPlatformController : Player2DController
    {
        public enum BodyType
        {
            Box,
            Capsule,
        }

        #region SerializeField

        public bool grounded = false;
        public bool isOnSlope = false;
        public bool jumping = false;
        public bool freeze = false;
        [HideInInspector]
        public bool wasGround = false;

        [Header("Settings")]
        [Min(0)] public float speedMax = 10f;
        [Min(0)] public float jumpHeight = 2;
        [Min(0)] public int jumpsInAir = 1;
        [Min(0)] public float gravityScale = 5f;
        
        [Header("Ground")]
        public LayerMask groundLayer = 1;
        [Min(0)]
        public float groundCheckDistance = 0.2f;
        [Range(0, 90)] [Tooltip("Degrees")]
        public float maxAngleSlope = 50f;
        [Range(0, 1)] [Tooltip("Seconds")]
        public float durationIgnorePlatform = 0.1f;
        
        [Header("Body")]
        public BodyType bodyType = BodyType.Box;
        [Min(0)] public float playerWidth = 1f;
        [Min(0)] public float playerHeight = 2f;

        #endregion

        #region Private Fields

        private Collider2D _collider = null;
        
        private float _direction = 0f;
        private int _jumpsInAir = 0;

        #endregion

        #region Private Properties
        
        private Vector2 boxCastPoint => playerPoint + playerNormal * boxCastDistance * 0.5f;
        private Vector2 boxCastSize => new Vector2(collider2d.bounds.size.x, boxCastDistance);
        private float boxCastAngle => rigid.rotation + 180f;
        private Vector2 boxCastDirection => -playerNormal;
        private float boxCastDistance => groundCheckDistance;
        
        private Vector2 circleCastPoint => playerPoint + playerNormal * circleCastRadius;
        private Vector2 circleCastDirection => -playerNormal;
        private float circleCastRadius => collider2d.bounds.size.x * 0.5f - 0.001f;
        private float circleCastDistance => groundCheckDistance;

        #endregion

        #region Public Properties

        public RaycastHit2D ground { get; private set; }
        
        public Collider2D collider2d => GetOrAddCollider();
        public Vector2 moveVelocity { get; private set; }

        public Vector2 playerPoint => transform.position;
        public Vector2 playerNormal => transform.up;
        public Vector2 playerRight => -Vector2.Perpendicular(playerNormal);

        public Vector2 groundRight => -Vector2.Perpendicular(ground.normal);
        
        public float slopeAngle => Vector2.Angle(-gravityDirection, ground.normal);
        
        public Vector2 gravity => Physics2D.gravity * gravityScale;
        public Vector2 gravityDirection => gravity.normalized;
        public float gravityPower => gravity.magnitude;

        #endregion

        #region Public API

        public void Move(float direction)
        {
            _direction = direction;
        }

        public void Stop()
        {
            Move(0f);
        }

        public void Jump()
        {
            var canJump = grounded || _jumpsInAir < jumpsInAir;
            
            if (canJump)
            {
                jumping = true;
                
                if (!grounded) _jumpsInAir++;
                
                var jumpDirection = playerNormal;
                var jumpSpeed = Mathf.Sqrt(2 * gravityPower * jumpHeight);
                var jumpVelocity = jumpSpeed * jumpDirection;
                var jumpImpulse = jumpVelocity * rigid.mass;
                
                rigid.velocity = Vector2.zero;
                rigid.AddForce(jumpImpulse, ForceMode2D.Impulse);
            }
        }

        public void Down()
        {
            if (grounded && !jumping)
            {
                if (ground.collider.usedByEffector)
                {
                    Ignore(ground.collider, durationIgnorePlatform);
                }
            }
        }
        
        #endregion

        private void Jumped()
        {
            Debug.Log("Jumped");
        }

        private void Landed()
        {
            _jumpsInAir = 0;
            jumping = false;
            
            Debug.Log("Landed");
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
        
        private RaycastHit2D CircleCast()
        {
            return Physics2D.CircleCast(circleCastPoint, circleCastRadius, circleCastDirection, circleCastDistance, groundLayer);
        }
        
        private RaycastHit2D BoxCast()
        {
            return Physics2D.BoxCast(boxCastPoint, boxCastSize, boxCastAngle, boxCastDirection, boxCastDistance, groundLayer);
        }

        private void UpdateGround()
        {
            wasGround = grounded;
            grounded = ground = bodyType == BodyType.Box ? BoxCast() : CircleCast();
            
            if (grounded)
            {
                grounded = slopeAngle < maxAngleSlope;
            }
            
            if (grounded && !wasGround)
            {
                Landed();
            }
            else if (!grounded && wasGround)
            {
                Jumped();
            }
        }

        private void UpdateSlope()
        {
            isOnSlope = grounded && slopeAngle > 0.001;
        }

        private void UpdateVelocity()
        {
            var moveRight = playerRight;

            if (grounded)
            {
                moveRight = groundRight;
            }

            moveVelocity = moveRight * (_direction * speedMax);
        }

        private void UpdateRigidbody()
        {
            rigid.gravityScale = gravityScale;

            rigid.bodyType = freeze ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;

            if (freeze) return;
            
            if (grounded)
            {
                if (isOnSlope)
                {
                    if (jumping)
                    {
                        rigid.velocity = new Vector2(moveVelocity.x, rigid.velocity.y);
                    }
                    else
                    {
                        rigid.gravityScale = moveVelocity == Vector2.zero ? 0f : gravityScale;
                        rigid.velocity = moveVelocity;
                    }
                }
                else
                {
                    rigid.velocity = new Vector2(moveVelocity.x, rigid.velocity.y);
                }
            }
            else
            {
                rigid.velocity = new Vector2(moveVelocity.x, rigid.velocity.y);
            }
        }

        private void UpdateCollider()
        {
            playerHeight = Mathf.Max(playerWidth, playerHeight);
            
            if (bodyType == BodyType.Box && _collider is BoxCollider2D box)
            {
                box.size = new Vector2(playerWidth, playerHeight);
                box.offset = new Vector2(0f, playerHeight * 0.5f);
            }
                
            if (bodyType == BodyType.Capsule && _collider is CapsuleCollider2D capsule)
            {
                capsule.size = new Vector2(playerWidth, playerHeight);
                capsule.offset = new Vector2(0f, playerHeight * 0.5f);
            }
        }
        
        private Collider2D GetOrAddCollider()
        {
            if (_collider != null)
            {
                if (bodyType == BodyType.Box)
                {
                    if (_collider is BoxCollider2D) return _collider;
                }
                
                if (bodyType == BodyType.Capsule)
                {
                    if (_collider is CapsuleCollider2D) return _collider;
                }

                if (Application.isPlaying)
                {
                    Destroy(_collider);
                }
                else
                {
                    DestroyImmediate(_collider);
                }
            }
            
            if (bodyType == BodyType.Box)
            {
                _collider = GetComponent<BoxCollider2D>();
            }
                
            if (bodyType == BodyType.Capsule)
            {
                _collider = GetComponent<CapsuleCollider2D>();
            }
            
            if (_collider != null)
            {
                return _collider;
            }
            
            if (bodyType == BodyType.Box)
            {
                _collider = gameObject.AddComponent<BoxCollider2D>();
            }
                
            if (bodyType == BodyType.Capsule)
            {
                _collider = gameObject.AddComponent<CapsuleCollider2D>();
            }
            
            UpdateCollider();
            
            return _collider;
        }

        #region MonoBehaviour

        private void Update()
        {
            UpdateCollider();
            
            UpdateGround();
            
            UpdateSlope();
            
            UpdateVelocity();
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();
        }

        #endregion

        #region Editor Only

        private void OnValidate()
        {
            UpdateCollider();
        }

        private void OnDrawGizmos()
        {
            if (bodyType == BodyType.Box)
            {
                Gizmos.color = Color.gray;
                CucuGizmos.DrawWireCube(boxCastPoint + boxCastDirection * boxCastDistance * 0.5f, Vector2.Scale(boxCastSize, new Vector2(1, 2)), Quaternion.Euler(0, 0, boxCastAngle));
            }
            else
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(circleCastPoint, circleCastRadius);
                Gizmos.DrawWireSphere(circleCastPoint + circleCastDirection * circleCastDistance, circleCastRadius);
            }
            
            if (grounded)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(ground.point, groundCheckDistance * 0.5f);
                Gizmos.DrawLine(ground.point, ground.point + ground.normal);
            }
            
            if (grounded)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(ground.point, moveVelocity.normalized);
            }
        }

        #endregion
    }
}