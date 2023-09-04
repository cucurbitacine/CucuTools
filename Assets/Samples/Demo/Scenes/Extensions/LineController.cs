using System;
using CucuTools;
using UnityEngine;

namespace Samples.Demo.Scenes.Extensions
{
    public class LineController : MonoBehaviour
    {
        public Transform begin;
        public Transform end;

        [Space] public LineRenderer line;

        public void UpdateLine()
        {
            if (line)
            {
                line.positionCount = 2;
                line.useWorldSpace = true;
                
                if (begin)
                {
                    line.SetPosition(0, begin.position);
                }

                if (end)
                {
                    line.SetPosition(1, end.position);
                }
            }
        }

        private void Update()
        {
            UpdateLine();
        }

        private void OnValidate()
        {
            UpdateLine();
        }
    }
}