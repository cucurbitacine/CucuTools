using UnityEngine;

namespace CucuTools.Surfaces
{
    public class SphereSurface : SurfaceEntity
    {
        [Header("Sphere")]  
        [SerializeField] private float radius = 0.5f;
        
        [Header("Latitude")]
        [Range(0, 360f)]
        [SerializeField] private float _minLatitude = 0f;
        [Range(0, 360f)]
        [SerializeField] private float _maxLatitude = 360f;
        
        [Header("Longitude")]
        [Range(-90, 90f)]
        [SerializeField] private float minLongitude = -90f;
        [Range(-90, 90f)]
        [SerializeField] private float maxLongitude = 90f;
        
        public float Radius
        {
            get => radius;
            set => radius = value;
        }
        
        /// <summary>
        /// Минимальный угол
        /// </summary>
        public float MinLatitude
        {
            get => _minLatitude;
            set => _minLatitude = Mathf.Clamp(value, 0f, 360f);
        }

        /// <summary>
        /// Максимальный угол
        /// </summary>
        public float MaxLatitude
        {
            get => _maxLatitude;
            set => _maxLatitude = Mathf.Clamp(value, 0f, 360f);
        }

        /// <summary>
        /// Минимальный угол
        /// </summary>
        public float MinLongitude
        {
            get => minLongitude;
            set => minLongitude = Mathf.Clamp(value, -90f, 90f);
        }

        /// <summary>
        /// Максимальный угол
        /// </summary>
        public float MaxLongitude
        {
            get => maxLongitude;
            set => maxLongitude = Mathf.Clamp(value, -90f, 90f);
        }
        
        public override Vector3 GetLocalPoint(Vector2 uv)
        {
            var rad = ((1 - uv.x) * MinLatitude + uv.x * MaxLatitude) * Mathf.Deg2Rad;
            var rad2 = ((1 - uv.y) * MinLongitude + uv.y * MaxLongitude)* Mathf.Deg2Rad;
            
            rad2 = Mathf.PI - (rad2 + Mathf.PI / 2);
            return new Vector3(Mathf.Cos(rad) * Mathf.Sin(rad2), Mathf.Sin(rad) * Mathf.Sin(rad2), Mathf.Cos(rad2)) *
                   Radius;
        }

        public override Vector3 GetLocalNormal(Vector2 uv)
        {
            return GetLocalPoint(uv).normalized * Mathf.Sign(Radius);
        }
    }
}