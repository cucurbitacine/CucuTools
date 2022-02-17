using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Impl;
using UnityEngine;
using UnityEngine.Events;

namespace Example.Scripts.DamageSystem
{
    public class HealthBehaviour : MonoBehaviour
    {
        public int Amount = 100;
        public int Maximum = 100;

        [Space]
        public UnityEvent<float> OnHealthChanged;
        public UnityEvent<DamageEvent> OnDamageApplied;
        public UnityEvent OnDied;
        
        public void ReceiveDamage(DamageEvent e)
        {
            if (Amount > 0)
            {
                ApplyDamage(e);
            }
        }

        public void ApplyDamage(DamageEvent e)
        {
            if (e.damage.amount == 0) return;
            
            SetHealth(Amount - e.damage.amount);
            
            OnDamageApplied.Invoke(e);
            
            DamageEventManager.WasApplied(e);
            
            if (Amount == 0) OnDied.Invoke();
        }

        public void SetHealth(int amount)
        {
            var newAmount = Mathf.Clamp(amount, 0, Maximum);
            if (Amount == newAmount) return;
            
            Amount = newAmount;
            OnHealthChanged.Invoke(Amount);
        }
    }
}