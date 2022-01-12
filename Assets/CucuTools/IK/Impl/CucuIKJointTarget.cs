using UnityEngine;

namespace CucuTools.IK.Impl
{
    public class CucuIKJointTarget : CucuIKTarget
    {
        public override Vector3 Target
        {
            get => UseWorldSpace ? transform.position : transform.localPosition;
            set
            {
                if (UseWorldSpace) transform.position = value;
                else transform.localPosition = value;
            }
        }
    }
}