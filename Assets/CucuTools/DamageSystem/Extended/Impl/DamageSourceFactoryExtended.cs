using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Impl
{
    public class DamageSourceFactoryExtended : DamageSourceExtended
    {
        [Space]
        public DamageFactory factory;
        
        public override Damage CreateDamage()
        {
            return factory.CreateDamage();
        }
    }
}