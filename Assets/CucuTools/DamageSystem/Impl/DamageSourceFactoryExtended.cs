using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    public class DamageSourceFactoryExtended : DamageSourceExtended
    {
        [Header("Factory Settings")]
        public DamageFactory factory;
        
        public override Damage CreateDamage()
        {
            return factory.CreateDamage();
        }
    }
}