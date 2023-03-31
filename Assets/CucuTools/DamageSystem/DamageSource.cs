using UnityEngine;

namespace CucuTools.DamageSystem
{
    public abstract class DamageSource : CucuBehaviour
    {
        [Space] public bool mute = false;

        public DamageManager manager = null;
        
        public abstract Damage CreateDamage();

        public DamageEvent GenerateDamage(DamageReceiver receiver)
        {
            var e = new DamageEvent(CreateDamage(), this, receiver);

            HandleDamage(e);

            if (manager != null)
            {
                manager.HandleDamageAsSource(e);
            }

            return e;
        }

        public virtual void SendDamage(DamageReceiver receiver)
        {
            if (mute) return;
            
            receiver.ReceiveDamage(GenerateDamage(receiver));
        }

        protected virtual void HandleDamage(DamageEvent e)
        {
        }
    }
}