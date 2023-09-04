using System;
using CucuTools.DamageSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Samples.Demo.Scenes.DamageSystem.Scripts
{
    public class HealthController : MonoBehaviour
    {
        [Min(0)] public int value = 100;
        [Min(0)] public int total = 100;

        [Space]
        public UnityEvent<int, int> onHealthChanged = new UnityEvent<int, int>();
        public UnityEvent onDied = new UnityEvent();
        
        public void ReceiveDamage(DamageEvent e)
        {
            value -= e.damage.amount;
            
            if (value <= 0) onDied.Invoke();
            
            value = Mathf.FloorToInt(Mathf.Repeat(value, total));

            onHealthChanged.Invoke(value, total);
        }

        private void OnValidate()
        {
            value = Math.Clamp(value, 0, total);
        }
    }
}