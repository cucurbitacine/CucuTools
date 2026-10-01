using System;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(DamageAgent))]
    public sealed class Health : MonoBehaviour
    {
        [field: Min(0)]
        [field: SerializeField] public int Value { get; private set; } = 1;
        [field: Min(1)]
        [field: SerializeField] public int MaxValue { get; private set; } = 1;
        
        private DamageAgent owner;
        
        public event Action<int, int> MaxValueChanged;
        public event Action<int, int> ValueChanged;
        public event Action<int> Damaged;
        public event Action<int> Healed;

        public DamageAgent GetOwner()
        {
            return owner;
        }

        public void ChangeMaxValue(int newMaxValue)
        {
            var oldMaxValue = MaxValue;
            newMaxValue = Mathf.Max(1, newMaxValue);
            
            if (oldMaxValue == newMaxValue) return;
            MaxValue = newMaxValue;

            if (Value > MaxValue)
            {
                ChangeValue(MaxValue);
            }
            
            MaxValueChanged?.Invoke(oldMaxValue, newMaxValue);
        }
        
        public void ChangeValue(int newValue)
        {
            var oldValue = Value;
            newValue = Mathf.Clamp(newValue, 0, MaxValue);
            
            if (oldValue == newValue) return;
            Value = newValue;
                
            ValueChanged?.Invoke(oldValue, newValue);
        }
        
        public void Damage(int amount)
        {
            if (amount <= 0) return;

            var oldValue = Value;
            ChangeValue(Value - amount);
            var delta = oldValue - Value;

            if (delta > 0)
            {
                Damaged?.Invoke(amount);
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
                Healed?.Invoke(amount);
            }
        }
        
        private void OnDamageReceived(DamageEvent damageEvent)
        {
            Damage(damageEvent.Damage.Amount);
        }

        private void Awake()
        {
            owner = GetComponent<DamageAgent>();
        }

        private void OnEnable()
        {
            owner.DamageReceived += OnDamageReceived;
        }

        private void OnDisable()
        {
            owner.DamageReceived -= OnDamageReceived;
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