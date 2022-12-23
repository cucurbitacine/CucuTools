using System;
using CucuTools.DamageSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Example.Scripts.DamageSystem.Filters
{
    public class CriticalChanceBehaviour : DamageEffectBehaviour
    {
        public CriticalChanceEffect Crit;
        
        public override IDamageEffect GetEffect()
        {
            return Crit;
        }
    }
    
    [Serializable]
    public class CriticalChanceEffect : IDamageEffect
    {
        [Range(0f, 1f)]
        public float chance = 0.1f;

        [Min(1f)]
        public float crit = 1.5f;
        
        public DamageInfo EvaluateDamage(DamageInfo damage)
        {
            if (!damage.crit.isOn && Random.value < chance)
            {
                damage.crit.isOn = true;
                damage.crit.amount = Mathf.CeilToInt(damage.amount * (crit - 1));
                damage.amount += damage.crit.amount;
            }
            
            return damage;
        }
    }
}