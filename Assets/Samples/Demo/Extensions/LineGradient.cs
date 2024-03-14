using CucuTools;
using UnityEngine;

namespace Samples.Demo.Extensions
{
    public class LineGradient : MonoBehaviour
    {
        public Gradient gradient;

        [Space] [Range(2, 128)] public int resolution = 32;
        public LineRenderer line;

        public void SetGradient(Gradient newGradient)
        {
            gradient = newGradient;

            UpdateLine();
        }

        public void UpdateLine()
        {
            if (line)
            {
                var t = CucuMath.LinSpace(resolution);

                var begin = line.GetPosition(0);
                var end = line.GetPosition(line.positionCount - 1);

                line.positionCount = t.Length;
                for (var i = 0; i < t.Length; i++)
                {
                    line.SetPosition(i, Vector3.Lerp(begin, end, t[i]));
                }

                line.colorGradient = gradient;
            }
        }

        private void OnValidate()
        {
            UpdateLine();
        }
    }
}