using System.Linq;
using CucuTools;
using UnityEngine;

namespace Samples.Demo.Extensions
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
                    var point = Bezier.Position(t[i],
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
}