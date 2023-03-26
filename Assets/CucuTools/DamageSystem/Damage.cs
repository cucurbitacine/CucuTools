using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CucuTools.DamageSystem
{
    [Serializable]
    public class Damage
    {
        public int amount;
        public bool critical;
    }

    public class DamageEvent
    {
        public readonly Damage damage = null;
        public readonly DamageSource source = null;
        public readonly DamageReceiver receiver = null;
        
        public Vector3 point = Vector3.zero;
        public Vector3 normal = Vector3.zero;
        
        public DamageEvent(Damage dmg, DamageSource src, DamageReceiver rcv)
        {
            damage = dmg;
            source = src;
            receiver = rcv;
        }
    }
    
    [Serializable]
    public class DamageTemplate
    {
        [Space]
        public int damageAmount = 1;
        
        [Space]
        [Min(0)]
        public float criticalRate = 0.2f;
        [Min(0)]
        public float criticalDamage = 1f;
        
        public void Apply(Damage dmg)
        {
            dmg.amount = damageAmount;

            dmg.critical = Random.Range(0f, 0.999f) < criticalRate;
            if (dmg.critical)
            {
                dmg.amount += (int)(dmg.amount * criticalDamage);
            }
        }
        
        public Damage Create()
        {
            var dmg = new Damage();

            Apply(dmg);

            return dmg;
        }
    }
}