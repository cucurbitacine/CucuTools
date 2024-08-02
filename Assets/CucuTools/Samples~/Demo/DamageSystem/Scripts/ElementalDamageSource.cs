using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts
{
    public class ElementalDamageSource : DamageSource
    {
        [Header("Elemental Settings")]
        [SerializeField] private ElementalType elemental;
        
        public override Damage CreateDamage(DamageReceiver receiver)
        {
            return new ElementalDamage(Damage.Generate(DamageAmount, CriticalChance, CriticalBonusPercentage))
            {
                elemental = elemental
            };
        }
    }
}