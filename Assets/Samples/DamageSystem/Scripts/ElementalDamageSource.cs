using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Impl;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    public class ElementalDamageSource : SimpleDamageSource
    {
        [Space]
        public Elemental elemental;
        public int damageBonus = 0;
        
        public override Damage CreateDamage()
        {
            var baseDmg = base.CreateDamage();

            var dmg = new ElementalDamage();
            
            dmg.elemental = elemental;
            dmg.amount = baseDmg.amount + damageBonus;
            dmg.critical = baseDmg.critical;
            
            return dmg;
        }
    }
}