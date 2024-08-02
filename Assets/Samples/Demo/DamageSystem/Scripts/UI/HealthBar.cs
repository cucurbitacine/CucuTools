using UnityEngine;
using UnityEngine.UI;

namespace Samples.Demo.DamageSystem.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Slider bar;

        private void HandleHealth(int current, int maximum)
        {
            if (bar)
            {
                bar.value = maximum > 0 ? (float)current / maximum : 0f;
            }
        }
        
        private void Awake()
        {
            if (health == null) health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            if (health)
            {
                health.OnHealthChanged += HandleHealth;
            }
        }

        private void OnDisable()
        {
            if (health)
            {
                health.OnHealthChanged -= HandleHealth;
            }
        }

        private void Start()
        {
            if (health)
            {
                HandleHealth(health.CurrentHealth, health.MaxHealth);
            }
        }
    }
}