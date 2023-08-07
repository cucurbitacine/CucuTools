using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    /// <summary>
    /// Damage Manager.
    /// An entity that represents damage sources and receivers
    /// <seealso cref="DamageSource"/>
    /// <seealso cref="DamageReceiver"/>
    /// <seealso cref="DamageEvent"/>
    /// <seealso cref="Damage"/>
    /// </summary>
    [DisallowMultipleComponent]
    public class DamageManager : CucuBehaviour
    {
        [Header("Events")]
        public UnityEvent<DamageEvent> onDamageReceived = new UnityEvent<DamageEvent>();
        public UnityEvent<DamageEvent> onDamageDelivered = new UnityEvent<DamageEvent>();
        
        /// <summary>
        /// Handle Damage as if Manager was Receiver
        /// </summary>
        /// <param name="e"></param>
        public virtual void HandleDamageAsReceiver(DamageEvent e)
        {
        }
        
        /// <summary>
        /// Handle Damage as if Manager was Source
        /// </summary>
        /// <param name="e"></param>
        public virtual void HandleDamageAsSource(DamageEvent e)
        {
        }
    }
    
    /*
     * *** SEQUENCE OF METHODS CALLS ***
     * 
     * DamageSource     : void SendDamage(DamageReceiver receiver, Action<DamageEvent> eventCallback)
     *    DamageSource  : Damage CreateDamage(DamageReceiver receiver)
     *    DamageSource  : void HandleDamage(DamageEvent e)
     *    DamageManager : void HandleDamageAsSource(DamageEvent e)
     *    DamageReceiver    : void ReceiveDamage(DamageEvent e, Action<DamageEvent> eventCallback)
     *       DamageReceiver : void HandleDamage(DamageEvent e)
     *       DamageManager  : void HandleDamageAsReceiver(DamageEvent e)
     *       DamageReceiver : onDamageReceived.Invoke(DamageEvent e)
     *       DamageManager  : onDamageReceived.Invoke(DamageEvent e)
     *       DamageReceiver : eventCallback.Invoke(DamageEvent e)
     *       Damage         : Event.Invoke(DamageEvent e)
     *    DamageSource : onDamageDelivered.Invoke(DamageEvent e)
     *    DamageManager: onDamageDelivered.Invoke(DamageEvent e)
     */
}