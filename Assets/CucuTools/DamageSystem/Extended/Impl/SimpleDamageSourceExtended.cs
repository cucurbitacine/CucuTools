using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Impl
{
    public class SimpleDamageSourceExtended : DamageSourceExtended
    {
        [Space] public DamageTemplate template = new DamageTemplate();
        
        public override Damage CreateDamage()
        {
            return template.Create();
        }
    }
}