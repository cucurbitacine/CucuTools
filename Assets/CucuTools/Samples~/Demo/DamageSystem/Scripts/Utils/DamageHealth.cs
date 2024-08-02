using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.Utils
{
    [DisallowMultipleComponent]
    public class DamageHealth : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private DamageReceiver damageReceiver;

        private void HandleDamage(DamageEvent damageEvent)
        {
            if (health)
            {
                if (damageEvent.damage.amount > 0)
                {
                    health.Damage(damageEvent.damage.amount);
                }
                else if (damageEvent.damage.amount < 0)
                {
                    health.Heal(-damageEvent.damage.amount);
                }
            }
        }

        private void Awake()
        {
            if (health == null) health = GetComponent<Health>();
            if (damageReceiver == null) damageReceiver = GetComponent<DamageReceiver>();
        }

        private void OnEnable()
        {
            if (damageReceiver)
            {
                damageReceiver.OnDamageReceived += HandleDamage;
            }
        }

        private void OnDisable()
        {
            if (damageReceiver)
            {
                damageReceiver.OnDamageReceived -= HandleDamage;
            }
        }
    }
}