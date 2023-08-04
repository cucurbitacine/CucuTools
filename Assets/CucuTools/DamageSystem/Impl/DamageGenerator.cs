using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CucuTools.DamageSystem.Impl
{
    [Serializable]
    public class DamageGenerator
    {
        [Space]
        public int damageAmount = 1;
        
        [Space]
        [Min(0)]
        public float criticalRate = 0.2f;
        [Min(0)]
        public float criticalDamage = 1f;
        
        public void Generate(Damage dmg)
        {
            dmg.amount = damageAmount;

            dmg.critical = Random.Range(0f, 0.999f) < criticalRate;
            if (dmg.critical)
            {
                dmg.amount += (int)(dmg.amount * criticalDamage);
            }
        }
        
        public Damage Generate()
        {
            var dmg = new Damage();

            Generate(dmg);

            return dmg;
        }
    }
}