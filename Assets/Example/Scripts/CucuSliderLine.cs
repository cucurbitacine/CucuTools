using CucuTools;
using CucuTools.Colors;
using UnityEngine;
using UnityEngine.Events;

namespace Example.Scripts
{
    public class CucuSliderLine : CucuSlider
    {
        [Header("Line")]
        [Min(0f)]
        [SerializeField] private float length = 1f;
        [SerializeField] private Vector3 localAxis = Vector3.right;
        [SerializeField] private Vector3 offset = Vector3.zero;
        
        public float Length
        {
            get => length;
            set => length = value;
        }
        
        public Vector3 LocalAxis
        {
            get => localAxis.normalized;
            set => localAxis = value.normalized;
        }

        public Vector3 Axis
        {
            get => transform.TransformDirection(LocalAxis);
            set => LocalAxis = transform.InverseTransformDirection(value);
        }
        
        public Vector3 Offset
        {
            get => offset;
            set => offset = value;
        } 

        public Vector3 BeginSlider => transform.TransformPoint(Offset);
        public Vector3 EndSlider => BeginSlider + Axis * Length;

        public override Vector3 HandlePosition => GetPointByProgress(Progress);
        
        public override Vector3 GetPointByProgress(float progress)
        {
            return Vector3.Lerp(BeginSlider, EndSlider, progress);
        }

        public override Vector3 GetPointByValue(float value)
        {
            return GetPointByProgress(value / Range);
        }

        public override float GetValueByPoint(Vector3 point, out Vector3 pointOnSlider)
        {
            pointOnSlider = point.GetNearestPointOnLine(BeginSlider, EndSlider);

            var dot = Vector3.Dot(pointOnSlider - BeginSlider, EndSlider - BeginSlider);

            return Mathf.Sign(dot) * Vector3.Distance(BeginSlider, pointOnSlider);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(BeginSlider, EndSlider);
            
            Gizmos.color = CucuColor.Jet.Evaluate(0f);
            Gizmos.DrawSphere(BeginSlider, 0.1f);
            Gizmos.color = CucuColor.Jet.Evaluate(1f);
            Gizmos.DrawSphere(EndSlider, 0.1f);

            Gizmos.color = CucuColor.Jet.Evaluate(Progress);
            Gizmos.DrawWireSphere(HandlePosition, 0.1f);
        }
    }
}
