using System;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts
{
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        [field: Min(0), SerializeField] public int CurrentHealth { get; private set; } = 100;
        [field: Min(0), SerializeField] public int MaxHealth { get; private set; } = 100;
        [field: Space, SerializeField] public bool IsDead { get; private set; }

        public event Action<int, int> OnHealthChanged;
        public event Action OnDied;

        public void SetHealth(int current, int max)
        {
            if (IsDead) return;

            var previousHealth = CurrentHealth;
            var previousHealthMax = MaxHealth;

            MaxHealth = Mathf.Max(1, max);
            CurrentHealth = Mathf.Clamp(current, 0, MaxHealth);

            if (CurrentHealth != previousHealth || MaxHealth != previousHealthMax)
            {
                OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

                if (CurrentHealth == 0)
                {
                    IsDead = true;

                    OnDied?.Invoke();
                }
            }
        }

        public void SetHealth(int current)
        {
            SetHealth(current, MaxHealth);
        }

        public void SetMaxHealth(int max)
        {
            SetHealth(CurrentHealth, max);
        }

        public void Damage(int amount)
        {
            if (amount > 0)
            {
                SetHealth(CurrentHealth - amount);
            }
        }

        public void Heal(int amount)
        {
            if (amount > 0)
            {
                SetHealth(CurrentHealth + amount);
            }
        }

        [ContextMenu(nameof(Die))]
        public void Die()
        {
            Damage(CurrentHealth);
        }
        
        public void Revive(int health)
        {
            IsDead = false;

            SetHealth(Mathf.Max(1, health));
        }
        
        [ContextMenu(nameof(Revive))]
        public void Revive()
        {
            Revive(1);
        }
    }
}