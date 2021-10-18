using System;
using UnityEngine;

namespace CucuTools.Surfaces
{
    public abstract class SurfaceEntity : CucuBehaviour
    {
        #region Abstract

        public abstract Vector3 GetLocalPoint(Vector2 uv);
        public abstract Vector3 GetLocalNormal(Vector2 uv);

        #endregion
        
        #region Virtual

        protected virtual Transform Root => transform;
        
        #endregion
        
        #region API
        
        public Vector3 position
        {
            get => Root.position;
            set => Root.position = value;
        }

        public Quaternion rotation
        {
            get => Root.rotation;
            set => Root.rotation = value;
        }
        
        public Vector3 GetPoint(Vector2 uv)
        {
            return Root.TransformPoint(GetLocalPoint(uv));
        }

        public Vector3 GetNormal(Vector2 uv)
        {
            return Root.TransformDirection(GetLocalNormal(uv));
        }
        
        public Vector3 GetLocalPoint(float u, float v)
        {
            return GetLocalPoint(new Vector2(u, v));
        }

        public Vector3 GetLocalNormal(float u, float v)
        {
            return GetLocalNormal(new Vector2(u, v));
        }
        
        public Vector3 GetPoint(float u, float v)
        {
            return GetPoint(new Vector2(u, v));
        }

        public Vector3 GetNormal(float u, float v)
        {
            return GetNormal(new Vector2(u, v));
        }

        public Vector3 GetPoint(Vector2 uv, out Vector3 normal)
        {
            normal = GetNormal(uv);
            return GetPoint(uv);
        }
        
        public Vector3 GetPoint(float u, float v, out Vector3 normal)
        {
            return GetPoint(new Vector2(u, v), out normal);
        }
        
        public Vector3 GetLocalPoint(Vector2 uv, out Vector3 localNormal)
        {
            localNormal = GetLocalNormal(uv);
            return GetLocalPoint(uv);
        }
        
        public Vector3 GetLocalPoint(float u, float v, out Vector3 localNormal)
        {
            return GetLocalPoint(new Vector2(u, v), out localNormal);
        }
        
        #endregion

        #region Static

        public static Vector3 LerpPoint(SurfaceEntity A, SurfaceEntity B, Vector2 uv, float t)
        {
            return Vector3.Lerp(A.GetPoint(uv), B.GetPoint(uv), t);
        }

        public static Vector3 LerpLocalPoint(SurfaceEntity A, SurfaceEntity B, Vector2 uv, float t)
        {
            return Vector3.Lerp(A.GetLocalPoint(uv), B.GetLocalPoint(uv), t);
        }

        public static Vector3 LerpNormal(SurfaceEntity A, SurfaceEntity B, Vector2 uv, float t)
        {
            return Vector3.Lerp(A.GetNormal(uv), B.GetNormal(uv), t);
        }

        public static Vector3 LerpLocalNormal(SurfaceEntity A, SurfaceEntity B, Vector2 uv, float t)
        {
            return Vector3.Lerp(A.GetLocalNormal(uv), B.GetLocalNormal(uv), t);
        }

        public static Vector3 Lerp(SurfaceEntity A, SurfaceEntity B, Vector2 uv, float t, out Vector3 normal)
        {
            normal = LerpNormal(A, B, uv, t);
            return LerpPoint(A, B, uv, t);
        }
        
        public static Vector3 LerpLocal(SurfaceEntity A, SurfaceEntity B, Vector2 uv, float t, out Vector3 localNormal)
        {
            localNormal = LerpLocalNormal(A, B, uv, t);
            return LerpLocalPoint(A, B, uv, t);
        }
        
        #endregion
        
        #region MonoBehaviour

        [SerializeField] protected GizmosSurface gizmosSurface;
        [SerializeField] private DebugPoint debugPoint;
        
        protected virtual void OnValidate()
        {
        }
        
        protected virtual void OnDrawGizmos()
        {
            gizmosSurface?.OnDrawGizmos(this);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            debugPoint?.OnDrawGizmos(this);
        }

        [Serializable]
        private class DebugPoint
        {
            public bool showDebug;
            
            public Vector2 coordPoint = new Vector2(0.5f, 0.5f);
            [Header("Point View")]
            [Range(0.001f, 1f)]
            public float radiusPoint = 0.01f;
            public bool wiredPoint;
            public Color colorPoint = Color.magenta;
            [Header("Line View")]
            public bool drawLineToPoint = true;
            
            public void OnDrawGizmos(SurfaceEntity surface)
            {
                if (!showDebug) return;   
                
                var point = surface.GetPoint(coordPoint);
                
                Gizmos.color = Color.Lerp(surface.gizmosSurface.GetUVColor(coordPoint), colorPoint, 0.5f);
                if (wiredPoint) Gizmos.DrawWireSphere(point, radiusPoint);
                else Gizmos.DrawSphere(point, radiusPoint);

                if (drawLineToPoint) Gizmos.DrawLine(surface.position, point);
            }
        }
        
        #endregion
    }
}