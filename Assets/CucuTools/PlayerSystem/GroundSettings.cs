using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem
{
    [Serializable]
    public class GroundSettings
    {
        public bool isGround = false;
        public bool isPlatform = false;
        
        [HideInInspector]
        public bool wasGround = false;

        [Space]
        public RaycastHit hit = default;
        public UnityEvent<bool> onGroundChanged = new UnityEvent<bool>();

        [Space]
        public float radiusCheck = 0.01f;
        public float distanceCheck = 0.01f;
        [Range(0f, 90f)]
        public float maxAngleSlope = 60f;
        
        [Space]
        public LayerMask layerGround = 1;

        private readonly Dictionary<Rigidbody, GroundPlatform> _platformCache = new Dictionary<Rigidbody, GroundPlatform>();

        public Ray GetRaySphereCast(Vector3 position, Vector3 up)
        {
            return new Ray(position + up * (radiusCheck + distanceCheck), -up);
        }
        
        public float GetRadiusSphereCast()
        {
            return radiusCheck;
        }
        
        public float GetDistanceSphereCast()
        {
            return 2 * distanceCheck;
        }
        
        public void UpdateGround(Vector3 position, Vector3 up)
        {
            var ray = GetRaySphereCast(position, up);
            var radius = GetRadiusSphereCast();
            var distance = GetDistanceSphereCast();
            
            // save previous value ground
            wasGround = isGround;

            // check ground
            isGround = Physics.SphereCast(ray, radius, out hit, distance, layerGround);

            if (isGround)
            {
                // check angle slope. it's looks like easy
                var angleSlope = Vector3.Angle(up, hit.normal);
                isGround = angleSlope < maxAngleSlope;
            }

            if (isGround != wasGround) onGroundChanged.Invoke(isGround);

            isPlatform = false;

            if (isGround && hit.rigidbody != null)
            {
                isPlatform = IsPlatform(hit.rigidbody);
            }
        }

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
    }
}