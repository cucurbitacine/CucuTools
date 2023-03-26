using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    public class DamageManager : MonoBehaviour
    {
        [Space]
        public UnityEvent<DamageInfo> onDamageReceived = new UnityEvent<DamageInfo>();
        
        [Space]
        public List<DamageSource> sources = new List<DamageSource>(); 
        public List<DamageReceiver> receivers = new List<DamageReceiver>(); 

        public void ReceiveDamage(DamageInfo info)
        {
            onDamageReceived.Invoke(info);
        }
        
        public virtual void HandleDamageAsSource(DamageInfo info)
        {
        }
        
        public virtual void HandleDamageAsReceiver(DamageInfo info)
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
     *    DamageSource: DamageInfo GenerateDamage(DamageReceiver receiver)
     *       DamageSource : Damage CreateDamage()
     *       DamageSource : void HandleDamage(DamageInfo info)
     *       DamageManager: void HandleDamageAsSource(DamageInfo info)
     *
     *    DamageReceiver: void ReceiveDamage(DamageInfo info)
     *       DamageReceiver: void HandleDamage(DamageInfo info)
     *       DamageManager : void HandleDamageAsReceiver(DamageInfo info)
     *       DamageReceiver: onDamageReceived.Invoke(info)
     *
     * DamageManager: void ReceiveDamage(DamageInfo info)
     * DamageManager: void HandleDamageAsReceiver(DamageInfo info)
     */
}