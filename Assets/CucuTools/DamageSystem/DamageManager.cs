using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    public class DamageManager : CucuBehaviour
    {
        [Header("Events")]
        public UnityEvent<DamageEvent> onDamageSent = new UnityEvent<DamageEvent>();
        public UnityEvent<DamageEvent> onDamageReceived = new UnityEvent<DamageEvent>();

        [Header("Sources Settings")]
        public bool getChildrenSources = true;
        public DamageSource[] sources = Array.Empty<DamageSource>();
        
        [Header("Receivers Settings")]
        public bool getChildrenReceivers = true;
        public DamageReceiver[] receivers = Array.Empty<DamageReceiver>();
        
        public virtual void HandleDamageAsSource(DamageEvent e)
        {
        }
        
        public virtual void HandleDamageAsReceiver(DamageEvent e)
        {
        }

        public void Link(DamageSource source)
        {
            source.manager = this;
        }

        public void Unlink(DamageSource source)
        {
            source.manager = null;
        }

        public void Link(DamageReceiver receiver)
        {
            receiver.manager = this;
        }

        public void Unlink(DamageReceiver receiver)
        {
            receiver.manager = null;
        }

        public void Link(params DamageSource[] sources)
        {
            foreach (var source in sources)
            {
                if (source != null) Link(source);
            }
        }
        
        public void Link(params DamageReceiver[] receivers)
        {
            foreach (var receiver in receivers)
            {
                if (receiver != null) Link(receiver);
            }
        }
        
        public void Unlink(params DamageSource[] sources)
        {
            foreach (var source in sources)
            {
                if (source != null) Unlink(source);
            }
        }
        
        public void Unlink(params DamageReceiver[] receivers)
        {
            foreach (var receiver in receivers)
            {
                if (receiver != null) Unlink(receiver);
            }
        }
        
        public void SendDamage(DamageEvent e)
        {
            onDamageSent.Invoke(e);
        }
        
        public void ReceiveDamage(DamageEvent e)
        {
            onDamageReceived.Invoke(e);
        }
        
        protected virtual void Awake()
        {
            if (getChildrenSources)
            {
                sources = GetComponentsInChildren<DamageSource>();
            }

            if (getChildrenReceivers)
            {
                receivers = GetComponentsInChildren<DamageReceiver>();
            }
            
            Link(sources);
            Link(receivers);
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