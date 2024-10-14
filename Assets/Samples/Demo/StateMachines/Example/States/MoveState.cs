using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class MoveState : StateBase<Transform>
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

        private Vector2 GetPoint(Transform target)
        {
            var targetPoint = point;
            
            if (fixedArea)
            {
                var center = GetAreaCenter(target);
                var size = GetAreaSize(target);
                    
                targetPoint.x = Mathf.Clamp(targetPoint.x, center.x - size.x * 0.5f, center.x + size.x * 0.5f);
                targetPoint.y = Mathf.Clamp(targetPoint.y, center.y - size.y * 0.5f, center.y + size.y * 0.5f);
            }

            return targetPoint;
        }
        
        private Vector2 GetAreaCenter(Transform target)
        {
            if (useParent && target.parent)
            {
                return target.parent.TransformPoint(areaCenter);
            }

            return areaCenter;
        }
        
        private Vector2 GetAreaSize(Transform target)
        {
            if (useParent && target.parent)
            {
                return target.parent.TransformVector(areaSize);
            }

            return areaSize;
        }
        
        protected override void OnExecute()
        {
            var targetPoint = GetPoint(Context);
            
            if (Vector2.Distance(Context.position, targetPoint) > threshold)
            {
                var direction = ((Vector3)targetPoint - Context.position).normalized;
                Context.position += direction * (speed * Time.deltaTime);
            }
            else
            {
                SetDone(true);
            }
        }

        private void OnDrawGizmos()
        {
            if (fixedArea)
            {
                var target = Context ? Context : transform;
                Gizmos.DrawWireCube(GetAreaCenter(target), GetAreaSize(target));
            }
            
            if (IsRunning && Context)
            {
                Gizmos.DrawLine(Context.position, GetPoint(Context));
                
                if (fixedArea)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(point, 0.5f);
                }
                
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(GetPoint(Context), 0.5f);
            }
        }
    }
}