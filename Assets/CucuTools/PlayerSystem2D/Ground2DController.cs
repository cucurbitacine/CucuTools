using CucuTools.Others;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem2D
{
    public class Ground2DController : CucuBehaviour
    {
        public GameObject groundGameObject = null;
        
        [Space]
        public bool grounded = false;
        public bool platform = false;
        [HideInInspector]
        public bool wasGrounded = false;

        [Space]
        public Vector2 pointCheck = Vector2.zero;
        public Vector2 normalCheck = Vector2.up;
        public Vector2 gravity = Vector2.down * -9.81f;
        
        [Space]
        public UnityEvent<bool> onGroundChanged = new UnityEvent<bool>();

        [Space]
        public LayerMask layerGround = 1;
        public float radiusCheck = 0.5f;
        public float distanceCheck = 0.1f;
        [Range(0f, 90f)]
        public float maxAngleSlope = 60f;

        [Space]
        [Range(-0.1f, 0.1f)]
        public float radiusOffset = -0.01f;
        public bool fastCast = false;

        private RaycastHit2D _hit = default;
        
        private const float RaySphereCastOffset = 0.01f;
        
        private readonly CachedComponent<Rigidbody2D, Ground2DPlatform> _platformCache = new();
        
        private readonly RaycastHit2D[] _overlap = new RaycastHit2D[32];

        public RaycastHit2D hit => _hit;
        
        private bool IsPlatform(Rigidbody2D rigid)
        {
            if (rigid == null) return false;
            
            if (rigid.isKinematic) return true;

            if (_platformCache.TryGetValue(rigid, out var groundPlatform)) return true;

            return false;
        }

        private Ray GetRaySphereCast(Vector3 position, Vector3 normal)
        {
            return new Ray(position + normal * (radiusCheck + RaySphereCastOffset), -normal);
        }

        private float GetRadiusSphereCast()
        {
            return radiusCheck + radiusOffset;
        }

        private float GetDistanceSphereCast()
        {
            return distanceCheck + RaySphereCastOffset;
        }

        private bool PhysicsCast(Ray ray, float radius, out RaycastHit2D raycastHit, float distance)
        {
            if (fastCast)
            {
                raycastHit = Physics2D.CircleCast(ray.origin, radius, ray.direction, distance, layerGround);

                return raycastHit.collider != null;
            }
            
            return PhysicsCastNonAlloc(ray, radius, out raycastHit, distance);
        }

        private bool PhysicsCastNonAlloc(Ray ray, float radius, out RaycastHit2D raycastHit, float distance)
        {
            var count = Physics2D.CircleCastNonAlloc(ray.origin, radius, ray.direction, _overlap, distance, layerGround);

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

                    if (Vector2.Angle(normalCheck, _overlap[i].normal) < maxAngleSlope)
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
            grounded = PhysicsCast(ray, radius, out _hit, distance);
            
            if (grounded)
            {
                // check angle slope. it's looks like easy
                var angleSlope = Vector3.Angle(-gravity.normalized, hit.normal);
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
            if (Application.isEditor && !Application.isPlaying)
            {
                pointCheck = transform.position;
                normalCheck = transform.up;
                gravity = Physics2D.gravity;
            }
            
            var ray = GetRaySphereCast(pointCheck, normalCheck);
            var radius = GetRadiusSphereCast();
            var distance = GetDistanceSphereCast();

            var wasHit = PhysicsCast(ray, radius, out var tmp, distance);
            
            var color = Color.white;
            color.a = 0.3f;
            Gizmos.color = color;
            Gizmos.DrawWireSphere(ray.origin, radius);
            
            color = Color.magenta;
            color.a = 0.3f;
            Gizmos.color = color;
            Gizmos.DrawLine(pointCheck, pointCheck - gravity.normalized);
            
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
                Gizmos.DrawWireCube(ray.origin + ray.direction * distance, Vector3.one * radius * 2);
            }
            else
            {
                color = Color.red;
                Gizmos.color = color;
                Gizmos.DrawWireSphere(ray.origin + ray.direction * distance, radius);
                Gizmos.DrawWireCube(ray.origin + ray.direction * distance, Vector3.one * radius * 2);
            }
        }
    }
}