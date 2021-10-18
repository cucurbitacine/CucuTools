using UnityEngine;

namespace CucuTools.Surfaces
{
    public class BoxSurface : SurfaceEntity
    {
        [Header("Box")]
        [SerializeField] private Vector2 _size = Vector2.one;

        public Vector2 Size
        {
            get => _size;
            set => _size = value;
        }

        public Vector3 Normal => Root.forward;
        public Vector3 LocalNormal => Root.InverseTransformDirection(Normal);

        public override Vector3 GetLocalPoint(Vector2 uv)
        {
            return (uv - Vector2.one * 0.5f) * Size;
        }

        public override Vector3 GetLocalNormal(Vector2 uv)
        {
            return LocalNormal;
        }
    }
}