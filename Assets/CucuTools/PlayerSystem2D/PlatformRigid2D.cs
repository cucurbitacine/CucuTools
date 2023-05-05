using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlatformRigid2D : MonoBehaviour
    {
        public bool active = true;
        [Range(0f, 1f)]
        public float blendMove = 0f;

        [Header("Movement")]
        public bool move = true;
        public Vector2 movementDirection = Vector2.right;
        [Min(0.001f)]
        public float movementDuration = 4f;
        [Min(0f)]
        public float pauseDuration = 1f;
        public bool smoothMovement = true;
        
        [Header("Rotating")]
        public bool rotate = false;
        public float rotatingSpeed = 10f;
        
        private Rigidbody2D _rigid = null;
        private float _lastBlendMove = 0f;
        private float _timeMove = 0f;
        private float _timePauseMove = 0f;
        
        public Rigidbody2D rigid => _rigid != null ? _rigid : (_rigid = GetComponent<Rigidbody2D>());
        
        public Vector2 position
        {
            get => rigid.position;
            private set => rigid.MovePosition(value);
        }

        public float rotation
        {
            get => rigid.rotation;
            private set => rigid.MoveRotation(value);
        }
        
        public Vector2 startPosition { get; private set; }
        public Vector2 targetPosition => startPosition + movementDirection;
        
        private Vector2 GetPosition(float blend)
        {
            var t = Mathf.Clamp(blend * 2 - 1, -1f, 1f);
            t = Mathf.Abs(t);
            if (smoothMovement) t = Mathf.SmoothStep(0f, 1f, t);
            return Vector2.Lerp(targetPosition, startPosition, t);
        }

        private bool CanMove() => active && move;
        private bool CanRotate() => active && rotate;

        private void UpdateConstraints()
        {
            var moveConstraints = CanMove() ? RigidbodyConstraints2D.None : RigidbodyConstraints2D.FreezePosition; 
            var rotateConstraints = CanRotate() ? RigidbodyConstraints2D.None : RigidbodyConstraints2D.FreezeRotation;

            rigid.constraints = moveConstraints | rotateConstraints;
        }
        
        private void UpdateMovement(float deltaTime)
        {
            _lastBlendMove = blendMove;
            blendMove = Mathf.Repeat(Mathf.Clamp01(_timeMove / (movementDuration * 2f)), 1f);

            if (_lastBlendMove < 0.5f && 0.5f <= blendMove)
            {
                _timePauseMove = pauseDuration;
            }
            
            if (_timePauseMove > 0)
            {
                _timePauseMove -= deltaTime;
                if (_timePauseMove < 0) _timePauseMove = 0f;
            }
            else
            {
                _timeMove += deltaTime;
                if (_timeMove >= movementDuration * 2f)
                {
                    _timeMove = 0f;
                    
                    _timePauseMove = pauseDuration;
                }
            }
                
            position = GetPosition(blendMove);
        }

        private void UpdateRotating(float deltaTime)
        {
            rotation += rotatingSpeed * deltaTime;
        }
        
        private void Awake()
        {
            startPosition = position;

            _timeMove = Mathf.Lerp(0f, movementDuration * 2f, blendMove);
            _timePauseMove = pauseDuration;
        }

        private void FixedUpdate()
        {
            UpdateConstraints();

            if (CanMove()) UpdateMovement(Time.fixedDeltaTime);

            if (CanRotate()) UpdateRotating(Time.fixedDeltaTime);
        }

        private void OnValidate()
        {
            UpdateConstraints();
        }

        private void OnDrawGizmos()
        {
            if (CanMove())
            {
                if (!Application.isPlaying)
                {
                    startPosition = position;

                    var bounds = Cucu.GetBounds(gameObject);

                    Gizmos.color = Color.white;
                    CucuGizmos.DrawWireCube(bounds.center, bounds.size, transform.rotation);
                    CucuGizmos.DrawWireCube(bounds.center + (Vector3)movementDirection, bounds.size, transform.rotation);
                    if (CanRotate())
                    {
                        Gizmos.DrawWireSphere(bounds.center, Mathf.Max(bounds.size.x, bounds.size.y) * 0.5f);
                        Gizmos.DrawWireSphere(bounds.center + (Vector3)movementDirection, Mathf.Max(bounds.size.x, bounds.size.y) * 0.5f);
                    }
                    
                    Gizmos.color = Color.yellow;
                    CucuGizmos.DrawWireCube(GetPosition(blendMove), bounds.size, transform.rotation);
                    
                    Gizmos.color = Color.Lerp(Color.white, Color.yellow, 0.5f);
                    Gizmos.DrawLine(startPosition, targetPosition);
                }
            }else if (CanRotate())
            {
                var bounds = Cucu.GetBounds(gameObject);

                Gizmos.DrawWireSphere(rigid.position, Mathf.Max(bounds.size.x, bounds.size.y) * 0.5f);
            }
        }
    }
}