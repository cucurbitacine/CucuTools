using CucuTools;
using CucuTools.Colors;
using UnityEngine;

namespace Example.Scripts
{
    public class CucuSliderCircle : CucuSlider
    {
        public float Radius = 0.5f;

        public Vector3 LocalNormalEuler = Vector3.zero;
        public Vector3 LocalCenter = Vector3.zero;
        
        public Quaternion LocalNormalRotation => Quaternion.Euler(LocalNormalEuler);
        public Vector3 LocalNormal => LocalNormalRotation * Vector3.forward;
        public Vector3 LocalAxisX => LocalNormalRotation * Vector3.right;
        public Vector3 LocalAxisY => LocalNormalRotation * Vector3.up;
        public Vector3 Normal => transform.TransformDirection(LocalNormal);
        public Vector3 AxisX => transform.TransformDirection(LocalAxisX);
        public Vector3 AxisY => transform.TransformDirection(LocalAxisY);
        public Vector3 Center => transform.TransformPoint(LocalCenter);

        public override Vector3 HandlePosition => GetPointByProgress(Progress);
        
        public override Vector3 GetPointByProgress(float progress)
        {
            var rad = 2 * Mathf.PI * progress;
            return Center + (AxisX * Mathf.Cos(rad) + AxisY * Mathf.Sin(rad)) * Radius;
        }

        public override Vector3 GetPointByValue(float value)
        {
            return GetPointByProgress(Mathf.Repeat(value, 360f) / 360f);
        }

        public override float GetValueByPoint(Vector3 point, out Vector3 pointOnSlider)
        {
            var vector = Vector3.ProjectOnPlane(point - Center, Normal);
            
            pointOnSlider = vector.normalized * Radius + Center;
            
            var signedAngle = Vector3.SignedAngle(AxisX, vector, Normal);
            if (signedAngle < 0f) signedAngle += 360f;
            
            return (signedAngle / 360f) * Range + MinValue;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            
            var t = Cucu.LinSpace(32);
            var lastPoint = Vector3.zero;
            for (var i = 0; i < t.Length; i++)
            {
                var point = GetPointByProgress(t[i]);
                
                if (i != 0) Gizmos.DrawLine(lastPoint, point);
                
                lastPoint = point;
            }
            
            Gizmos.DrawLine(Center, Center + Normal);
            
            Gizmos.color = CucuColor.Jet.Evaluate(Progress);
            Gizmos.DrawLine(Center, HandlePosition);
            Gizmos.DrawWireSphere(HandlePosition, 0.1f);
        }
    }
}