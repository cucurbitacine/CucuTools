using UnityEngine;

namespace CucuTools.DamageSystem.Utils
{
    [DisallowMultipleComponent]
    public sealed class DamageBridge : MonoBehaviour
    {
        private DamageReceiver receiver;
        private DamageReceiver destination;
        
        private void HandleReceiverOwner(GameObject owner)
        {
            destination = owner.GetComponent<DamageReceiver>();
        }
        
        private void HandleDamageEvent(DamageEvent damageEvent)
        {
            if (destination && destination != receiver)
            {
                destination.ReceiveDamageEvent(damageEvent);
            }
        }

        private void Awake()
        {
            receiver = GetComponent<DamageReceiver>();
        }

        private void OnEnable()
        {
            if (receiver)
            {
                receiver.OnOwnerChanged += HandleReceiverOwner;
                receiver.OnDamageReceived += HandleDamageEvent;

                HandleReceiverOwner(receiver.Owner);
            }
        }

        private void OnDisable()
        {
            if (receiver)
            {
                receiver.OnOwnerChanged -= HandleReceiverOwner;
                receiver.OnDamageReceived -= HandleDamageEvent;
            }
        }
    }
}