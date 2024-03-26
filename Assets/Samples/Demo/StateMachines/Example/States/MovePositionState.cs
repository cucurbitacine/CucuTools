using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class MovePositionState : StateBase<IHavePosition>
    {
        [Header("MOVE")]
        [Min(0)] public float speed = 1f;
        [Min(0)] public float threshold = 0.1f;
        [Space] public Vector2 point;

        [Space]
        public bool fixedArea;
        public bool useParent = false;
        public Vector2 areaCenter = Vector2.zero;
        public Vector2 areaSize = Vector2.one;

        private Vector2 GetPoint()
        {
            var targetPoint = point;
            
            if (fixedArea)
            {
                var center = GetAreaCenter();
                var size = GetAreaSize();
                    
                targetPoint.x = Mathf.Clamp(targetPoint.x, center.x - size.x * 0.5f, center.x + size.x * 0.5f);
                targetPoint.y = Mathf.Clamp(targetPoint.y, center.y - size.y * 0.5f, center.y + size.y * 0.5f);
            }

            return targetPoint;
        }
        
        private Vector2 GetAreaCenter()
        {
            if (useParent && transform.parent)
            {
                return transform.parent.TransformPoint(areaCenter);
            }

            return areaCenter;
        }
        
        private Vector2 GetAreaSize()
        {
            if (useParent && transform.parent)
            {
                return transform.parent.TransformVector(areaSize);
            }

            return areaSize;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            var targetPoint = GetPoint();
            
            if (Vector2.Distance(core.position, targetPoint) > threshold)
            {
                var direction = (targetPoint - core.position).normalized;
                core.position += direction * (speed * deltaTime);
            }
            else
            {
                isDone = true;
            }
        }

        private void OnDrawGizmos()
        {
            if (fixedArea)
            {
                Gizmos.DrawWireCube(GetAreaCenter(), GetAreaSize());
            }
            
            if (isActive && core != null)
            {
                Gizmos.DrawLine(core.position, GetPoint());
                
                if (fixedArea)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(point, 0.5f);
                }
                
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(GetPoint(), 0.5f);
            }
        }
    }
}