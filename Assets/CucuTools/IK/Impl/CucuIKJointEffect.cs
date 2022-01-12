using UnityEngine;

namespace CucuTools.IK.Impl
{
    public class CucuIKJointEffect : CucuIKEffect
    {
        [Header("Damp")]
        public bool UseDamp = false;
        public float Damp = 8f;
        
        [Header("Joints")]
        public Transform[] Joints = default;
        
        public override void OnSolved()
        {
            if (Joints == null) return;

            for (var i = 0; i < Brain.PointCount; i++)
            {
                if (Joints.Length <= i) break;
                if (Joints[i] == null) continue;
                var point = Brain.GetPoint(i);
                if (UseDamp) point = Vector3.Lerp(Joints[i].position, point, Damp * Time.deltaTime);
                Joints[i].position = point;
            }
        }
    }
}