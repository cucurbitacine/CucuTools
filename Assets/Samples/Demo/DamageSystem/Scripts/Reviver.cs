using System;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts
{
    public class Reviver : MonoBehaviour
    {
        [SerializeField] private bool revive = true;
        [SerializeField] [Range(0, 1)] private float reviveVolume = 0.75f;
        [SerializeField] private Health health;
        
        private void HandleDeath()
        {
            if (revive && health)
            {
                var volume = (int)(health.MaxHealth * reviveVolume);
                
                health.Revive(volume);
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
                health.OnDied += HandleDeath;
            }
        }

        private void OnDisable()
        {
            if (health)
            {
                health.OnDied -= HandleDeath;
            }
        }
    }
}