using System;
using CucuTools.Colors;
using UnityEngine;

namespace CucuTools.Surfaces
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SurfaceEntity))]
    public class SurfaceDrawer : CucuBehaviour
    {
        [SerializeField] private bool isDrawing = true;
        [SerializeField] private SurfaceEntity surface;
        [SerializeField] private DrawerConfig drawerConfig;
        
        public bool IsDrawing
        {
            get => isDrawing;
            set => isDrawing = value;
        }

        public SurfaceEntity Surface => surface != null ? surface : (surface = GetComponent<SurfaceEntity>());

        public DrawerConfig Config => drawerConfig != null ? drawerConfig : (drawerConfig = new DrawerConfig());

        private void OnDrawGizmos()
        {
            if (IsDrawing) Config.Draw(Surface);
        }
    }

    [Serializable]
    public class DrawerConfig
    {
        [Header("Surface")]
        public bool showSurface = true;
        [Range(4, 128)] public int resolution = 32;

        [Header("Grid")]
        public bool showGrid = true;
        [Range(0, 128)] public int countGridU = 7;
        [Range(0, 128)] public int countGridV = 7;

        [Header("Normal")]
        public bool showNormal;
        [Min(0)]
        public float normalHeight = 0.1f;

        [Header("Colors")]
        public Color color00 = new Color(0.45f, 0.35f, 0.65f);
        public Color color01 = new Color(0.00f, 0.70f, 0.70f);
        public Color color11 = new Color(1.00f, 0.90f, 0.15f);
        public Color color10 = new Color(0.95f, 0.35f, 0.25f);

        public float[] T => t != null && t.Length == resolution ? t : (t = Cucu.LinSpace(resolution));

        public float[] GridU
        {
            get
            {
                if (gridU != null && gridU.Length == countGridU) return gridU;
                gridU = new float[countGridU];
                for (int i = 0; i < countGridU; i++)
                {
                    gridU[i] = 1f / (countGridU + 1) + (float) (i) / (countGridU + 1);
                }

                return gridU;
            }
        }

        public float[] GridV
        {
            get
            {
                if (gridV != null && gridV.Length == countGridV) return gridV;
                gridV = new float[countGridV];
                for (int i = 0; i < countGridV; i++)
                {
                    gridV[i] = 1f / (countGridV + 1) + (float) (i) / (countGridV + 1);
                }

                return gridV;
            }
        }

        private float[] t;
        private float[] gridU;
        private float[] gridV;

        public void Draw(SurfaceEntity surface)
        {
            if (showSurface) DrawSurface(surface);

            if (showGrid) DrawGrid(surface);

            if (showNormal) DrawNormal(surface);
        }

        public void DrawSurface(SurfaceEntity surface)
        {
            var clrFrom = color00;
            var clrTo = color10;
            for (int i = 0; i < T.Length - 1; i++)
            {
                Gizmos.color = Color.Lerp(clrFrom, clrTo, T[i]);
                Gizmos.DrawLine(surface.GetPoint(T[i], 0), surface.GetPoint(T[i + 1], 0));
            }

            clrTo = color01;
            for (int i = 0; i < T.Length - 1; i++)
            {
                Gizmos.color = Color.Lerp(clrFrom, clrTo, T[i]);
                Gizmos.DrawLine(surface.GetPoint(0, T[i]), surface.GetPoint(0, T[i + 1]));
            }

            clrFrom = color01;
            clrTo = color11;
            for (int i = 0; i < T.Length - 1; i++)
            {
                Gizmos.color = Color.Lerp(clrFrom, clrTo, T[i]);
                Gizmos.DrawLine(surface.GetPoint(T[i], 1), surface.GetPoint(T[i + 1], 1));
            }

            clrFrom = color10;
            for (int i = 0; i < T.Length - 1; i++)
            {
                Gizmos.color = Color.Lerp(clrFrom, clrTo, T[i]);
                Gizmos.DrawLine(surface.GetPoint(1, T[i]), surface.GetPoint(1, T[i + 1]));
            }
        }

        public void DrawGrid(SurfaceEntity surface)
        {
            for (int i = 0; i < T.Length - 1; i++)
            {
                var point = Vector3.zero;

                for (int j = 0; j < GridU.Length; j++)
                {
                    var uv = new Vector2(GridU[j], T[i]);
                    var uv2 = new Vector2(GridU[j], T[i + 1]);

                    point = surface.GetPoint(uv);

                    Gizmos.color = GetUVColor(uv);
                    Gizmos.DrawLine(point, surface.GetPoint(uv2));

                    if (showNormal)
                    {
                        Gizmos.color = Gizmos.color.LerpTo(Color.cyan, 0.5f);
                        //Gizmos.DrawLine(point, point + surface.GetNormal(uv) * normalHeight);
                    }
                }

                for (int j = 0; j < GridV.Length; j++)
                {
                    var uv = new Vector2(T[i], GridV[j]);
                    var uv2 = new Vector2(T[i + 1], GridV[j]);

                    point = surface.GetPoint(uv);

                    Gizmos.color = GetUVColor(uv);
                    Gizmos.DrawLine(point, surface.GetPoint(uv2));

                    if (showNormal)
                    {
                        Gizmos.color = Gizmos.color.LerpTo(Color.cyan, 0.25f);
                        //Gizmos.DrawLine(point, point + surface.GetNormal(uv) * normalHeight);
                    }
                }
            }
        }

        public void DrawNormal(SurfaceEntity surface)
        {
            void _DrawNormal(Vector2 uv)
            {
                var point = surface.GetPoint(uv);
                var normal = surface.GetNormal(uv);

                Gizmos.color = GetUVColor(uv);
                Gizmos.DrawLine(point, point + normal * normalHeight);
                Gizmos.color = Gizmos.color.AlphaTo(0.2f);
                Gizmos.DrawSphere(point, normalHeight / 10);
            }

            var uv = Vector2.zero;
            for (int i = 0; i < GridU.Length; i++)
            {
                uv.x = GridU[i];
                for (int j = 0; j < GridV.Length; j++)
                {
                    uv.y = GridV[j];

                    _DrawNormal(uv);
                }
            }

            for (int i = 0; i < GridU.Length; i++)
            {
                uv.x = GridU[i];
                uv.y = 0;

                _DrawNormal(uv);

                uv.y = 1;

                _DrawNormal(uv);
            }

            for (int i = 0; i < GridV.Length; i++)
            {
                uv.y = GridV[i];
                uv.x = 0;

                _DrawNormal(uv);

                uv.x = 1;

                _DrawNormal(uv);
            }

            _DrawNormal(new Vector2(0, 0));
            _DrawNormal(new Vector2(0, 1));
            _DrawNormal(new Vector2(1, 1));
            _DrawNormal(new Vector2(1, 0));
        }

        public Color GetUVColor(Vector2 uv)
        {
            return Color.Lerp(
                Color.Lerp(
                    Color.Lerp(color00, color10, uv.x), Color.Lerp(color01, color11, uv.x), uv.y),
                Color.Lerp(
                    Color.Lerp(color00, color01, uv.y), Color.Lerp(color10, color11, uv.y), uv.x),
                0.5f);
        }
    }
}