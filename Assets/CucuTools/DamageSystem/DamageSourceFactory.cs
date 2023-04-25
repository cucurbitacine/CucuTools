using UnityEngine;

namespace CucuTools.DamageSystem
{
    public class DamageSourceFactory : DamageSource
    {
        [Space]
        public DamageFactory factory;
        
        public override Damage CreateDamage()
        {
            return factory.CreateDamage();
        }
    }
}