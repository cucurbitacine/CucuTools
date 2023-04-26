using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Impl
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