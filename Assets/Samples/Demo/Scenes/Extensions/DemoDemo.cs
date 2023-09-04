using System.Linq;
using CucuTools;
using UnityEditor;
using UnityEngine;

namespace Samples.Demo.Scenes.Extensions
{
    public class DemoDemo : CucuBehaviour
    {
        [Header("Color")] public ColorPaletteType paletteType;
        [Space] public Gradient gradient;
        [Range(0, 1)] public float value;
        public Color color;

        [Header("Math")] [Min(4)] public int resolution = 32;
        public LineRenderer line;
        [Space] public Transform[] points;

        public void UpdateColor()
        {
            color = gradient.Evaluate(value);
        }

        public void UpdateGradient()
        {
            var palette = CucuColor.Palettes[paletteType];

            gradient = palette.CreateGradient();

            UpdateColor();
        }

        public void UpdateLine()
        {
            if (points.Length != 4 || points.Any(p => p == null)) return;

            if (line)
            {
                line.positionCount = resolution;

                var t = CucuMath.LinSpace(0, 1, resolution);

                for (var i = 0; i < t.Length; i++)
                {
                    var point = CucuMath.BezierPosition(t[i],
                        points[0].position, points[1].position, points[2].position, points[3].position);

                    line.SetPosition(i, point);
                }

                line.colorGradient = gradient;
            }
        }

        private void Update()
        {
            UpdateGradient();

            UpdateLine();
        }

        private void OnValidate()
        {
            UpdateGradient();

            UpdateLine();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DemoDemo))]
    public class DemoDemoEditor : Editor
    {
        public float[] values;

        public void OnSceneGUI()
        {
            if (values == null) values = new float[] { 1, 1, 1, 1 };

            if (target is DemoDemo demo)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i < demo.points.Length && demo.points[i] != null)
                    {
                        var point = demo.points[i];

                        point.position = Handles.PositionHandle(point.position, Quaternion.identity);
                    }
                }

                Handles.DrawDottedLine(demo.points[0].position, demo.points[1].position, 1f);
                Handles.DrawDottedLine(demo.points[2].position, demo.points[3].position, 1f);

                demo.UpdateLine();
            }
        }
    }
#endif
}