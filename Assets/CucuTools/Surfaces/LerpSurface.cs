using UnityEngine;

namespace CucuTools.Surfaces
{
    public class LerpSurface : SurfaceEntity
    {
        [Range(0, 1)]
        public float T;
        public SurfaceEntity SurfaceA;
        public SurfaceEntity SurfaceB;
        
        public override Vector3 GetLocalPoint(Vector2 uv)
        {
            if (SurfaceA == null || SurfaceB == null) return Vector3.zero;

            var pnt = SurfaceEntity.LerpPoint(SurfaceA, SurfaceB, uv, T);
            return Root.InverseTransformPoint(pnt);
        }

        public override Vector3 GetLocalNormal(Vector2 uv)
        {
            if (SurfaceA == null || SurfaceB == null) return Root.forward;
            
            var nml = SurfaceEntity.LerpNormal(SurfaceA, SurfaceB, uv, T);
            return Root.InverseTransformDirection(nml);
        }
    }
}