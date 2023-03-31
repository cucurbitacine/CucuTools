using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    public class DamageManager : CucuBehaviour
    {
        [Space]
        public UnityEvent<DamageEvent> onDamageReceived = new UnityEvent<DamageEvent>();
        
        [Space]
        public List<DamageSource> sources = new List<DamageSource>(); 
        public List<DamageReceiver> receivers = new List<DamageReceiver>(); 

        public void ReceiveDamage(DamageEvent e)
        {
            onDamageReceived.Invoke(e);
        }
        
        public virtual void HandleDamageAsSource(DamageEvent e)
        {
        }
        
        public virtual void HandleDamageAsReceiver(DamageEvent e)
        {
        }
        
        protected virtual void Awake()
        {
            sources.ForEach(s => s.manager = this);
            receivers.ForEach(r => r.manager = this);
        }

        protected virtual void OnEnable()
        {
            foreach (var receiver in receivers)
            {
                receiver.onDamageReceived.AddListener(ReceiveDamage);
            }
        }
        
        protected virtual void OnDisable()
        {
            foreach (var receiver in receivers)
            {
                receiver.onDamageReceived.RemoveListener(ReceiveDamage);
            }
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