using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem
{
    public class GroundController : MonoBehaviour
    {
        public RaycastHit hit = default;
        public GameObject groundGameObject = null;
        
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
        public float distanceCheck = 0.1f;
        [Range(0f, 90f)]
        public float maxAngleSlope = 60f;

        [Space]
        public bool fastCast = false;
        
        private readonly Dictionary<Rigidbody, GroundPlatform> _platformCache = new Dictionary<Rigidbody, GroundPlatform>();
        private readonly RaycastHit[] _overlap = new RaycastHit[32];
        
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
        
        private Ray GetRaySphereCast(Vector3 position, Vector3 normal)
        {
            return new Ray(position + normal * (radiusCheck + distanceCheck), -normal);
        }

        private float GetRadiusSphereCast()
        {
            return radiusCheck;
        }

        private float GetDistanceSphereCast()
        {
            return 2 * distanceCheck;
        }

        private bool PhysicsCast(Ray ray, float radius, out RaycastHit raycastHit, float distance)
        {
            if (fastCast)
            {
                return Physics.SphereCast(ray, radius, out raycastHit, distance, layerGround, QueryTriggerInteraction.Ignore);
            }
            
            return PhysicsCastNonAlloc(ray, radius, out raycastHit, distance);
        }

        private bool PhysicsCastNonAlloc(Ray ray, float radius, out RaycastHit raycastHit, float distance)
        {
            var count = Physics.SphereCastNonAlloc(ray, radius, _overlap, distance, layerGround, QueryTriggerInteraction.Ignore);

            if (count == 0)
            {
                raycastHit = default;
                return false;
            }
            
            var index = 0;

            if (count > 1)
            {
                var distanceMax = float.MaxValue;
                for (var i = 0; i < count; i++)
                {
                    if (_overlap[i].collider.gameObject == gameObject) continue;

                    if (Vector3.Angle(normalCheck, _overlap[i].normal) < maxAngleSlope)
                    {
                        if (_overlap[i].distance < distanceMax)
                        {
                            distanceMax = _overlap[i].distance;
                            index = i;
                        }
                    }
                }
            }
            
            raycastHit = _overlap[index];

            return raycastHit.collider.gameObject != gameObject;
        }

        public void UpdateGround()
        {
            var ray = GetRaySphereCast(pointCheck, normalCheck);
            var radius = GetRadiusSphereCast();
            var distance = GetDistanceSphereCast();
            
            // save previous value ground
            wasGrounded = grounded;

            // check ground
            grounded = PhysicsCast(ray, radius, out hit, distance);

            if (grounded)
            {
                // check angle slope. it's looks like easy
                var angleSlope = Vector3.Angle(normalCheck, hit.normal);
                grounded = angleSlope < maxAngleSlope;
            }

            if (grounded != wasGrounded) onGroundChanged.Invoke(grounded);

            platform = false;

            if (grounded && hit.rigidbody != null)
            {
                platform = IsPlatform(hit.rigidbody);
            }

            if (grounded)
            {
                groundGameObject = hit.collider.gameObject;
            }
            else groundGameObject = null;
        }
        
        private void Update()
        {
            UpdateGround();
        }
        
        private void OnDrawGizmos()
        {
            var ray = GetRaySphereCast(pointCheck, normalCheck);
            var radius = GetRadiusSphereCast();
            var distance = GetDistanceSphereCast();

            var wasHit = PhysicsCast(ray, radius, out var tmp, distance);
            
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
                Gizmos.DrawWireSphere(tmp.point + tmp.normal * radius, radius);
                Gizmos.DrawLine(tmp.point, tmp.point + tmp.normal * radius);
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