using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Extended;
using UnityEngine;
using UnityEngine.Events;

namespace Examples.DamageShow.Scripts
{
    public class ZombieDamageManager : DamageManagerExtended
    {
        [Space]
        public int health = 32;
        public int healthTotal = 32;

        [Space] public UnityEvent<ZombieDamageManager> onDied = new UnityEvent<ZombieDamageManager>();
        
        private void HandleHealth(DamageEvent info)
        {
            health -= info.damage.amount;

            if (health < 0)
            {
                gameObject.SetActive(false);
                onDied.Invoke(this);
            }
        }
        
        private void Start()
        {
            health = healthTotal;

            onDamageReceived.AddListener(HandleHealth);
        }
    }
}