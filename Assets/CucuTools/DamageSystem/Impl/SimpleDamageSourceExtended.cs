using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    public class SimpleDamageSourceExtended : DamageSourceExtended
    {
        [Header("Template Settings")]
        public DamageTemplate template = new DamageTemplate();
        
        public override Damage CreateDamage()
        {
            return template.Create();
        }
    }
}