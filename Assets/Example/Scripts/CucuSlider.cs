using CucuTools;
using UnityEngine;
using UnityEngine.Events;

namespace Example.Scripts
{
    public abstract class CucuSlider : CucuBehaviour
    {
        [SerializeField] private float value = 0f;
        
        [Space]
        [SerializeField] private float minValue = 0f;
        [SerializeField] private float maxValue = 1f;
        
        [Space]
        [SerializeField] private UnityEvent<float> onValueChanged = default;
        
        public float Value
        {
            get => value;
            set
            {
                this.value = value;
                
                OnValueChanged.Invoke(this.value);
            }
        }
        
        public float MinValue
        {
            get => minValue;
            set => minValue = Mathf.Min(value, MaxValue);
        }

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = Mathf.Max(MinValue, value);
        }
        
        public float Range => MaxValue - MinValue;
        
        public float Progress
        {
            get => (Value - MinValue) / Range;
            set => Value = MinValue + value * Range;
        }
        
        public UnityEvent<float> OnValueChanged => onValueChanged != null ? onValueChanged : (onValueChanged = new UnityEvent<float>());
        
        public abstract Vector3 HandlePosition { get; }

        public abstract Vector3 GetPointByProgress(float progress);
        public abstract Vector3 GetPointByValue(float value);
        
        public abstract float GetValueByPoint(Vector3 point, out Vector3 pointOnSlider);
        
        public virtual float GetValueByPoint(Vector3 point)
        {
            return GetValueByPoint(point, out _);
        }
    }
}