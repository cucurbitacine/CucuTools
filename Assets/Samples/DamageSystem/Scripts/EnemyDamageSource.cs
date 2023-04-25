using CucuTools;
using CucuTools.DamageSystem;
using CucuTools.Others;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyDamageSource : ElementalDamageSource
    {
        public LayerMask layerTargets = 1;

        private readonly CachedDictionary<Collider, DamageReceiver> receivers =
            new(cld => cld.GetComponent<DamageReceiver>(),
                dr => dr != null);

        private Collider _cld = null;
        private Rigidbody _rigid = null;
        
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
                    SendDamage(receiver);
                }
            }
        }
    }
}