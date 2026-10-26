using System;
using UnityEngine;

namespace CucuTools.NodeSystem
{
    public abstract class ValueObject : MonoBehaviour
    {
        [SerializeField] private string _valueName;

        private string _defaultValueName;

        public string GetValueName() => string.IsNullOrWhiteSpace(_valueName) ? GetDefaultValueName() : _valueName;
        public abstract ValueType GetValueType();
        public abstract ValueData GetData(INode node);

        private string GetDefaultValueName()
        {
            if (string.IsNullOrEmpty(_defaultValueName))
            {
                _defaultValueName = $"{GetType().Name} ({name})";
            }

            return _defaultValueName;
        }
    }
    
    public enum ValueType
    {
        Bool,
        Int,
        Float,
    }
    
    [Serializable]
    public struct ValueData
    {
        [SerializeField] private float _value;
        
        public float GetFloat() => _value;
        public int GetInt() => (int)GetFloat();
        public bool GetBool() => GetInt() != 0;

        public void SetFloat(float value) => _value = value;
        public void SetInt(int value) => SetFloat(value);
        public void SetBool(bool value) => SetInt(value ? 1 : 0);

        public ValueData(float value)
        {
            _value = value;
        }
        
        public ValueData(int value) : this((float)value)
        {
        }
        
        public ValueData(bool value) : this(value ? 1 : 0)
        {
        }

        public string ToString(ValueType type)
        {
            switch (type)
            {
                case ValueType.Bool: return GetBool().ToString();
                case ValueType.Int: return GetInt().ToString();
                case ValueType.Float: return $"{GetFloat():F}";
                default: return string.Empty;
            }
        }
    }
}