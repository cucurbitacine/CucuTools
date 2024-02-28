using CucuTools;
using CucuTools.Attributes;
using UnityEngine;

namespace Samples.Demo.Extensions
{
    public class BezierLine : CucuBehaviour
    {
        [Header("Color")] public ColorPaletteType paletteType;
        [Space] public Gradient gradient;
        [Range(0, 1)] public float value;
        public Color color;
        [Min(0.1f)]
        public float step = 0.1f;
        
        [Header("References")]
        public BezierPoint point;
        public LineRenderer line;

        [DrawButton]
        public void SwitchAuto()
        {
            if (point)
            {
                var points = point.GetPoints();
                foreach (var p in points)
                {
                    p.bezier.autoWeight = !p.bezier.autoWeight;
                    p.bezier.autoTangent = !p.bezier.autoTangent;
                }
            }
        }
        
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

        public void UpdateLine(int resolution)
        {
            if (line && point)
            {
                line.positionCount = resolution;

                var t = CucuMath.LinSpace(0, 1, resolution);

                for (var i = 0; i < t.Length; i++)
                {
                    var position = point.bezier.Progress(t[i]);

                    line.SetPosition(i, position);
                }

                line.colorGradient = gradient;
            }
        }

        private int Resolution()
        {
            if (point)
            {
                var length = point.bezier.GetLengthFull();

                return Mathf.Max((int)(length / step), 2);
            }

            return 2;
        }

        private void UpdateLine()
        {
            UpdateLine(Resolution());
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