
using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Impl;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    public class ElementalDamageSource : SimpleDamageSource
    {
        [Header("Elemental Damage Settings")]
        public Elemental elemental = Elemental.Fire;
        
        public override Damage CreateDamage()
        {
            return new ElementalDamage(base.CreateDamage(), elemental);
        }
    }
}