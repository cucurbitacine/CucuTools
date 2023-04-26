using System;
using CucuTools;
using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Samples.DamageSystem.Scripts
{
    public class HealthController : CucuBehaviour
    {
        [Min(0)] [SerializeField] private int _health = 50;
        [Min(1)] public int healthMax = 100;

        [Space] public HealthEvents events = new HealthEvents();
        
        public int health
        {
            get => _health;
            private set
            {
                var nextHealth = Mathf.Clamp(value, 0, healthMax);
                var diff = _health - nextHealth;
                if (diff == 0) return;
                
                _health = nextHealth;
                
                events.onHealthChanged.Invoke(_health);
                if (diff > 0) events.onHealed.Invoke(diff);
                if (diff < 0) events.onDamaged.Invoke(-diff);
                if (_health == 0) events.onDied.Invoke();
            }
        }

        public void Heal(int amount)
        {
            health += amount;
        }
        
        public void Damage(int amount)
        {
            health -= amount;
        }

        [DrawButton()]
        private void Restore()
        {
            health = healthMax;
        }

        [DrawButton()]
        private void Kill()
        {
            health = 0;
        }
    }
    
    [Serializable]
    public class HealthEvents
    {
        public UnityEvent<int> onHealthChanged = new UnityEvent<int>();
        public UnityEvent<int> onHealed = new UnityEvent<int>();
        public UnityEvent<int> onDamaged = new UnityEvent<int>();
        public UnityEvent onDied = new UnityEvent();
    }
}