using UnityEngine;

namespace CucuTools.Surfaces
{
    public class PipeSurface : SurfaceEntity
    {
        [Header("Pipe")]
        [SerializeField] private float height = 1f;
        [SerializeField] private float topRadius = 0.5f;
        [SerializeField] private float bottomRadius = 0.5f;
        
        public float TopRadius
        {
            get => topRadius;
            set => topRadius = value;
        }
        
        public float BottomRadius
        {
            get => bottomRadius;
            set => bottomRadius = value;
        }
        
        public float Height
        {
            get => height;
            set => height = value;
        }
        
        public override Vector3 GetLocalPoint(Vector2 uv)
        {
            var rad = uv.x * 2 * Mathf.PI;
            var r = (1f - uv.y) * BottomRadius + uv.y * TopRadius;
            var h = Height * uv.y;
            
            return new Vector3(r * Mathf.Cos(rad), r * Mathf.Sin(rad), h);
        }

        public override Vector3 GetLocalNormal(Vector2 uv)
        {
            var alpha = Mathf.Atan((BottomRadius - TopRadius) / Height) * Mathf.Rad2Deg;
            var r = (1f - uv.y) * BottomRadius + uv.y * TopRadius;

            var dir = GetLocalPoint(uv).Scale(1, 1, 0).normalized * Mathf.Sign(r);
            dir = Quaternion.AngleAxis(alpha, Vector3.Cross(dir, Root.forward)) * dir;

            return dir;
        }
    }
}