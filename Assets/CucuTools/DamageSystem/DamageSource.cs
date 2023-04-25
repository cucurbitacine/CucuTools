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

        public void SendDamage(DamageReceiver receiver)
        {
            if (mute) return;

            var damage = GenerateDamage(receiver);
            
            onDamageSent.Invoke(damage);
            
            receiver.ReceiveDamage(damage);
        }

        private DamageEvent GenerateDamage(DamageReceiver receiver)
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