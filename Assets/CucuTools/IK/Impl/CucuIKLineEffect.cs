using UnityEngine;

namespace CucuTools.IK.Impl
{
    public class CucuIKLineEffect : CucuIKEffect
    {
        [Header("Damp")]
        public bool UseDamp = false;
        public float Damp = 8f;
        
        [Header("Line")]
        public LineRenderer Line;
        
        public override void OnSolved()
        {
            if (Line == null) return;

            if (Line.positionCount != Brain.PointCount)
            {
                Line.positionCount = Brain.PointCount;
            }

            for (var i = 0; i < Brain.PointCount; i++)
            {
                var point = Brain.GetPoint(i, Line.useWorldSpace);
                if (UseDamp) point = Vector3.Lerp(Line.GetPosition(i), point, Damp * Time.deltaTime);
                Line.SetPosition(i, point);
            }
        }
        
        private void OnValidate()
        {
            if (Brain != null) OnSolved();
        }
    }
}