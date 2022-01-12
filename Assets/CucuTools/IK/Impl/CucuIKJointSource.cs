using System;
using System.Linq;
using UnityEngine;

namespace CucuTools.IK.Impl
{
    public class CucuIKJointSource : CucuIKSource
    {
        [Header("Joints")]
        public Transform[] Joints = default;
        
        public override Vector3[] GetPoints()
        {
            if (Joints == null) return Array.Empty<Vector3>();

            return Joints.Where(j => j != null).Distinct().Select(j => j.position).ToArray();
        }
    }
}