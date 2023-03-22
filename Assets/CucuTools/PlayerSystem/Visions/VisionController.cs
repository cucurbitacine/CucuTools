using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem.Visions
{
    public class VisionController : MonoBehaviour
    {
        [Space]
        public bool isOn = true;
        
        [Header("Info")]
        public bool seeSomething = false;
        public RaycastHit current = default;
        public RaycastHit last = default;

        [Header("Settings")]
        [Min(0f)]
        public float distanceVision = 100f;
        [Min(0f)]
        public float radiusVision = 0.02f;
        public LayerMask layerVision = 1;

        [Space]
        [Tooltip("First current hit, second last hit")]
        public UnityEvent<RaycastHit, RaycastHit> onVisionChanged = new UnityEvent<RaycastHit, RaycastHit>();
        
        [Header("References")]
        public Transform eyes = null;
        
        [Space]
        [SerializeField] private bool debugLog = true;
        
        public Vector3 origin => eyes.position;
        public Vector3 direction => eyes.forward;
        public Ray ray => new Ray(origin, direction);
        
        public bool Cast(out RaycastHit hit)
        {
            return Physics.Raycast(ray, out hit, distanceVision, layerVision) ||
                   Physics.SphereCast(ray, radiusVision, out hit, distanceVision, layerVision);
        }
        
        private void UpdateVision(float deltaTime)
        {
            last = current;

            seeSomething = Cast(out current);

            if (last.collider != current.collider)
            {
                onVisionChanged.Invoke(current, last);
            }
        }

        private void DebugLog(RaycastHit cur, RaycastHit lst)
        {
            Debug.Log($"Vision :: Lost \"{(lst.collider != null ? lst.collider.name : "???")}\"");
            Debug.Log($"Vision :: See  \"{(cur.collider != null ? cur.collider.name : "???")}\"");
        }
        
        private void OnEnable()
        {
            if (eyes == null) eyes = transform;
            if (debugLog) onVisionChanged.AddListener(DebugLog);
        }

        private void OnDisable()
        {
            onVisionChanged.RemoveListener(DebugLog);
        }

        private void FixedUpdate()
        {
            if (isOn) UpdateVision(Time.fixedDeltaTime);
        }

        private void OnValidate()
        {
            if (eyes == null) eyes = transform;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin, 0.1f);
            
            if (seeSomething)
            {
                var point = current.point + current.normal * radiusVision;
                Gizmos.DrawLine(origin, point);
                
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(point, radiusVision);
            }
            else
            {
                var point = origin + direction * distanceVision;
                Gizmos.DrawLine(origin, point);
                
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(point, radiusVision);
            }
        }
    }
}