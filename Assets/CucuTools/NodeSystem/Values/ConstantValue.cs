using UnityEngine;

namespace CucuTools.NodeSystem.Values
{
    public sealed class ConstantValue : ValueObject
    {
        [Header("Constant")]
        [SerializeField] private ValueType _valueType;
        [SerializeField] private ValueData _valueData;

        public override ValueType GetValueType()
        {
            return _valueType;
        }

        public override ValueData GetData(INode node)
        {
            return _valueData;
        }
    }
}