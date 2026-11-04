using System;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    public sealed class Health : MonoBehaviour
    {
        [field: Min(0)]
        [field: SerializeField] public int Value { get; private set; } = 1;
        [field: Min(1)]
        [field: SerializeField] public int MaxValue { get; private set; } = 1;
        
        public event Action<int, int> maxValueChanged;
        public event Action<int, int> valueChanged;
        public event Action<int> damaged;
        public event Action<int> healed;
        
        public void ChangeMaxValue(int newMaxValue)
        {
            newMaxValue = Mathf.Max(1, newMaxValue);
            
            var oldMaxValue = MaxValue;
            if (oldMaxValue == newMaxValue) return;
            
            MaxValue = newMaxValue;

            if (Value > MaxValue)
            {
                ChangeValue(MaxValue);
            }
            
            maxValueChanged?.Invoke(oldMaxValue, newMaxValue);
        }
        
        public void ChangeValue(int newValue)
        {
            newValue = Mathf.Clamp(newValue, 0, MaxValue);
            
            var oldValue = Value;
            if (oldValue == newValue) return;
            
            Value = newValue;
                
            valueChanged?.Invoke(oldValue, newValue);
        }
        
        public void Damage(int amount)
        {
            if (amount <= 0) return;

            var oldValue = Value;
            
            ChangeValue(Value - amount);
            
            var delta = oldValue - Value;
            if (delta > 0)
            {
                damaged?.Invoke(amount);
            }
        }
        
        public void Heal(int amount)
        {
            if (amount <= 0) return;
            
            var oldValue = Value;
            
            ChangeValue(Value + amount);
            
            var delta = Value - oldValue;
            if (delta > 0)
            {
                healed?.Invoke(amount);
            }
        }

        private void OnValidate()
        {
            Value = Mathf.Clamp(Value, 0, MaxValue);
        }
    }

    public static class HealthExtension
    {
        public static void ChangeHealth(this Health health, int value, int maxValue)
        {
            health.ChangeMaxValue(maxValue);
            health.ChangeValue(value);
        }

        public static bool IsAlive(this Health health)
        {
            return health.Value > 0;
        }
        
        public static bool IsDead(this Health health)
        {
            return !health.IsAlive();
        }
        
        public static bool IsFull(this Health health)
        {
            return health.Value == health.MaxValue;
        }
        
        public static bool IsDamaged(this Health health)
        {
            return !health.IsFull();
        }
        
        public static void Kill(this Health health)
        {
            health.Damage(health.Value);
        }
        
        public static void Restore(this Health health)
        {
            health.Heal(health.MaxValue - health.Value);
        }
    }
}