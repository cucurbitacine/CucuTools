using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public class FirstPersonRigidController : PersonController
    {
        #region SerializeField

        [Space]
        [SerializeField] private PersonInfo _info = new PersonInfo();
        [SerializeField] private PersonSettings _settings = new PersonSettings();
        [SerializeField] private GroundController _ground = null;
        [SerializeField] private Transform _eyes = null;
        
        [Header("First Person Settings")]
        [Min(0f)] public float moveSpeedChangeRate = 24f;
        [Min(0f)] public float rotationChangeRate = 24f;
        [Space]
        [Range(0f, 90f)] public float fovUpper = 89;
        [Range(0f, 90f)] public float fovBottom = 89;
        [Space]
        [Min(0f)] public float jumpTimeout = 0.1f;
        //[Min(0f)] public float fallTimeout = 0.15f;
        [Space]
        [Range(0, 1)]
        public float airMoveControl = 1f;
        public Vector3 velocityExternal = Vector3.zero;
        [Space]
        public bool useAngularVelocity = true;
        
        [Space]

        #endregion

        #region Private Fields
        
        private Vector2 _moveInput = Vector2.zero;
        private Vector2 _rotateInput = Vector2.zero;
        private Vector2 _rotateAngle = Vector2.zero;
        
        private Vector3 _velocityMove = Vector3.zero;
        private Vector3 _velocityJump = Vector3.zero;
        private Vector3 _velocityFall = Vector3.zero;
        private Vector3 _velocityPlatform = Vector3.zero;
        private Vector3 _angularVelocity = Vector3.zero;

        private Quaternion _platformRotation = Quaternion.identity;
        
        private float _jumpTimeoutDelta = 0f;
        //private float _fallTimeoutDelta = 0f;

        private Rigidbody _rigid = null;
        private CapsuleCollider _capsule = null;
        
        #endregion

        #region PlayerController

        public override PersonInfo info => _info;
        public override PersonSettings settings => _settings;
        public override GroundController ground => _ground;
        public override Transform eyes => _eyes != null ? _eyes : (_eyes = rigid.transform);

        public override void Move(Vector2 moveInput)
        {
            _moveInput = moveInput;

            info.moving = _moveInput != Vector2.zero;
        }

        public override void Rotate(Vector2 rotateInput)
        {
            _rotateInput = rotateInput;
            
            info.rotating = _rotateInput != Vector2.zero;
        }

        public override void MoveInDirection(Vector3 direction)
        {
            Move(direction.ToLocalDirection(rigid.transform).OnlyXZ());
        }

        public override void MoveAtPoint(Vector3 point)
        {
            MoveInDirection(point - position);
        }

        public override void LookInDirection(Vector3 direction)
        {
            if (direction.sqrMagnitude == 0) return;

            var angX = Vector3.SignedAngle(rigid.transform.forward, Vector3.ProjectOnPlane(direction, Vector3.up), Vector3.up);
            var angY = Vector3.SignedAngle(eyes.forward, Quaternion.Euler(0, -angX, 0) * direction, eyes.right);

            rigid.MoveRotation(Quaternion.Euler(0, angX, 0) * rigid.rotation);
            
            _rotateAngle.y += angY;
        }

        public override void LookAtPoint(Vector3 point)
        {
            LookInDirection(point - eyes.position);
        }
        
        public override void Jump()
        {
            if (settings.canJump && _jumpTimeoutDelta < 0)
            {
                _velocityJump = Vector3.up * Mathf.Sqrt(-2 * settings.jumpHeight * settings.gravity);

                info.jumping = true;
            }
        }

        public override void Stop()
        {
            Move(Vector3.zero);
            Rotate(Vector2.zero);
        }
        
        #endregion
        
        #region Public Properties
        
        public Rigidbody rigid => GetOrAddRigidbody();
        public CapsuleCollider capsule => GetOrAddCapsule();

        public Vector3 velocity => _velocityMove +
                                   _velocityJump +
                                   _velocityFall +
                                   _velocityPlatform +
                                   velocityExternal;

        #endregion

        #region Private API

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
            capsule.center = Vector3.up * (capsule.height * 0.5f);
            
            ground.pointCheck = position;
            ground.radiusCheck = capsule.radius * Mathf.Max(capsule.transform.lossyScale.x, capsule.transform.lossyScale.z);
        }

        private void UpdateMove(float deltaTime)
        {
            // calculating world move direction
            var inputDir = rigid.transform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y)).normalized;
            var moveDir = settings.canMove ? inputDir : Vector3.zero;
            
            // correcting move direction relative of ground normal
            if (ground.grounded)
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, ground.hit.normal).normalized;
            }
            
            // calculating velocity movement
            var velocityMove = moveDir *  settings.moveSpeed;

            // correcting velocity movement if in air  
            if (!ground.grounded)
            {
                velocityMove = Vector3.Lerp(_velocityMove, velocityMove, airMoveControl);
            }
            
            _velocityMove = Vector3.Lerp(_velocityMove, velocityMove, moveSpeedChangeRate * deltaTime);
        }

        private void UpdateFall(float deltaTime)
        {
            if (ground.grounded)
            {
                if (0 <= _jumpTimeoutDelta)
                {
                    _jumpTimeoutDelta -= deltaTime;
                }
                
                // just have landed
                if (!ground.wasGrounded)
                {
                    //_fallTimeoutDelta = fallTimeout;
                    
                    info.jumping = false;
                    info.falling = false;
                    
                    _velocityJump = Vector3.zero;
                    _velocityFall = Vector3.zero;
                }
            }
            else
            {
                // update jumping state
                if (info.jumping)
                {
                    info.jumping = 0 <= velocity.y;
                }

                // update falling state
                if (!info.falling)
                {
                    info.falling = !info.jumping;
                }
                
                // just have left ground
                if (ground.wasGrounded)
                {
                    _jumpTimeoutDelta = jumpTimeout;

                    // save inertion speed from platform
                    _velocityFall = _velocityPlatform;
                }

                //if (0 <= _fallTimeoutDelta)
                //{
                //    _fallTimeoutDelta -= deltaTime;
                //}
                
                //if (_fallTimeoutDelta < 0)
                {
                    // woow! watch out! it is graavity!
                    _velocityFall += Vector3.up * (settings.gravity * deltaTime);
                }
            }
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
                var origin = position + Vector3.up * (capsule.height - radius - offset);
                var direction = Vector3.up;
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
        
        private void UpdateRotation(float deltaTime)
        {
            if (settings.canRotate)
            {
                // accumulate rotating angle around axis Y (yes, input X around axis Y) 
                _rotateAngle.x += settings.rotateSpeed * _rotateInput.x;

                // accumulate rotating angle around axis X (yes, input Y around axis X)
                _rotateAngle.y -= settings.rotateSpeed * _rotateInput.y;
                
                // bottom and upper clipping view
                _rotateAngle.y = Mathf.Clamp(_rotateAngle.y, -fovUpper, fovBottom);
            }

            // if have eyes - apply rotating around axis X
            if (eyes != null)
            {
                var eyesRot = Quaternion.Euler(Vector3.right * _rotateAngle.y);
                
                eyes.localRotation = Quaternion.Lerp(eyes.localRotation, eyesRot, rotationChangeRate * deltaTime);
            }

            var dx = settings.rotateSpeed * (settings.canRotate ? _rotateInput.x : 0);
            if (useAngularVelocity)
            {
                // calculating angular velocity around axis Y
                var angVel = Vector3.up * (Mathf.Deg2Rad * dx) / deltaTime;
            
                // if ground is platform - add platform's angular velocity
                if (ground.platform)
                {
                    var angVelPlatform = Vector3.Project(ground.hit.rigidbody.angularVelocity, Vector3.up);

                    angVel += angVelPlatform;
                }

                _angularVelocity = Vector3.Lerp(rigid.angularVelocity, angVel, rotationChangeRate * deltaTime);
            }
            else
            {
                if (ground.platform)
                {
                    var angle = ground.hit.rigidbody.angularVelocity.y * Mathf.Rad2Deg * deltaTime;
                    _platformRotation = Quaternion.AngleAxis(angle, Vector3.up) * _platformRotation;
                }
                
                var targetRotation = Quaternion.Euler(0, _rotateAngle.x, 0) * _platformRotation;
                targetRotation = Quaternion.Lerp(rigid.rotation, targetRotation, rotationChangeRate * deltaTime);
                rigid.MoveRotation(targetRotation);
            }
        }

        private void UpdateRigidbody(float deltaTime)
        {
            // sum all velocities
            rigid.velocity = velocity;

            // set angular velocity
            if (useAngularVelocity) rigid.angularVelocity = _angularVelocity;
            
            // dont touch. it is working. really. it is saving from sharp and small losing of ground
            rigid.useGravity = ground.grounded && _moveInput != Vector2.zero;
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
            _rotateAngle.x = transform.localRotation.eulerAngles.x;
            _rotateAngle.y = eyes.localRotation.eulerAngles.x;
        }
        
        private void Update()
        {
            UpdateBody();

            UpdateMove(Time.deltaTime);
            UpdateFall(Time.deltaTime);
            UpdatePlatform(Time.deltaTime);

            CheckHeadHit();
            
            UpdateRigidbody(Time.deltaTime);
        }

        private void LateUpdate()
        {
            UpdateRotation(Time.deltaTime);
        }

        private void OnValidate()
        {
            SetupRigid();
            
            UpdateBody();
        }

        #endregion
    }
}