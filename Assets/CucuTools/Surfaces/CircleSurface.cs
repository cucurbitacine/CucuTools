using UnityEngine;

namespace CucuTools.Surfaces
{
    public class CircleSurface : SurfaceEntity
    {
        [Header("Circle")]
        [Range(0, 360)]
        [SerializeField] private float _minAngle = 0f;
        [Range(0, 360)]
        [SerializeField] private float _maxAngle = 360f;
        [Min(0)]
        [SerializeField] private float _innerRadius = 0f;
        [Min(0)]
        [SerializeField] private float _outerRadius = 0.5f;

        /// <summary>
        /// Минимальный угол
        /// </summary>
        public float MinAngle
        {
            get => _minAngle;
            set => _minAngle = value;
        }

        /// <summary>
        /// Максимальный угол
        /// </summary>
        public float MaxAngle
        {
            get => _maxAngle;
            set => _maxAngle = value;
        }
        
        /// <summary>
        /// Внутренний радиус
        /// </summary>
        public float InnerRadius
        {
            get => _innerRadius;
            set => _innerRadius = value;
        }

        /// <summary>
        /// Внешний радиус
        /// </summary>
        public float OuterRadius
        {
            get => _outerRadius;
            set => _outerRadius = value;
        }
        
        public Vector3 Normal => Root.forward;
        public Vector3 LocalNormal => Root.InverseTransformDirection(Normal);
        
        public override Vector3 GetLocalPoint(Vector2 uv)
        {
            var r = (1 - uv.x) * InnerRadius + uv.x * OuterRadius;
            var deg = (1 - uv.y) * MinAngle + uv.y * MaxAngle;
            var rad = Mathf.Deg2Rad * deg;
            return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * r;
        }

        public override Vector3 GetLocalNormal(Vector2 uv)
        {
            return LocalNormal;
        }
    }
}