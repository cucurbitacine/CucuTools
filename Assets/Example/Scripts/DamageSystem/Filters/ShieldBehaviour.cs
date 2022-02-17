using System;
using CucuTools.DamageSystem;
using UnityEngine;

namespace Example.Scripts.DamageSystem.Filters
{
    public class ShieldBehaviour : DamageEffectBehaviour
    {
        public ShieldEffect Shield;
        
        public override IDamageEffect GetEffect()
        {
            return Shield;
        }
    }
    
    [Serializable]
    public class ShieldEffect : DamageEffect
    {
        [Min(0)]
        public int Amount = 1;
        public DamageType DamageType = DamageType.Physical;

        public override DamageInfo Evaluate(DamageInfo damage)
        {
            if (DamageType == damage.type) damage.amount = Mathf.Max(0, damage.amount - Amount);
            return damage;
        }
    }
}