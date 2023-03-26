using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    public class SimpleDamageSource : DamageSource
    {
        [Space] public DamageTemplate template = new DamageTemplate();
        
        public override Damage CreateDamage()
        {
            return template.Create();
        }
    }
}
