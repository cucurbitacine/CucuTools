using System;
using UnityEngine;

namespace CucuTools.Surfaces
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SurfaceEntity))]
    public class SurfaceDebug : CucuBehaviour
    {
        [SerializeField] private bool isDrawing = true;
        [SerializeField] private SurfaceEntity surface;
        [SerializeField] private SurfaceDebugPoint debugPoint;

        public bool IsDrawing
        {
            get => isDrawing;
            set => isDrawing = value;
        }

        public SurfaceEntity Surface => surface != null ? surface : (surface = GetComponent<SurfaceEntity>());

        public SurfaceDebugPoint DebugPoint => debugPoint != null ? debugPoint : (debugPoint = new SurfaceDebugPoint());

        private void OnDrawGizmos()
        {
            if (IsDrawing) DebugPoint.Draw(Surface);
        }
    }

    [Serializable]
    public class SurfaceDebugPoint
    {
        [Header("Point View")]
        public bool showPoint = true;
        public Vector2 coordPoint = new Vector2(0.5f, 0.5f);
        [Range(0.001f, 1f)]
        public float radiusPoint = 0.03f;
        public bool wiredPoint;
        public Color colorPoint = Color.magenta;
        
        [Header("Line View")]
        public bool showLine = true;

        public void Draw(SurfaceEntity surface)
        {
            if (!showPoint) return;

            var point = surface.GetPoint(coordPoint);

            Gizmos.color = colorPoint;
            if (wiredPoint) Gizmos.DrawWireSphere(point, radiusPoint);
            else Gizmos.DrawSphere(point, radiusPoint);

            if (showLine) Gizmos.DrawLine(surface.position, point);
        }
    }
}