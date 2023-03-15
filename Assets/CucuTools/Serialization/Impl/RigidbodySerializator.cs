using System;
using UnityEngine;

namespace CucuTools.Serialization.Impl
{
    [CreateAssetMenu(menuName = "Create RigidbodySerializator", fileName = "RigidbodySerializator", order = 0)]
    public class RigidbodySerializator : ComponentSerializator<Rigidbody, RigidbodySerializator.RigidbodyData>
    {
        [Serializable]
        public class RigidbodyData
        {
            public Vector3 velocity;
            public Vector3 angularVelocity;
        }

        public override RigidbodyData GetData(Rigidbody component)
        {
            var data = new RigidbodyData();
            
            data.velocity = component.velocity;
            data.angularVelocity = component.angularVelocity;

            return data;
        }

        public override void SetData(RigidbodyData data, ref Rigidbody component)
        {
            component.velocity = data.velocity;
            component.angularVelocity = data.angularVelocity;
        }
    }
}