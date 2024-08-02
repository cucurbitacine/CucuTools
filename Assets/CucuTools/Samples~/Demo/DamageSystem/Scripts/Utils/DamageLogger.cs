using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.Utils
{
    public class DamageLogger : MonoBehaviour
    {
        [SerializeField] private DamageReceiver receiver;

        private void HandleDamage(DamageEvent damageEvent)
        {
            var receiverName = damageEvent.receiver.Owner != damageEvent.receiver.gameObject
                ? $"\"{damageEvent.receiver.Owner.name}/{damageEvent.receiver.name}\" ({damageEvent.receiver.GetType().Name})"
                : $"\"{damageEvent.receiver.Owner.name}\" ({damageEvent.receiver.GetType().Name})";
            
            var sourceName = damageEvent.source.Owner != damageEvent.source.gameObject
                ? $"\"{damageEvent.source.Owner.name}/{damageEvent.source.name}\" ({damageEvent.source.GetType().Name})"
                : $"\"{damageEvent.source.Owner.name}\" ({damageEvent.source.GetType().Name})";

            var damageName = $"{damageEvent.damage} ({damageEvent.damage.GetType().Name})";
            
            Debug.Log($"{receiverName} got {damageName} from {sourceName}");
        }

        private void Awake()
        {
            if (receiver == null) receiver = GetComponent<DamageReceiver>();
        }

        private void OnEnable()
        {
            if (receiver)
            {
                receiver.OnDamageReceived += HandleDamage;
            }
        }
        
        private void OnDisable()
        {
            if (receiver)
            {
                receiver.OnDamageReceived -= HandleDamage;
            }
        }
    }
}