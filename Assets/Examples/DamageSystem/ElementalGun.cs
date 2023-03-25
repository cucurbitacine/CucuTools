using CucuTools.DamageSystem;
using UnityEngine;

namespace Examples.DamageSystem
{
    public class ElementalGun : DamageSource
    {
        [Space]
        public int damageAmount = 1;
        
        [Space]
        [Range(0, 100)]
        public int criticalRate = 20;
        [Min(0)]
        public int criticalDamage = 100;

        public override Damage CreateDamage()
        {
            Damage dmg;
            
            switch (Random.Range(0, 5))
            {
                case 0: dmg = new FireDamage(); break;
                case 1: dmg = new WaterDamage(); break;
                case 2: dmg = new AirDamage(); break;
                case 3: dmg = new EarthDamage(); break;
                case 4: dmg = new LightningDamage(); break;
                default: dmg = new Damage(); break;
            }
            
            dmg.amount = damageAmount;
            
            dmg.critical = Random.Range(0, 99) < criticalRate;
            if (dmg.critical)
            {
                dmg.amount += (int)(dmg.amount * (criticalDamage / 100f));
            }
            
            return dmg;
        }
    }
}