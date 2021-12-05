using System;
using CucuTools.Avatar;
using UnityEngine;

namespace Example.Scripts
{
    public class Avatar3DCameraFollower : MonoBehaviour
    {
        private readonly RaycastHit[] hits = new RaycastHit[16];

        private Vector3 _targetPoint;
        
        public float SpeedDamp = 4f;
        
        [Range(0, 1)]
        public float DynamicOffsetWidth = 0.5f;
        
        public Vector3 Offset = Vector3.back + Vector3.up;
        public float RadiusSphereCast = 0.2f;
        public float MaxDistanceCast = 4f;
        public LayerMask LayerMaskCast = 1;
        public QueryTriggerInteraction InteractionCast = QueryTriggerInteraction.Ignore;
        
        public CucuAvatar CucuAvatar;
        
        public Transform Follow => CucuAvatar.Body;
        public Transform LookAt => CucuAvatar.Head;
        
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _targetPoint, SpeedDamp * Time.deltaTime);
            transform.LookAt(LookAt);
        }

        private void FixedUpdate()
        {
            if (CucuAvatar == null) return;

            var offset =Vector3.Lerp(Offset, -CucuAvatar.InputInfo.move, DynamicOffsetWidth);
            
            var origin = LookAt.position;
            var direction = Follow.TransformDirection(offset.normalized).normalized;
            var count = Physics.SphereCastNonAlloc(origin, RadiusSphereCast, direction, hits, MaxDistanceCast, LayerMaskCast, InteractionCast);

            for (var i = 0; i < count; i++)
            {
                var hit = hits[i];
                if (hit.transform == Follow || hit.transform == LookAt) continue;

                var point = hit.point + hit.normal * RadiusSphereCast;

                _targetPoint = point;
                
                return;
            }

            _targetPoint = Follow.position + direction * MaxDistanceCast;
        }
    }
}
