using System;
using CucuTools.DamageSystem;
using UnityEngine;

namespace Example.Scripts.DamageSystem.Filters
{
    public class ShieldBehaviour : DamageFilterBehaviour
    {
        public ShieldProtection Shield;
        
        public override DamageFilter GetFilter()
        {
            return Shield;
        }
    }
    
    [Serializable]
    public class ShieldProtection : DamageFilter
    {
        [Min(0)]
        public int Amount = 1;
        public DamageType DamageType = DamageType.Physical;

        public override DamageInfo Compute(DamageInfo damage)
        {
            if (DamageType == damage.type) damage.amount = Mathf.Max(0, damage.amount - Amount);
            return damage;
        }
    }
}