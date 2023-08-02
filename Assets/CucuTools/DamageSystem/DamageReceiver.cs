using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    public class DamageReceiver : CucuBehaviour
    {
        public DamageManager manager = null;
        
        [Space]
        public UnityEvent<DamageEvent> onDamageReceived = new UnityEvent<DamageEvent>();

        public void ReceiveDamage(DamageEvent e, Action<DamageEvent> eventCallback)
        {
            HandleDamage(e);
            
            if (manager)
            {
                manager.HandleDamageAsReceiver(e);
            }
            
            onDamageReceived.Invoke(e);
            
            if (manager)
            {
                manager.DamageReceived(e);
            }
            
            eventCallback?.Invoke(e);

            Damage.Event?.Invoke(e);
        }
        
        public void ReceiveDamage(DamageEvent e)
        {
            ReceiveDamage(e, null);
        }
        
        protected virtual void HandleDamage(DamageEvent e)
        {
        }
    }
}
