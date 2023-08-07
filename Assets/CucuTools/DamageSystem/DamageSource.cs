using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    /// <summary>
    /// Damage Source.
    /// An entity that represents an object which able to create and send a damage
    /// <seealso cref="DamageManager"/>
    /// <seealso cref="DamageReceiver"/>
    /// <seealso cref="DamageEvent"/>
    /// <seealso cref="Damage"/>
    /// </summary>
    public abstract class DamageSource : CucuBehaviour
    {
        public DamageManager manager = null;
        
        [Space]
        public UnityEvent<DamageEvent> onDamageDelivered = new UnityEvent<DamageEvent>();
        
        /// <summary>
        /// Create Base Damage
        /// </summary>
        /// <returns></returns>
        public abstract Damage CreateDamage();
        
        /// <summary>
        /// Create and Send damage
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="eventCallback"></param>
        public void SendDamage(DamageReceiver receiver, Action<DamageEvent> eventCallback)
        {
            var dmg = CreateDamage();
            
            var e = new DamageEvent(dmg, this, receiver);

            HandleDamage(e);

            if (manager)
            {
                manager.HandleDamageAsSource(e);
            }
            
            receiver.ReceiveDamage(e, eventCallback);
            
            onDamageDelivered.Invoke(e);
            
            if (manager)
            {
                manager.onDamageDelivered.Invoke(e);
            }
        }
        
        /// <summary>
        /// Create and Send damage
        /// </summary>
        /// <param name="receiver"></param>
        public void SendDamage(DamageReceiver receiver)
        {
            SendDamage(receiver, null);
        }

        /// <summary>
        /// Handle to sent damage
        /// </summary>
        /// <param name="e"></param>
        protected virtual void HandleDamage(DamageEvent e)
        {
        }
    }
}