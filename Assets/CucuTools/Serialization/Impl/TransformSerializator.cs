using System;
using UnityEngine;

namespace CucuTools.Serialization.Impl
{
    [CreateAssetMenu(menuName = Cucu.AddComponent + Cucu.SerializationGroup + Serializators + ObjectName, fileName = ObjectName, order = 0)]
    public class TransformSerializator : ComponentSerializator<Transform, TransformSerializator.TransformData>
    {
        public const string ObjectName = nameof(TransformSerializator);
        
        public override TransformData GetData(Transform component)
        {
            var data = new TransformData();

            data.position = component.position;
            data.rotation = component.rotation;
            data.localScale = component.localScale;

            return data;
        }

        public override void SetData(TransformData data, ref Transform component)
        {
            component.position = data.position;
            component.rotation = data.rotation;
            component.localScale = data.localScale;
        }
        
        [Serializable]
        public class TransformData
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 localScale;
        }
    }
}