using UnityEngine;

namespace CucuTools.DamageSystem
{
    public abstract class DamageSource : MonoBehaviour
    {
        [Space] public bool mute = false;

        public DamageManager manager = null;
        
        public abstract Damage CreateDamage();

        public DamageInfo GenerateDamage(DamageReceiver receiver)
        {
            var info = new DamageInfo(CreateDamage(), this, receiver);

            HandleDamage(info);

            if (manager != null)
            {
                manager.HandleDamageAsSource(info);
            }

            return info;
        }

        public virtual void SendDamage(DamageReceiver receiver)
        {
            if (mute) return;
            
            receiver.ReceiveDamage(GenerateDamage(receiver));
        }

        protected virtual void HandleDamage(DamageInfo info)
        {
        }
    }
}