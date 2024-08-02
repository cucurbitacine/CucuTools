using System;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    public class DamageReceiver : MonoBehaviour
    {
        [SerializeField] private GameObject owner;

        #region Public API

        public GameObject Owner => owner ? owner : (owner = gameObject);
        public event Action<GameObject> OnOwnerChanged;
        public event Action<DamageEvent> OnDamageReceived;
        
        public void SetOwner(GameObject newOwner)
        {
            owner = newOwner;

            OnOwnerChanged?.Invoke(owner);
        }
        
        public DamageEvent ReceiveDamage(Damage damage, DamageSource source)
        {
            var damageEvent = new DamageEvent()
            {
                damage = damage,
                source = source,
                receiver = this,
            };

            ReceiveDamageEvent(damageEvent);

            return damageEvent;
        }

        public bool ReceiveDamageEvent(DamageEvent damageEvent)
        {
            if (damageEvent.receiver != this && damageEvent.receiver.Owner != Owner) return false;
            
            HandleDamageEvent(damageEvent);
            
            OnDamageReceived?.Invoke(damageEvent);

            return true;
        }
        
        public virtual void HandleDamageEvent(DamageEvent damageEvent)
        {
        }
        
        #endregion
    }
}
