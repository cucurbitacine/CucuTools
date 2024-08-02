using System;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    public class DamageSource : MonoBehaviour
    {
        [SerializeField] private GameObject owner;

        #region Public API
        
        [field: Space]
        [field: SerializeField] public int DamageAmount { get; set; } = 1;
        [field: Min(0f)]
        [field: SerializeField] public float CriticalChance { get; set; } = 0f;
        [field: Min(0f)]
        [field: SerializeField] public float CriticalBonusPercentage { get; set; } = 1f;
        
        public GameObject Owner => owner ? owner : (owner = gameObject);
        public event Action<GameObject> OnOwnerChanged;
        public event Action<DamageEvent> OnDamageDelivered;
        
        public void SetOwner(GameObject newOwner)
        {
            owner = newOwner;

            OnOwnerChanged?.Invoke(owner);
        }
        
        public DamageEvent SendDamage(Damage damage, DamageReceiver receiver)
        {
            var damageEvent = receiver.ReceiveDamage(damage, this);
            
            OnDamageDelivered?.Invoke(damageEvent);

            return damageEvent;
        }
        
        public virtual Damage CreateDamage(DamageReceiver receiver)
        {
            return Damage.Generate(DamageAmount, CriticalChance, CriticalBonusPercentage);
        }
        
        #endregion
    }
}