using UnityEngine;

namespace CucuTools.NodeSystem.Values
{
    public sealed class TimeInState : ValueObject
    {
        [SerializeField] private float _timeInState;

        private ValueData _valueData = new ValueData(0f);

        public override ValueType GetValueType()
        {
            return ValueType.Float;
        }

        public override ValueData GetData(INode node)
        {
            _timeInState = node.GetRunningTime();
            
            _valueData.SetFloat(_timeInState);
            return _valueData;
        }
    }
}