using System;
using CucuTools.DamageSystem;

namespace Examples.DamageSystem
{
    public class ZombieDamageManager : DamageManager
    {
        public int health = 32;
        public int healthTotal = 32;
        
        public override void HandleDamageAsReceiver(DamageInfo info)
        {
            if (info.damage is FireDamage fire)
            {
                fire.amount *= 2;
            } 
        }

        private void Start()
        {
            health = healthTotal;
            
            onDamageReceived.AddListener(t =>
            {
                health -= t.damage.amount;

                if (health < 0) gameObject.SetActive(false);
            });
        }
    }
}