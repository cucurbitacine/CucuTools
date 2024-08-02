using System;
using Random = UnityEngine.Random;

namespace CucuTools.DamageSystem
{
    /// <summary>
    /// Damage information. Use it or inherit it
    /// </summary>
    [Serializable]
    public class Damage
    {
        public int amount;
        public bool critical;

        public static Damage Generate(int damageAmount, float criticalChance, float criticalBonusPercentage)
        {
            var critical = 0f < criticalChance && Random.value <= criticalChance;
            var criticalDamage = critical ? (int)(criticalBonusPercentage * damageAmount) : 0;

            return new Damage()
            {
                critical = critical,
                amount = damageAmount + criticalDamage,
            };
        } 
        
        public override string ToString()
        {
            return $"{amount}{(critical?" CRITICAL":"")}";
        }
    }

    [Serializable]
    public class DamageEvent
    {
        public Damage damage;
        public DamageSource source;
        public DamageReceiver receiver;
    }
}