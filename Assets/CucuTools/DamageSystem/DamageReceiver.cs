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
        public UnityEvent<DamageInfo> onDamageReceived = new UnityEvent<DamageInfo>();

        public void ReceiveDamage(DamageInfo info)
        {
            if (mute) return;
            
            HandleDamage(info);
            
            if (manager != null)
            {
                manager.HandleDamageAsReceiver(info);
            }
            
            onDamageReceived.Invoke(info);
        }
        
        protected virtual void HandleDamage(DamageInfo info)
        {
        }
    }
}
