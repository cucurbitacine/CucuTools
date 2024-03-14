using CucuTools;
using UnityEngine;

namespace Samples.Demo.Extensions
{
    public class TangentPoint : MonoBehaviour
    {
        public BezierPoint point;
        public LineRenderer line;

        private void UpdateLine()
        {
            if (line)
            {
                line.positionCount = 2;
                if (point)
                {
                    line.SetPosition(0, point.bezier.pointIn);
                    line.SetPosition(1, point.bezier.pointTangentIn);
                }
            }
        }
        
        private void UpdateTangent()
        {
            if (!point) return;
            
            if (point.bezier.autoTangent && point.bezier.autoWeight)
            {
                transform.position = point.bezier.pointTangentIn;
            }
            
            if (!point.bezier.autoTangent && !point.bezier.autoWeight)
            {
                point.bezier.pointTangentIn = transform.position;
            }
        }

        private void Update()
        {
            UpdateTangent();

            UpdateLine();
        }

        private void OnValidate()
        {
            UpdateTangent();

            UpdateLine();
        }
    }
}