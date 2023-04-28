using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    public abstract class DamageSource : CucuBehaviour
    {
        [Space] public bool mute = false;

        public DamageManager manager = null;
        
        [Space]
        public UnityEvent<DamageEvent> onDamageSent = new UnityEvent<DamageEvent>();
        
        public abstract Damage CreateDamage();
        
        public void SendDamage(DamageReceiver receiver, Action<DamageEvent> onSent, Action<DamageEvent> onReceived)
        {
            if (mute) return;

            var e = GenerateDamageEvent(receiver);
            
            onSent?.Invoke(e);
            
            onDamageSent.Invoke(e);
            
            if (manager != null)
            {
                manager.SendDamage(e);
            }
            
            receiver.ReceiveDamage(e);
            
            onReceived?.Invoke(e);
        }
        
        public void SendDamage(DamageReceiver receiver)
        {
            SendDamage(receiver, null, null);
        }

        private DamageEvent GenerateDamageEvent(DamageReceiver receiver)
        {
            var e = new DamageEvent(CreateDamage(), this, receiver);

            HandleDamage(e);

            if (manager != null)
            {
                manager.HandleDamageAsSource(e);
            }

            return e;
        }
        
        protected virtual void HandleDamage(DamageEvent e)
        {
        }
    }
}