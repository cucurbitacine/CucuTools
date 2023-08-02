using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    public class DamageManager : CucuBehaviour
    {
        [Header("Events")]
        public UnityEvent<DamageEvent> onDamageReceived = new UnityEvent<DamageEvent>();
        public UnityEvent<DamageEvent> onDamageDelivered = new UnityEvent<DamageEvent>();
        
        public virtual void HandleDamageAsSource(DamageEvent e)
        {
        }
        
        public virtual void HandleDamageAsReceiver(DamageEvent e)
        {
        }
        
        public void DamageReceived(DamageEvent e)
        {
            onDamageReceived.Invoke(e);
        }
        
        public void DamageDelivered(DamageEvent e)
        {
            onDamageDelivered.Invoke(e);
        }
    }
    
    /*
     * *** SEQUENCE OF METHODS CALLS ***
     * 
     * DamageSource: void SendDamage(DamageReceiver receiver)
     *    DamageSource: DamageEvent GenerateDamage(DamageReceiver receiver)
     *       DamageSource : Damage CreateDamage()
     *       DamageSource : void HandleDamage(DamageEvent e)
     *       DamageManager: void HandleDamageAsSource(DamageEvent e)
     *
     *    DamageReceiver: void ReceiveDamage(DamageEvent e)
     *       DamageReceiver: void HandleDamage(DamageEvent e)
     *       DamageManager : void HandleDamageAsReceiver(DamageEvent e)
     *       DamageReceiver: onDamageReceived.Invoke(info)
     *
     * DamageManager: void ReceiveDamage(DamageEvent e)
     * DamageManager: onDamageReceived.Invoke(e)
     */
}