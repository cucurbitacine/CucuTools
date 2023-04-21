using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class PlayerPlatformController : Player2DController
    {
        #region SerializeField

        [SerializeField] private Player2DInfo _info = new Player2DInfo();
        [SerializeField] private Player2DSettings _settings = new Player2DSettings();
        [SerializeField] private Ground2DController _ground = null;
        
        [Header("Body")]
        [Min(0f)] public float heightBody = 2f;
        [Min(0f)] public float radiusBody = 0.5f;

        [Space]
        [Min(0f)] public float jumpTimeout = 0.05f;
        [Min(0f)]
        public float inertionSpeedFade = 0.05f;
        [Range(0f, 1f)] public float airMoveControl = 0.9f;
        [Min(0f)] public float maxFallSpeed = 32f;
        
        [Header("Snapping")]
        public bool snap = true;
        [Min(0f)]
        public float snapChangeRate = 8f;
        [Min(0f)]
        public float snapMaxDistance = 0.01f;
        
        [Space]
        public Vector2 velocityAdditional = Vector2.zero;
        
        #endregion
        
        private CapsuleCollider2D _capsule = null;
        
        private Vector2 _moveInput = Vector2.zero;
        private Vector2 _velocityMove = Vector2.zero;
        private Vector2 _velocityFall = Vector2.zero;
        private Vector2 _velocityPlatform = Vector2.zero;
        private Vector2 _velocityInertion = Vector2.zero;
        
        private float _jumpTimeoutDelta = 0f;
        private int _jumpDoneAmount = 0;
        
        private readonly RaycastHit2D[] _overlap = new RaycastHit2D[32];
        
        public Player2DInfo info => _info;
        public Player2DSettings settings => _settings;
        public Ground2DController ground => GetOrAddGroundController();
        public CapsuleCollider2D capsule => GetOrAddCapsule();
        public Vector2 normal => rigid.transform.up;
        
        public Vector2 velocitySelf => _velocityMove +
                                       _velocityFall;

        public Vector2 velocityExternal => _velocityPlatform +
                                           _velocityInertion +
                                           velocityAdditional;

        public Vector2 position => transform.position;

        public void Move(Vector2 moveInput)
        {
            _moveInput = moveInput;

            info.moving = _moveInput != Vector2.zero;
        }
        
        public void Jump()
        {
            var firstJump = _jumpDoneAmount == 0;

            var ableToJump = firstJump && ground.grounded || !firstJump && _jumpDoneAmount < settings.jumpMaxAmount;
            
            if (settings.canJump && _jumpTimeoutDelta < 0 && ableToJump)
            {
                _jumpDoneAmount++;
                _jumpTimeoutDelta = jumpTimeout;
                
                _velocityFall = -settings.gravity.normalized * Mathf.Sqrt(2 * settings.jumpHeight * settings.gravity.magnitude);

                info.jumping = true;
            }
        }
        
        public void Stop()
        {
            Move(Vector2.zero);
        }
        
        private Ground2DController GetOrAddGroundController()
        {
            if (_ground != null) return _ground;
            _ground = GetComponent<Ground2DController>();
            if (_ground != null) return _ground;
            _ground = gameObject.AddComponent<Ground2DController>();
            return _ground;
        }
        
        private CapsuleCollider2D GetOrAddCapsule()
        {
            if (_capsule != null) return _capsule;
            _capsule = GetComponent<CapsuleCollider2D>();
            if (_capsule != null) return _capsule;
            _capsule = gameObject.AddComponent<CapsuleCollider2D>();
            return _capsule;
        }

        private void SetupRigid()
        {
            rigid.isKinematic = false;
            rigid.gravityScale = 0f;
            rigid.freezeRotation = true;
            rigid.interpolation = RigidbodyInterpolation2D.Interpolate;
            rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        
        private void UpdateBody()
        {
            capsule.size = new Vector2(2 * radiusBody, heightBody);
            capsule.offset = Vector2.up * (heightBody * 0.5f);
            
            ground.pointCheck = position;
            ground.normalCheck = normal;
            ground.gravity = settings.gravity;
            ground.radiusCheck = radiusBody * Mathf.Max(capsule.transform.lossyScale.x, capsule.transform.lossyScale.z);
        }

        private void UpdateMove(float deltaTime)
        {
            // calculating world move direction
            var inputDir = (Vector2)rigid.transform.TransformDirection(Vector2.right * _moveInput.x);
            var moveDir = settings.canMove ? inputDir : Vector2.zero;
            
            // correcting move direction relative of ground normal
            if (ground.grounded)
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, ground.hit.normal).normalized;
            }
            
            // calculating velocity movement
            var velocityMove = moveDir *  settings.moveSpeed;

            // correcting velocity movement if in air  moveSpeed
            if (!ground.grounded)
            {
                velocityMove = Vector3.Lerp(_velocityMove, velocityMove, airMoveControl);
            }
            
            _velocityMove = velocityMove;
        }

        private void UpdateFall(float deltaTime)
        {
            if (0 <= _jumpTimeoutDelta)
            {
                _jumpTimeoutDelta -= deltaTime;
            }

            if (ground.grounded)
            {
                // just have landed
                if (!ground.wasGrounded)
                {
                    _jumpDoneAmount = 0;
                    
                    info.jumping = false;
                    info.falling = false;
                    
                    _velocityFall = Vector3.zero;
                    _velocityInertion = Vector3.zero;
                }
            }
            else
            {
                // update jumping state
                if (info.jumping)
                {
                    info.jumping = velocitySelf.y >= 0;
                }

                // update falling state
                if (!info.falling)
                {
                    info.falling = velocitySelf.y < 0;
                }
                
                // just have left ground
                if (ground.wasGrounded)
                {
                    // save inertion speed from platform
                    _velocityInertion = _velocityPlatform;
                }

                // woow! watch out! it is graavity!
                _velocityFall += settings.gravity * deltaTime;
            }

            _velocityFall = Vector2.ClampMagnitude(_velocityFall, maxFallSpeed);
        }

        private void UpdateInertion(float deltaTime)
        {
            _velocityInertion = Vector3.Lerp(_velocityInertion, Vector3.zero, inertionSpeedFade * deltaTime);
        }

        private void UpdatePlatform(float deltaTime)
        {
            _velocityPlatform = Vector2.zero;
            
            if (ground.platform)
            {
                // calculate velocity platform relative of player's position  
                _velocityPlatform = ground.hit.rigidbody.GetPointVelocity(position);
            }
        }

        private void CheckHeadHit()
        {
            // check head hit with objects when are moving up (f*ck gravity) 
            if (velocitySelf.y > 0) 
            {
                var offset = 0.01f;
                var radius = radiusBody;
                var origin = position + normal * (heightBody - radius - offset);
                var direction = normal;
                var distance = 2 * offset;

                var count = Physics2D.CircleCastNonAlloc(origin, radius, direction, _overlap, distance, ground.layerGround);

                for (var i = 0; i < count; i++)
                {
                    var hit = _overlap[i];
                    
                    if (hit.collider == capsule) continue;

                    var hitRigid = hit.rigidbody;

                    if (hit.transform.gameObject.isStatic)
                    {
                        _velocityFall = Vector3.zero;
                        break;
                    }

                    if (hit.collider.usedByEffector) continue;
                    
                    if (hitRigid == null) continue;

                    var bodyType = hitRigid.bodyType;
                    
                    var isKinematicRigidbody = bodyType == RigidbodyType2D.Kinematic;
                    var isStaticRigidbody = bodyType == RigidbodyType2D.Static;
                    
                    if (isKinematicRigidbody || isStaticRigidbody)
                    {
                        _velocityFall = Vector3.zero;
                        break;
                    }
                }
            }
        }
        
        private void UpdateRigidbody(float deltaTime)
        {
            rigid.velocity = velocitySelf + velocityExternal;
        }

        private void UpdateRotation(float deltaTime)
        {
            rigid.SetRotation(Quaternion.LookRotation(Vector3.forward, -settings.gravity.normalized));
        }
        
        private void UpdateSnap(float deltaTime)
        {
            if (snap && ground.grounded && !info.inAir && _moveInput == Vector2.zero)
            {
                var center = position + normal * radiusBody;
                
                var point = ground.hit.point;

                var touch = center + (point - center).normalized * radiusBody;

                var snapOffset = point - touch;

                snapOffset = Vector3.Project(snapOffset, normal);
                
                if (snapOffset.magnitude > snapMaxDistance)
                {
                    var snapPoint = rigid.position + snapOffset;

                    rigid.MovePosition(Vector2.Lerp(rigid.position, snapPoint, snapChangeRate * deltaTime));
                }
            }
        }
        
        private void Awake()
        {
            SetupRigid();
        }

        private void Update()
        {
            UpdateBody();

            UpdateMove(Time.deltaTime);
            UpdateFall(Time.deltaTime);
            UpdateInertion(Time.deltaTime);
            UpdatePlatform(Time.deltaTime);

            CheckHeadHit();
            
            UpdateRigidbody(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            UpdateRotation(Time.fixedDeltaTime);
            
            UpdateSnap(Time.fixedDeltaTime);
        }

        private void OnValidate()
        {
            SetupRigid();
            
            UpdateBody();
        }
    }
}