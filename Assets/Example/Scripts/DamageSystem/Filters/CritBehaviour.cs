using System;
using CucuTools.DamageSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Example.Scripts.DamageSystem.Filters
{
    public class CritBehaviour : DamageFilterBehaviour
    {
        public CriticalChance Crit;
        
        public override DamageFilter GetFilter()
        {
            return Crit;
        }
    }
    
    [Serializable]
    public class CriticalChance : DamageFilter
    {
        [Range(0f, 1f)]
        public float chance = 0.1f;

        [Min(1f)]
        public float crit = 1.5f;
        
        public override DamageInfo Compute(DamageInfo damage)
        {
            if (!damage.isCritical && Random.value < chance)
            {
                damage.isCritical = true;
                damage.amount = Mathf.CeilToInt(damage.amount * crit);
            }
            
            return damage;
        }
    }
}