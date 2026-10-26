using System;
using UnityEngine;

namespace CucuTools.NodeSystem
{
    [Serializable]
    public class Condition : IReady
    {
        [HideInInspector]
        [SerializeField] private string _displayName;
        [SerializeField] private ValueObject _valueObject;
        [SerializeField] private ValueOperation _operation;
        [SerializeField] private ValueData _valueTarget;
        [SerializeField] private ValueAsset _valueAsset;

        public bool IsReady(INode node)
        {
            var value = GetValue().GetData(node);
            var operation = GetOperation();
            var target = GetTarget();
            var type = GetValue().GetValueType();
            var compare = Condition.CompareTo(value, target, type);
            var result = Condition.Match(compare, operation);
            
            return result;
        }

        public ValueObject GetValue()
        {
            return _valueObject;
        }
        
        public ValueOperation GetOperation()
        {
            return _operation;
        }
        
        public ValueData GetTarget()
        {
            return _valueAsset != null ? _valueAsset.GetData() : _valueTarget;
        }

        public void OnValidate()
        {
            var valueObj = GetValue();
            
            var valueName = valueObj != null ? valueObj.GetValueName() : "[]";
            var valueOperator = GetString(GetOperation());
            var valueTarget = valueObj != null ? GetTarget().ToString(valueObj.GetValueType()) : "[]";

            _displayName = $"\"{valueName}\" {valueOperator} {valueTarget}";
        }
        
        public static int CompareTo(ValueData a, ValueData b, ValueType type)
        {
            switch (type)
            {
                case ValueType.Bool: return a.GetBool().CompareTo(b.GetBool());
                case ValueType.Int: return a.GetInt().CompareTo(b.GetInt());
                case ValueType.Float: return a.GetFloat().CompareTo(b.GetFloat());
            }

            throw new Exception(); // TODO
        }
        
        public static bool Match(int compare, ValueOperation operation)
        {
            switch (operation)
            {
                case ValueOperation.Equal: return compare == 0;
                case ValueOperation.Less: return compare < 0;
                case ValueOperation.LessOrEqual: return compare <= 0;
                case ValueOperation.MoreOrEqual: return compare >= 0;
                case ValueOperation.More: return compare > 0;
                case ValueOperation.NotEqual: return compare != 0;
                default: return false;
            }
        }

        public static string GetString(ValueOperation operation)
        {
            switch (operation)
            {
                case ValueOperation.Equal: return "=";
                case ValueOperation.Less: return "<";
                case ValueOperation.LessOrEqual: return "<=";
                case ValueOperation.MoreOrEqual: return ">=";
                case ValueOperation.More: return ">";
                case ValueOperation.NotEqual: return "!=";
                default: return "?";
            }
        }
    }
    
    public enum ValueOperation
    {
        [InspectorName("==")]
        Equal = 0,
        [InspectorName("<")]
        Less = 1,
        [InspectorName("<=")]
        LessOrEqual = 2,
        [InspectorName(">=")]
        MoreOrEqual = 3,
        [InspectorName(">")]
        More = 4,
        [InspectorName("!=")]
        NotEqual = 5,
    }
}