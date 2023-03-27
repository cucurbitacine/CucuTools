using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem
{
    public class GroundController : MonoBehaviour
    {
        public RaycastHit hit = default;
        
        [Space]
        public bool onGround = false;
        public bool onPlatform = false;
        [HideInInspector]
        public bool wasOnGround = false;

        [Space]
        public Vector3 positionCheck = Vector3.zero;
        
        [Space]
        public UnityEvent<bool> onGroundChanged = new UnityEvent<bool>();

        [Space]
        public LayerMask layerGround = 1;
        public float radiusCheck = 0.5f;
        public float distanceCheck = 0.01f;
        [Range(0f, 90f)]
        public float maxAngleSlope = 60f;

        private bool _sphereCast = false;
        
        private readonly Dictionary<Rigidbody, GroundPlatform> _platformCache = new Dictionary<Rigidbody, GroundPlatform>();

        private bool IsPlatform(Rigidbody rigid)
        {
            if (rigid.isKinematic) return true;

            if (TryGetGroundPlatform(rigid, out var platform)) return true;

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
        
        private void FixGround()
        {
            if (wasOnGround && !_sphereCast) // left ground
            {
                if (onGround)
                {
                    onGround = false;
                }
                else
                {
                    wasOnGround = false;
                }
            }

            if (!wasOnGround && _sphereCast) // found ground
            {
                if (onGround)
                {
                    wasOnGround = true;
                }
                else
                {
                    onGround = true;
                }
            }

            if (!onGround) onPlatform = false;
        }
        
        public void UpdateGround(Vector3 point, Vector3 up)
        {
            var ray = GetRaySphereCast(point, up);
            var radius = GetRadiusSphereCast();
            var distance = GetDistanceSphereCast();
            
            // save previous value ground
            wasOnGround = _sphereCast;

            // check ground
            _sphereCast = Physics.SphereCast(ray, radius, out hit, distance, layerGround);

            if (_sphereCast)
            {
                // check angle slope. it's looks like easy
                var angleSlope = Vector3.Angle(up, hit.normal);
                _sphereCast = angleSlope < maxAngleSlope;
            }

            if (_sphereCast != wasOnGround) onGroundChanged.Invoke(_sphereCast);

            onPlatform = false;

            if (_sphereCast && hit.rigidbody != null)
            {
                onPlatform = IsPlatform(hit.rigidbody);
            }
        }
        
        private void LateUpdate()
        {
            // because wasGrounded and isGrounded update in FixedUpdate. but use it in Update
            FixGround();
        }

        private void FixedUpdate()
        {
            UpdateGround(positionCheck, Vector3.up);
        }
        
        private void OnDrawGizmos()
        {
            var ray = GetRaySphereCast(positionCheck, Vector3.up);
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