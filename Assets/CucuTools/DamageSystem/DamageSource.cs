using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    public abstract class DamageSource : CucuBehaviour
    {
        public DamageManager manager = null;
        
        [Space]
        public UnityEvent<DamageEvent> onDamageDelivered = new UnityEvent<DamageEvent>();
        
        public abstract Damage CreateDamage();
        
        public void SendDamage(DamageReceiver receiver, Action<DamageEvent> eventCallback)
        {
            var dmg = CreateDamage();
            
            var e = new DamageEvent(dmg, this, receiver);

            HandleDamage(e);

            if (manager)
            {
                manager.HandleDamageAsSource(e);
            }

            receiver.ReceiveDamage(e, eventCallback);
            
            onDamageDelivered.Invoke(e);
            
            if (manager)
            {
                manager.DamageDelivered(e);
            }
        }
        
        public void SendDamage(DamageReceiver receiver)
        {
            SendDamage(receiver, null);
        }

        protected virtual void HandleDamage(DamageEvent e)
        {
        }
    }
}