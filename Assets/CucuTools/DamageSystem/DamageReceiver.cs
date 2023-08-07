using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    /// <summary>
    /// Damage Receiver.
    /// An entity that represents an object which able to receive a damage
    /// <seealso cref="DamageManager"/>
    /// <seealso cref="DamageEvent"/>
    /// </summary>
    public class DamageReceiver : CucuBehaviour
    {
        public DamageManager manager = null;
        
        [Space]
        public UnityEvent<DamageEvent> onDamageReceived = new UnityEvent<DamageEvent>();

        /// <summary>
        /// Receive and Handle to damage
        /// </summary>
        /// <param name="e"></param>
        /// <param name="eventCallback"></param>
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
                manager.onDamageReceived.Invoke(e);
            }
            
            eventCallback?.Invoke(e);

            Damage.Event?.Invoke(e);
        }
        
        /// <summary>
        /// Receive and Handle to damage
        /// </summary>
        /// <param name="e"></param>
        public void ReceiveDamage(DamageEvent e)
        {
            ReceiveDamage(e, null);
        }

        /// <summary>
        /// Handle to received damage
        /// </summary>
        /// <param name="e"></param>
        protected virtual void HandleDamage(DamageEvent e)
        {
        }
    }
}
