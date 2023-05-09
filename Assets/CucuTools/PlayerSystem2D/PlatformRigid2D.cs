using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlatformRigid2D : MonoBehaviour
    {
        public bool active = true;

        [Header("Movement")]
        public bool move = true;
        public Vector2 directionMovement = Vector2.right;
        [Min(0.001f)]
        public float durationMovement = 4f;
        public bool smoothMovement = true;
        
        [Header("Rotating")]
        public bool rotate = false;
        public float phaseRotation = 0f;
        public float speedRotation = 10f;
        
        private Rigidbody2D _rigid = null;
        
        private float _timeMoving = 0f;
        private float _timeRotating = 0f;
        
        public Rigidbody2D rigidbody2d => _rigid != null ? _rigid : (_rigid = GetComponent<Rigidbody2D>());

        public Vector2 startMovement { get; private set; }
        
        public Vector2 endMovement
        {
            get => startMovement + directionMovement;
            set => directionMovement = value - startMovement;
        }

        public float distanceMovement => Vector2.Distance(startMovement, endMovement);
        public float speedMovement => distanceMovement / durationMovement;
        
        private bool CanMove() => active && move;
        private bool CanRotate() => active && rotate;

        private void UpdateRigidbody()
        {
            var moveConstraints = CanMove() ? RigidbodyConstraints2D.None : RigidbodyConstraints2D.FreezePosition; 
            var rotateConstraints = CanRotate() ? RigidbodyConstraints2D.None : RigidbodyConstraints2D.FreezeRotation;

            rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
            rigidbody2d.constraints = moveConstraints | rotateConstraints;
        }
        
        private void UpdateMovement(float deltaTime)
        {
            var lerp = Mathf.PingPong(_timeMoving / durationMovement, 1f);

            if (smoothMovement)
            {
                lerp = Mathf.SmoothStep(0f, 1f, lerp);
            }

            var position = Vector2.Lerp(startMovement, endMovement, lerp);

            rigidbody2d.velocity = (position - rigidbody2d.position) / deltaTime;
            
            _timeMoving += Time.fixedDeltaTime;
            _timeMoving = Mathf.Repeat(_timeMoving, 2 * durationMovement);
        }
        
        private void UpdateRotating(float deltaTime)
        {
            var fullPeriod = 360 / Mathf.Abs(speedRotation);
            if (float.IsNaN(fullPeriod) || float.IsInfinity(fullPeriod)) return;
            
            _timeRotating += deltaTime;
            _timeRotating = Mathf.Repeat(_timeRotating, fullPeriod);
            
            var rotation = phaseRotation + speedRotation * _timeRotating;
            while (rotation < 0) rotation += 360;
            rotation = Mathf.Repeat(rotation, 360);
            
            rigidbody2d.rotation = rotation;
        }
        
        private void Awake()
        {
            startMovement = rigidbody2d.position;
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();

            if (CanMove()) UpdateMovement(Time.fixedDeltaTime);

            if (CanRotate()) UpdateRotating(Time.fixedDeltaTime);
        }

        private void OnValidate()
        {
            UpdateRigidbody();
        }

        private void OnDrawGizmos()
        {
            if (CanMove())
            {
                var bounds = Cucu.GetBounds(gameObject);
                var rotation = transform.rotation;

                if (CanRotate())
                {
                    rotation = Quaternion.Euler(0, 0, phaseRotation);
                }
                
                var start = (Vector2)bounds.center;
                var end = start + directionMovement;
                
                if (Application.isPlaying)
                {
                    start = startMovement;
                    end = endMovement;
                }

                CucuGizmos.DrawWireCube(start, bounds.size, rotation);
                Gizmos.DrawLine(start, end);
                CucuGizmos.DrawWireCube(end, bounds.size, rotation);
                
                Gizmos.color = Color.yellow;
                CucuGizmos.DrawWireCube(bounds.center, bounds.size, rotation);

                if (CanRotate())
                {
                    Gizmos.DrawWireSphere(bounds.center, Mathf.Max(bounds.size.x, bounds.size.y) * 0.5f);
                }
            }
            else if (CanRotate())
            {
                var bounds = Cucu.GetBounds(gameObject);

                Gizmos.color = Color.yellow;
                CucuGizmos.DrawWireCube(bounds.center, bounds.size, Quaternion.Euler(0, 0, phaseRotation));
                Gizmos.DrawWireSphere(bounds.center, Mathf.Max(bounds.size.x, bounds.size.y) * 0.5f);
            }
        }
    }
}