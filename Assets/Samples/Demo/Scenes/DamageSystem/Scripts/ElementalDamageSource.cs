using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Impl;
using UnityEngine;

namespace Samples.Demo.Scenes.DamageSystem.Scripts
{
    public class ElementalDamageSource : DamageSource
    {
        [Space]
        public Elemental elemental;
        public int damageBonus = 0;
        public DamageGenerator generator;
        
        public override Damage CreateDamage()
        {
            var dmg = new ElementalDamage();

            generator.Generate(dmg);
            
            dmg.elemental = elemental;
            dmg.amount += damageBonus;
            
            return dmg;
        }
    }
}