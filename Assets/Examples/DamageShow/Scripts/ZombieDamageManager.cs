using CucuTools.DamageSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Examples.DamageShow.Scripts
{
    public class ZombieDamageManager : DamageManager
    {
        public int health = 32;
        public int healthTotal = 32;

        [Space] public UnityEvent<ZombieDamageManager> onDied = new UnityEvent<ZombieDamageManager>();

        public override void HandleDamageAsReceiver(DamageEvent e)
        {
            if (e.damage is FireDamage fire)
            {
                fire.amount *= 2;
            }
        }

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