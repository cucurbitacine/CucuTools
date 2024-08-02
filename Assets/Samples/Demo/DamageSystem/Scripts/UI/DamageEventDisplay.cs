using CucuTools.DamageSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Samples.Demo.DamageSystem.Scripts.UI
{
    public class DamageEventDisplay : MonoBehaviour
    {
        [SerializeField] private float radiusSpread = 0.5f;

        [Space]
        [SerializeField] private DamageReceiver damageReceiver;
        [SerializeField] private PopupText popupTextPrefab;

        protected virtual void HandlePopupText(PopupText popupText, DamageEvent damageEvent)
        {
            var damageAmount = damageEvent.damage.amount;

            var damageText = damageAmount <= 0 ? $"+{-damageAmount}" : $"-{damageAmount}";
            
            popupText.SetText(damageText);
        }
        
        private void HandleDamageEvent(DamageEvent damageEvent)
        {
            if (!popupTextPrefab) return;
            
            var spawnPoint = GetSpawnPoint(damageEvent.receiver.transform) + Random.insideUnitCircle * radiusSpread;
            var popupText = Instantiate(popupTextPrefab, spawnPoint, Quaternion.identity);

            HandlePopupText(popupText, damageEvent);
        }

        private static Vector2 GetSpawnPoint(Transform target)
        {
            if (target.TryGetComponent<Collider2D>(out var cld2d))
            {
                return cld2d.bounds.center;
            }

            return target.position;
        }
        
        private void OnEnable()
        {
            if (damageReceiver)
            {
                damageReceiver.OnDamageReceived += HandleDamageEvent;
            }
        }

        private void OnDisable()
        {
            if (damageReceiver)
            {
                damageReceiver.OnDamageReceived -= HandleDamageEvent;
            }
        }

        private void OnDrawGizmosSelected()
        {
            var center = GetSpawnPoint(transform);
            Gizmos.DrawWireSphere(center, radiusSpread);
        }
    }
}