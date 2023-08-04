using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    public class SimpleDamageSource : DamageSource
    {
        [Header("Damage Settings")]
        public DamageGenerator generator = new DamageGenerator();
        
        public override Damage CreateDamage()
        {
            return generator.Generate();
        }
    }
}
