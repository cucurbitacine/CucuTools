using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public class PlayerController : CucuBehaviour
    {
        #region SerializeField

        [Header("Person")]
        [SerializeField] private PlayerInfo _info = new PlayerInfo();
        [SerializeField] private PlayerSettings _settings = new PlayerSettings();
        [SerializeField] private GroundController _ground = null;
        [SerializeField] private Transform _head = null;

        [Header("Body")]
        [Min(0f)] public float heightBody = 2f;
        [Min(0f)] public float radiusBody = 0.5f;
        [Min(0f)] public float heightHead = 1.8f;
        
        [Header("Change Rates")]
        [Min(0f)] public float speedChangeRate = 24f;
        [Min(0f)] public float rotationChangeRate = 24f;
        [Min(0f)] public float inertionSpeedFade = 0.05f;
        
        [Header("FOV")]
        [Range(0f, 90f)] public float fovUpper = 89;
        [Range(0f, 90f)] public float fovBottom = 89;
        
        [Header("Other")]
        [Min(0f)] public float jumpTimeout = 0.05f;

        [Space]
        [Range(0, 1)]
        public float airMoveControl = 1f;
        public Vector3 velocityAdditional = Vector3.zero;

        [Space]
        public bool useAngularVelocity = true;
        
        #endregion

        #region Private Fields

        private Vector3 _moveInput = Vector3.zero;
        private Vector2 _viewInput = Vector2.zero;
        private Vector2 _viewAngle = Vector2.zero;
        
        private Vector3 _velocityMove = Vector3.zero;
        private Vector3 _velocityJump = Vector3.zero;
        private Vector3 _velocityFall = Vector3.zero;
        private Vector3 _velocityPlatform = Vector3.zero;
        private Vector3 _velocityInertion = Vector3.zero;
        
        private float _jumpTimeoutDelta = 0f;
        private int _jumpDoneAmount = 0;
        
        private Vector3 _angularVelocity = Vector3.zero;
        private Quaternion _personRotation = Quaternion.identity;
        
        private Rigidbody _rigid = null;
        private CapsuleCollider _capsule = null;

        #endregion

        #region Public Properties

        public PlayerInfo info => _info;
        public PlayerSettings settings => _settings;
        public GroundController ground => GetOrAddGroundController();
        public Transform head => _head != null ? _head : (_head = rigid.transform);
        
        public Vector3 position => transform.position;
        public Quaternion rotation => transform.rotation;
        
        public Vector3 velocity => velocitySelf + velocityExternal;
        public Vector3 normal => rigid.transform.up;
        
        public Rigidbody rigid => GetOrAddRigidbody();
        public CapsuleCollider capsule => GetOrAddCapsule();

        #endregion

        #region Public API

        public Vector3 velocitySelf => _velocityMove +
                                       _velocityJump +
                                       _velocityFall;

        public Vector3 velocityExternal => _velocityPlatform +
                                           _velocityInertion +
                                           velocityAdditional;
        
        public void Move(Vector3 move)
        {
            _moveInput = move;

            info.moving = _moveInput != Vector3.zero;
        }

        public void MoveInDirection(Vector3 direction)
        {
            Move(direction.ToLocalDirection(rigid.transform).normalized);
        }

        public void MoveAtPoint(Vector3 point)
        {
            MoveInDirection(point - position);
        }
        
        public void View(Vector2 view)
        {
            _viewInput = view;
        }

        public void LookInDirection(Vector3 direction)
        {
            if (direction.sqrMagnitude == 0) return;

            var angX = Vector3.SignedAngle(rigid.transform.forward, Vector3.ProjectOnPlane(direction, normal), normal);
            var angY = Vector3.SignedAngle(head.forward, Quaternion.Euler(0, -angX, 0) * direction, head.right);

            _viewAngle.x += angX;
            _viewAngle.y += angY;
            
            if (useAngularVelocity)
            {
                rigid.MoveRotation(Quaternion.AngleAxis(angX, normal) * rigid.rotation);
            }
            else
            {
                _personRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, normal), normal);
            }
            
            if (head != null)
            {
                head.localRotation = Quaternion.Euler(Vector3.right * _viewAngle.y);
            }
        }

        public void LookAtPoint(Vector3 point)
        {
            LookInDirection(point - head.position);
        }
        
        public void Jump()
        {
            if (settings.canJump && _jumpTimeoutDelta < 0 && _jumpDoneAmount < settings.jumpMaxAmount)
            {
                _jumpDoneAmount++;
                _jumpTimeoutDelta = jumpTimeout;
                
                _velocityJump = normal * Mathf.Sqrt(2 * settings.jumpHeight * settings.gravity.magnitude);
                _velocityFall = Vector3.zero;
                
                info.jumping = true;
            }
        }

        public void Stop()
        {
            Move(Vector3.zero);
            View(Vector2.zero);
        }
        
        #endregion

        #region Private API
        
        private GroundController GetOrAddGroundController()
        {
            if (_ground != null) return _ground;
            _ground = GetComponent<GroundController>();
            if (_ground != null) return _ground;
            _ground = gameObject.AddComponent<GroundController>();
            return _ground;
        }
        
        private Rigidbody GetOrAddRigidbody()
        {
            if (_rigid != null) return _rigid;
            _rigid = GetComponent<Rigidbody>();
            if (_rigid != null) return _rigid;
            _rigid = gameObject.AddComponent<Rigidbody>();
            return _rigid;
        }
        
        private CapsuleCollider GetOrAddCapsule()
        {
            if (_capsule != null) return _capsule;
            _capsule = GetComponent<CapsuleCollider>();
            if (_capsule != null) return _capsule;
            _capsule = gameObject.AddComponent<CapsuleCollider>();
            return _capsule;
        }
        
        private void SetupRigid()
        {
            rigid.isKinematic = false;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rigid.interpolation = RigidbodyInterpolation.Interpolate;
            rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rigid.maxAngularVelocity = float.MaxValue;
        }

        private void UpdateBody()
        {
            capsule.height = heightBody;
            capsule.radius = radiusBody;
            capsule.center = Vector3.up * (capsule.height * 0.5f);
            
            ground.pointCheck = position;
            ground.normalCheck = normal;
            ground.radiusCheck = capsule.radius * Mathf.Max(capsule.transform.lossyScale.x, capsule.transform.lossyScale.z);

            if (head != transform) head.localPosition = Vector3.up * heightHead;
        }

        private void UpdateMove(float deltaTime)
        {
            // calculating world move direction
            var inputDir = rigid.transform.TransformDirection(_moveInput).normalized;
            var moveDir = settings.canMove ? inputDir : Vector3.zero;
            
            // correcting move direction relative of ground normal
            if (ground.grounded)
            {
                moveDir = (Vector3.Project(moveDir, normal) + Vector3.ProjectOnPlane(moveDir, ground.hit.normal)).normalized;
            }
            
            // calculating velocity movement
            var velocityMove = moveDir *  settings.moveSpeed;

            // correcting velocity movement if in air  
            if (!ground.grounded)
            {
                velocityMove = Vector3.Lerp(_velocityMove, velocityMove, airMoveControl);
            }
            
            _velocityMove = Vector3.Lerp(_velocityMove, velocityMove, speedChangeRate * deltaTime);
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
                    
                    _velocityJump = Vector3.zero;
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
                    // start time out
                    //_jumpTimeoutDelta = jumpTimeout;

                    // save inertion speed from platform
                    _velocityInertion = _velocityPlatform;
                }

                // woow! watch out! it is graavity!
                _velocityFall += settings.gravity * deltaTime;
            }
        }

        private void UpdateInertion(float deltaTime)
        {
            _velocityInertion = Vector3.Lerp(_velocityInertion, Vector3.zero, inertionSpeedFade * deltaTime);
        }

        private void UpdatePlatform(float deltaTime)
        {
            _velocityPlatform = Vector3.zero;
            
            if (ground.platform)
            {
                // calculate velocity platform relative of player's position  
                _velocityPlatform = ground.hit.rigidbody.GetPointVelocity(position);
            }
        }

        private void CheckHeadHit()
        {
            // check head hit with objects when are moving up (f*ck gravity) 
            if ((_velocityJump + _velocityFall).y > 0) 
            {
                var offset = 0.01f;
                var radius = capsule.radius;
                var origin = position + normal * (capsule.height - radius - offset);
                var direction = normal;
                var distance = 2 * offset;

                if (Physics.SphereCast(origin, radius, direction, out var hit, distance, ground.layerGround))
                {
                    var isStaticGameObject = hit.transform.gameObject.isStatic;
                    var isKinematicRigidbody = hit.rigidbody != null && hit.rigidbody.isKinematic;
                    if (isStaticGameObject || isKinematicRigidbody)
                    {
                        _velocityJump = Vector3.zero;
                    }
                }
            }
        }

        private void UpdateRigidbody(float deltaTime)
        {
            // sum all velocities
            rigid.velocity = velocity;
 
            // update angular velocity
            if (useAngularVelocity) rigid.angularVelocity = _angularVelocity;
            
            // dont touch. it is working. really. it is saving from sharp and small losing of ground
            rigid.useGravity = ground.grounded && _moveInput != Vector3.zero;
        }

        private void UpdateRotation(float deltaTime)
        {
            if (settings.canRotate)
            {
                // accumulate rotating angle around axis Y (yes, input X around axis Y) 
                _viewAngle.x += settings.rotateSpeed * _viewInput.x;

                // accumulate rotating angle around axis X (yes, input Y around axis X)
                _viewAngle.y -= settings.rotateSpeed * _viewInput.y;
                
                // bottom and upper clipping view
                _viewAngle.y = Mathf.Clamp(_viewAngle.y, -fovUpper, fovBottom);
            }

            UpdateHeadRotation(deltaTime);

            UpdateBodyRotation(deltaTime);
        }

        private void UpdateHeadRotation(float deltaTime)
        {
            // if have eyes - apply rotating around axis X
            if (head != null)
            {
                var eyesRot = Quaternion.Euler(Vector3.right * _viewAngle.y);
                
                head.localRotation = Quaternion.Lerp(head.localRotation, eyesRot, rotationChangeRate * deltaTime);
            }
        }

        private void UpdateBodyRotation(float deltaTime)
        {
            var dx = settings.rotateSpeed * (settings.canRotate ? _viewInput.x : 0);
            
            if (useAngularVelocity)
            {
                var angVel = Vector3.zero;
                
                // calculating angular velocity around player normal
                angVel += normal * (Mathf.Deg2Rad * dx) / deltaTime;

                // if ground is platform - add platform's angular velocity
                if (ground.platform)
                {
                    var angVelPlatform = Vector3.Project(ground.hit.rigidbody.angularVelocity, normal);

                    angVel += angVelPlatform;
                }

                _angularVelocity = Vector3.Lerp(rigid.angularVelocity, angVel, rotationChangeRate * deltaTime);
            }
            else
            {
                _personRotation = Quaternion.AngleAxis(dx, normal) * _personRotation;
                
                
                if (ground.platform)
                {
                    var angVel = ground.hit.rigidbody.angularVelocity.y * Mathf.Rad2Deg;
                    _personRotation = Quaternion.AngleAxis(angVel * deltaTime, normal) * _personRotation;
                }

                var changeRate = rotationChangeRate * (ground.grounded ? 1f : airMoveControl);
                rigid.MoveRotation(Quaternion.Lerp(rigid.rotation, _personRotation, changeRate * deltaTime));
            }
        }
        
        private void CheckObstaclesInAir(Collision collision)
        {
            if (ground.grounded) return;

            if (!Cucu.ContainsLayer(ground.layerGround, collision.gameObject.layer)) return;
            
            if (collision.gameObject.isStatic)
            {
                _velocityInertion = Vector3.zero;
            }
            else if (collision.rigidbody != null && collision.rigidbody.isKinematic)
            {
                _velocityInertion = Vector3.zero;
            }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            SetupRigid();
            
            UpdateBody();
        }

        private void OnEnable()
        {
            _viewAngle.x = transform.localRotation.eulerAngles.x;
            _viewAngle.y = head.localRotation.eulerAngles.x;
            
            _personRotation = rigid.rotation;
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

        private void LateUpdate()
        {
            UpdateRotation(Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckObstaclesInAir(collision);
        }

        private void OnValidate()
        {
            SetupRigid();
            
            UpdateBody();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(head.position, 0.05f);
        }

        #endregion
    }
}