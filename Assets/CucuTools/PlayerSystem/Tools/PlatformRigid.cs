using UnityEngine;

namespace CucuTools.PlayerSystem.Tools
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlatformRigid : MonoBehaviour
    {
        public float timeMove = 0f;
        public float timePauseMove = 0f;
        [Range(0f, 1f)]
        public float blendMove = 0f;

        [Space]
        public bool move = true;
        public Vector3 directionMove = Vector3.forward;
        [Min(0.001f)]
        public float durationMove = 4f;
        [Min(0f)]
        public float durationPauseMove = 1f;
        
        [Space]
        public bool rotate = true;
        public float angularVelocity = 30f;
        public Vector3 axis = Vector3.up;

        private Rigidbody _rigid = null;
        private float _lastBlendMove = 0f;
        
        public Rigidbody rigid => _rigid != null ? _rigid : (_rigid = GetComponent<Rigidbody>());
        
        public Vector3 position
        {
            get => rigid.position;
            private set => rigid.MovePosition(value);
        }
        
        public Quaternion rotation
        {
            get => rigid.rotation;
            private set => rigid.MoveRotation(value);
        }
        
        public Vector3 startPosition { get; private set; }
        public Vector3 targetPosition => startPosition + directionMove;
        
        public Quaternion startRotation { get; private set; }

        private Vector3 GetPosition(float blend)
        {
            var t = Mathf.Clamp(blend * 2 - 1, -1f, 1f);
            t = Mathf.Abs(t);
            t = Mathf.SmoothStep(0f, 1f, t);
            return Vector3.Lerp(targetPosition, startPosition, t);
        }
        
        private void Awake()
        {
            startPosition = position;
            startRotation = rotation;

            timeMove = Mathf.Lerp(0f, durationMove, blendMove);
            timePauseMove = durationPauseMove;
        }

        private void FixedUpdate()
        {
            if (move)
            {
                _lastBlendMove = blendMove;
                blendMove = Mathf.Repeat(Mathf.Clamp01(timeMove / durationMove), 1f);

                if (_lastBlendMove < 0.5f && 0.5f <= blendMove)
                {
                    timePauseMove = durationPauseMove;
                }
            
                if (timePauseMove > 0)
                {
                    timePauseMove -= Time.fixedDeltaTime;
                    if (timePauseMove < 0) timePauseMove = 0f;
                }
                else
                {
                    timeMove += Time.fixedDeltaTime;
                    if (timeMove >= durationMove)
                    {
                        timeMove = 0f;
                    
                        timePauseMove = durationPauseMove;
                    }
                }
                
                position = GetPosition(blendMove);
            }

            if (rotate)
            {
                var dRot = Quaternion.Euler(axis.normalized * (angularVelocity * Time.fixedDeltaTime));
                rigid.MoveRotation(dRot * rigid.rotation);
            }
        }

        private void OnDrawGizmos()
        {
            
            
            if (move)
            {
                if (!Application.isPlaying)
                {
                    startPosition = position;

                    var renderers = GetComponentsInChildren<Renderer>();
                    Bounds bounds;
                    if (renderers.Length > 0)
                    {
                        bounds = renderers[0].bounds;

                        for (var i = 1; i < renderers.Length; i++)
                        {
                            bounds.Encapsulate(renderers[1].bounds);
                        }

                        CucuGizmos.DrawWireCube(bounds.center, bounds.size, transform.rotation);
                        CucuGizmos.DrawWireCube(bounds.center + directionMove, bounds.size, transform.rotation);
                        Gizmos.color = Color.yellow;
                        CucuGizmos.DrawWireCube(GetPosition(blendMove), bounds.size, transform.rotation);
                    }

                    Gizmos.DrawLine(startPosition, targetPosition);
                }
            }
        }
    }
}
