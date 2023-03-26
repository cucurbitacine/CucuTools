using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    public class DamageReceiver : MonoBehaviour
    {
        [Space]
        public bool mute = false;

        public DamageManager manager = null;
        
        [Space]
        public UnityEvent<DamageEvent> onDamageReceived = new UnityEvent<DamageEvent>();

        public void ReceiveDamage(DamageEvent e)
        {
            if (mute) return;
            
            HandleDamage(e);
            
            if (manager != null)
            {
                manager.HandleDamageAsReceiver(e);
            }
            
            onDamageReceived.Invoke(e);
        }
        
        protected virtual void HandleDamage(DamageEvent e)
        {
        }
    }
}
