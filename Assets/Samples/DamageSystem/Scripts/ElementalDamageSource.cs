
using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Extended.Impl;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    public class ElementalDamageSource : SimpleDamageSourceExtended
    {
        [Header("Elemental Damage Settings")]
        public Elemental elemental = Elemental.Fire;
        
        public override Damage CreateDamage()
        {
            return new ElementalDamage(base.CreateDamage(), elemental);
        }
    }
}