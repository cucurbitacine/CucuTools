using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Samples.Demo.StateMachines
{
    [Serializable]
    public class FixedArea
    {
        public bool fixedArea;
        
        [Space]
        public bool useParent = false;
        public Transform parent = null;
        
        [Space]
        public Vector2 areaCenter = Vector2.zero;
        public Vector2 areaSize = Vector2.one;

        public Vector2 Evaluate(Vector2 point)
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

        public Vector2 GetRandom()
        {
            var center = GetAreaCenter();
            var size = GetAreaSize();

            var point = new Vector2(Random.value, Random.value) - Vector2.one * 0.5f;

            point = Vector2.Scale(point, size) + center;

            return point;
        }
        
        public Vector2 GetAreaCenter()
        {
            if (useParent && parent)
            {
                return parent.TransformPoint(areaCenter);
            }

            return areaCenter;
        }
        
        public Vector2 GetAreaSize()
        {
            if (useParent && parent)
            {
                return parent.TransformVector(areaSize);
            }

            return areaSize;
        }
    }
}