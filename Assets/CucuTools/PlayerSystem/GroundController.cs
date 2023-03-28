using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem
{
    public class GroundController : MonoBehaviour
    {
        public RaycastHit hit = default;
        
        [Space]
        public bool grounded = false;
        public bool platform = false;
        [HideInInspector]
        public bool wasGrounded = false;

        [Space]
        public Vector3 pointCheck = Vector3.zero;
        public Vector3 normalCheck = Vector3.up;
        
        [Space]
        public UnityEvent<bool> onGroundChanged = new UnityEvent<bool>();

        [Space]
        public LayerMask layerGround = 1;
        public float radiusCheck = 0.5f;
        public float distanceCheck = 0.01f;
        [Range(0f, 90f)]
        public float maxAngleSlope = 60f;

        private readonly Dictionary<Rigidbody, GroundPlatform> _platformCache = new Dictionary<Rigidbody, GroundPlatform>();

        private bool IsPlatform(Rigidbody rigid)
        {
            if (rigid == null) return false;
            
            if (rigid.isKinematic) return true;

            if (TryGetGroundPlatform(rigid, out var groundPlatform)) return true;

            return false;
        }
        
        private bool TryGetGroundPlatform(Rigidbody rigid, out GroundPlatform platform)
        {
            if (!_platformCache.TryGetValue(rigid, out platform))
            {
                platform = rigid.GetComponent<GroundPlatform>();
                _platformCache.Add(rigid, platform);
            }

            return platform != null;
        }
        
        private Ray GetRaySphereCast(Vector3 position, Vector3 up)
        {
            return new Ray(position + up * (radiusCheck + distanceCheck), -up);
        }

        private float GetRadiusSphereCast()
        {
            return radiusCheck;
        }

        private float GetDistanceSphereCast()
        {
            return 2 * distanceCheck;
        }

        public void UpdateGround(Vector3 point, Vector3 normal)
        {
            var ray = GetRaySphereCast(point, normal);
            var radius = GetRadiusSphereCast();
            var distance = GetDistanceSphereCast();
            
            // save previous value ground
            wasGrounded = grounded;

            // check ground
            grounded = Physics.SphereCast(ray, radius, out hit, distance, layerGround);

            if (grounded)
            {
                // check angle slope. it's looks like easy
                var angleSlope = Vector3.Angle(normal, hit.normal);
                grounded = angleSlope < maxAngleSlope;
            }

            if (grounded != wasGrounded) onGroundChanged.Invoke(grounded);

            platform = false;

            if (grounded && hit.rigidbody != null)
            {
                platform = IsPlatform(hit.rigidbody);
            }
        }
        
        private void Update()
        {
            UpdateGround(pointCheck, normalCheck);
        }
        
        private void OnDrawGizmos()
        {
            var ray = GetRaySphereCast(pointCheck, Vector3.up);
            var radius = GetRadiusSphereCast();
            var distance = GetDistanceSphereCast();
            
            var wasHit = Physics.SphereCast(ray, radius, out var hit, distance, layerGround);
            
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
    }
}