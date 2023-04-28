using CucuTools;
using CucuTools.DamageSystem;
using CucuTools.Others;
using UnityEngine;

namespace Samples.DamageSystem.Scripts.Enemy
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyDamageSource : ElementalDamageSource
    {
        [Header("Enemy Settings")]
        public LayerMask layerTargets = 1;

        private readonly CachedComponent<Collider, DamageReceiver> receivers = new();

        private Collider _cld = null;
        private Rigidbody _rigid = null;
        
        private void MessageSent(DamageEvent e)
        {
            Debug.Log($"{e.source.name} sent [{e.damage.amount}] damage to {e.receiver.name}");
        }
        
        private void MessageReceived(DamageEvent e)
        {
            Debug.Log($"{e.receiver.name} received [{e.damage.amount}] damage from {e.source.name}");
        }
        
        private void Awake()
        {
            _cld = GetComponent<Collider>();
            _cld.isTrigger = true;

            _rigid = GetComponent<Rigidbody>();
            _rigid.isKinematic = true;
            _rigid.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer.Contains(layerTargets))
            {
                if (receivers.TryGetValidValue(other, out var receiver))
                {
                    SendDamage(receiver, MessageSent, MessageReceived);
                }
            }
        }
    }
}