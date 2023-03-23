using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public class PlayerRigidController : PlayerController
    {
        #region SerializeField

        [Space]
        public PlayerInfo info = new PlayerInfo();
        public PlayerSettings settings = new PlayerSettings();
        public GroundSettings ground = new GroundSettings();

        [Space]
        public Transform _eyes = null;

        #endregion

        #region Private Fields

        private Vector3 _velocityMove = Vector3.zero;
        private Vector3 _velocityJump = Vector3.zero;
        private Vector3 _velocityFall = Vector3.zero;
        private Vector3 _velocityAir = Vector3.zero;
        private Vector3 _velocityPlatform = Vector3.zero;
        private Vector3 _angularVelocity = Vector3.zero;
        
        private Vector2 _viewAngle = Vector2.zero;
        private Vector2 _viewInput = Vector2.zero;
        
        private float _timerAbleToJump = 0f;
        private bool _ableJump = true;

        private Rigidbody _rigid = null;
        private CapsuleCollider _capsule = null;
        
        [Header("Dev")]
        public bool useMoveRotation = false;
        public bool updateAngularVelocity = true;
        public bool overrideAngularVelocity = false;
        private Quaternion _initRotation = Quaternion.identity;
        private Quaternion _bodyRotation = Quaternion.identity;
        
        #endregion

        #region Public Properties

        public Rigidbody rigid => GetOrAddRigidbody();
        public CapsuleCollider capsule => GetOrAddCapsule();

        public Transform eyes => _eyes != null ? _eyes : (_eyes = rigid.transform);
        public Transform body => rigid.transform;
        
        public Vector3 position => body.position;
        public Quaternion rotation => body.rotation;
        public Vector3 up => Vector3.up;
        public Vector3 velocity => _velocityMove + _velocityJump + _velocityFall + _velocityAir + _velocityPlatform;
        
        public Vector3 moveDirection { get; private set; }
        
        #endregion

        #region Public API

        public void Move(Vector3 direction)
        {
            moveDirection = direction.normalized;

            info.isMoving = moveDirection != Vector3.zero;
        }

        public void Rotate(Vector2 viewInput)
        {
            _viewInput = viewInput;
        }

        public void LookIn(Vector3 direction)
        {
            if (direction.sqrMagnitude == 0) return;

            var angX = Vector3.SignedAngle(body.forward, Vector3.ProjectOnPlane(direction, Vector3.up), Vector3.up);
            var angY = Vector3.SignedAngle(eyes.forward, Quaternion.Euler(0, -angX, 0) * direction, eyes.right);

            rigid.MoveRotation(Quaternion.Euler(0, angX, 0) * rigid.rotation);
            
            _viewAngle.y += angY;
        }

        public void LookAt(Vector3 point)
        {
            LookIn(point - eyes.position);
        }
        
        public void Jump()
        {
            if (settings.canJump && _ableJump)
            {
                _velocityJump = up * Mathf.Sqrt(2 * settings.jumpHeight * settings.gravity.magnitude);;
                
                _ableJump = false;
                info.isJumping = true;
            }
        }

        public void Stop()
        {
            Move(Vector3.zero);
        }
        
        public void MoveLocal(Vector3 localDirection)
        {
            Move(body.transform.TransformDirection(localDirection));
        }

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

        private void FixGround()
        {
            if (ground.wasGround && !ground.isGround) // left ground
            {
                if (info.isGrounded)
                {
                    info.isGrounded = false;
                }
                else
                {
                    ground.wasGround = false;
                }
            }

            if (!ground.wasGround && ground.isGround) // found ground
            {
                if (info.isGrounded)
                {
                    ground.wasGround = true;
                }
                else
                {
                    info.isGrounded = true;
                }
            }

            if (!ground.isGround) ground.isPlatform = false;
        }
        
        private void UpdateBody()
        {
            capsule.center = Vector3.up * (capsule.height * 0.5f);
        }

        private void UpdateMove(float deltaTime)
        {
            if (ground.isGround)
            {
                // calculate move direction relative of ground
                var moveDir = settings.canMove ? moveDirection : Vector3.zero;
                moveDir = Vector3.ProjectOnPlane(moveDir, ground.hit.normal).normalized;
                
                // calculate velocity movement
                var velocityMove = moveDir *  settings.speed;

                if (!ground.wasGround)
                {
                    // if just have grounded - add velocity in air  
                    velocityMove = Vector3.ClampMagnitude(velocityMove + _velocityAir, settings.speedMax);
                }
                
                // apply damping. or not
                if (settings.useWalkDamping)
                {
                    _velocityMove = Vector3.Lerp(_velocityMove, velocityMove, settings.dampWalk * deltaTime);
                }
                else
                {
                    _velocityMove = velocityMove;
                }
            }
            else
            {
                _velocityMove = Vector3.zero;
            }
        }

        private void UpdatePlatform(float deltaTime)
        {
            _velocityPlatform = Vector3.zero;
            
            if (ground.isPlatform)
            {
                // calculate velocity platform relative of player's position  
                _velocityPlatform = ground.hit.rigidbody.GetPointVelocity(position);
            }
        }

        private void UpdateFall(float deltaTime)
        {
            if (ground.isGround)
            {
                if (!ground.wasGround)
                {
                    info.isFalling = false;
                    
                    // just have landed. falling is over and able jump again
                    _velocityJump = Vector3.zero;
                    _velocityFall = Vector3.zero;
                    _ableJump = true;
                }
            }
            else
            {
                if (info.isJumping)
                {
                    info.isJumping = 0 <= velocity.y;
                    
                }

                if (!info.isFalling)
                {
                    info.isFalling = !info.isJumping;
                }
                
                if (ground.wasGround)
                {
                    // just have left ground, it is time to start timer when still able jump 
                    _timerAbleToJump = 0f;

                    _velocityFall = _velocityPlatform;
                }
                else
                {
                    // calculating timer when still able jump 
                    _timerAbleToJump += deltaTime;
                    if (_timerAbleToJump >= settings.jumpDelay)
                    {
                        // sorry, too late for jumping. come next time
                        _ableJump = false;
                    }
                }
                
                // woow! watch out! it is graavity!
                _velocityFall += settings.gravity * deltaTime;
                
                // check head hit with objects when are moving up (f*ck gravity) 
                if ((_velocityJump + _velocityFall).y > 0) 
                {
                    var offset = 0.01f;
                    var radius = capsule.radius;
                    var origin = position + up * (capsule.height - radius - offset);
                    var direction = up;
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
        }

        private void UpdateAir(float deltaTime)
        {
            if (ground.isGround)
            {
                _velocityAir = Vector3.zero;
            }
            else
            {
                // calculate direction movement
                var moveDir = settings.canMove ? moveDirection : Vector3.zero;

                // calculate air velocity 
                var velocityAir = moveDir * settings.speed;
                
                // apply damping. or not
                if (settings.useWalkDamping)
                {
                    _velocityAir = Vector3.Lerp(_velocityAir, velocityAir, settings.dampWalk * deltaTime);
                }
                else
                {
                    _velocityAir = velocityAir;
                }
            }
        }

        private void UpdateView(float deltaTime)
        {
            if (settings.canView)
            {
                // accumulate rotating angle around axis Y (yes, input X around axis Y) 
                _viewAngle.x += _viewInput.x;
        
                // accumulate rotating angle around axis X (yes, input Y around axis X)
                _viewAngle.y -= _viewInput.y;
                
                // bottom and upper clipping view
                _viewAngle.y = Mathf.Clamp(_viewAngle.y, -settings.fovUpper, settings.fovBottom);
            }

            // if have eyes - apply rotating around axis X
            if (eyes != null)
            {
                var eyesRot = Quaternion.Euler(Vector3.right * _viewAngle.y);
                
                // apply damping. or not
                if (settings.useViewDamping)
                {
                    eyes.localRotation = Quaternion.Lerp(eyes.localRotation, eyesRot, settings.dampView * deltaTime);
                }
                else
                {
                    eyes.localRotation = eyesRot;
                }
            }

            // calculating angular velocity around axis Y
            var angVel = Vector3.up * (Mathf.Deg2Rad * (settings.canView ? _viewInput.x : 0)) / deltaTime;
            
            // if ground is platform - add platform's angular velocity
            if (ground.isPlatform)
            {
                var angVelPlatform = Vector3.Project(ground.hit.rigidbody.angularVelocity, up);

                angVel += angVelPlatform;
            }

            // apply damping. or not
            if (settings.useViewDamping)
            {
                _angularVelocity = Vector3.Lerp(rigid.angularVelocity, angVel, settings.dampView * deltaTime);
            }
            else
            {
                _angularVelocity = angVel;
            }

            if (useMoveRotation) DevMoveRotation(deltaTime);
        }

        private void DevMoveRotation(float deltaTime)
        {
            if (ground.isPlatform)
            {
                var angVelPlatform = Vector3.Project(ground.hit.rigidbody.angularVelocity, up);
                _initRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angVelPlatform.y * deltaTime, Vector3.up) * _initRotation;
            }

            _bodyRotation = Quaternion.AngleAxis(_viewAngle.x, Vector3.up) * _initRotation;
            
            if (settings.useViewDamping)
            {
                _bodyRotation = Quaternion.Lerp(rigid.rotation, _bodyRotation, settings.dampView * deltaTime);
            }

            if (overrideAngularVelocity)
            {
                var frameRotation = Quaternion.Inverse(rigid.rotation) * _bodyRotation;
                frameRotation.ToAngleAxis(out var angle, out var axis);
                _angularVelocity = Vector3.up * (Vector3.Project(axis * angle, Vector3.up).y * Mathf.Deg2Rad / deltaTime);
            }
        }
        
        private void UpdateRigidbody(float deltaTime)
        {
            // sum all velocities
            rigid.velocity = velocity;

            // set angular velocity
            if (updateAngularVelocity) rigid.angularVelocity = _angularVelocity;
            if (useMoveRotation) rigid.MoveRotation(_bodyRotation);
            
            // dont touch. it is working. really. it is saving from sharp and small losing of ground
            rigid.useGravity = ground.isGround && moveDirection != Vector3.zero;
        }
        
        private void UpdateGround(float deltaTime)
        {
            // apply radius of player
            ground.radiusCheck = capsule.radius;
            
            // update ground info
            ground.UpdateGround(position, up);
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
            _initRotation = rigid.rotation;
            _bodyRotation = _initRotation;

            _viewAngle.y = eyes.localRotation.eulerAngles.x;
        }
        
        private void LateUpdate()
        {
            // because wasGrounded and isGrounded update in FixedUpdate. but use it in Update
            FixGround();
                
            UpdateBody();

            UpdateMove(Time.deltaTime);
            UpdateFall(Time.deltaTime);
            UpdatePlatform(Time.deltaTime);
            UpdateAir(Time.deltaTime);
            UpdateView(Time.deltaTime);

            UpdateRigidbody(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            UpdateGround(Time.fixedDeltaTime);
        }

        private void OnValidate()
        {
            UpdateBody();
        }

        private void OnDrawGizmos()
        {
            var ray = ground.GetRaySphereCast(position, up);
            var radius = ground.GetRadiusSphereCast();
            var distance = ground.GetDistanceSphereCast();
            
            var wasHit = Physics.SphereCast(ray, radius, out var hit, distance, ground.layerGround);
            
            var color = Color.white;
            color.a = 0.3f;
            Gizmos.color = color;
            Gizmos.DrawWireSphere(ray.origin, radius);
            
            if (wasHit)
            {
                color = Color.yellow;
                color.a = 0.2f;
                Gizmos.color = color;
                Gizmos.DrawWireSphere(ray.origin + ray.direction * distance, radius);

                color = Color.green;
                Gizmos.color = color;
                Gizmos.DrawWireSphere(hit.point + hit.normal * radius, radius);
                Gizmos.DrawLine(hit.point, hit.point + hit.normal * radius);
            }
            else
            {
                color = Color.red;
                Gizmos.color = color;
                Gizmos.DrawWireSphere(ray.origin + ray.direction * distance, radius);
            }
        }

        #endregion
    }
}