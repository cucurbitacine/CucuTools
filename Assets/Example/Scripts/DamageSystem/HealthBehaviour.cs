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
        public UnityEvent<DamageCommand> OnDamageApplied;
        public UnityEvent OnDied;
        
        public void ReceiveDamage(DamageCommand cmd)
        {
            if (Amount > 0)
            {
                ApplyDamage(cmd);
            }
        }

        public void ApplyDamage(DamageCommand cmd)
        {
            SetHealth(Amount - cmd.damage.amount);
            
            OnDamageApplied.Invoke(cmd);
            
            DamageEventManager.WasApplied(cmd);
            
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