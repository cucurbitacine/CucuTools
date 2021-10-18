using CucuTools.Attributes;
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

        [CucuButton("Add", group: "Drawer")]
        protected void AddDrawer()
        {
            var drawer = gameObject.GetComponent<SurfaceDrawer>();
            if (drawer == null) drawer = gameObject.AddComponent<SurfaceDrawer>();
        }

        [CucuButton("Remove", group: "Drawer")]
        protected void RemoveDrawer()
        {
            var drawer = gameObject.GetComponent<SurfaceDrawer>();
            if (drawer != null) DestroyImmediate(drawer);
        }
        
        [CucuButton("Add", group: "Debugger")]
        protected void AddDebugger()
        {
            var debagger = gameObject.GetComponent<SurfaceDebug>();
            if (debagger == null) debagger = gameObject.AddComponent<SurfaceDebug>();
        }

        [CucuButton("Remove", group: "Debugger")]
        protected void RemoveDebugger()
        {
            var debagger = gameObject.GetComponent<SurfaceDebug>();
            if (debagger != null) DestroyImmediate(debagger);
        }
        
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
    }
}