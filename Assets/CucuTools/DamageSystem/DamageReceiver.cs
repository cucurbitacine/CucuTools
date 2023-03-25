using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    public class DamageReceiver : MonoBehaviour
    {
        [Space]
        public bool isEnabled = true;

        [Space]
        public UnityEvent<DamageInfo> onDamageReceived = new UnityEvent<DamageInfo>();

        [Space]
        public DamageManager manager = null;

        public void ReceiveDamage(DamageInfo info)
        {
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
