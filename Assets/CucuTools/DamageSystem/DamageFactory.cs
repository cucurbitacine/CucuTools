using UnityEngine;

namespace CucuTools.DamageSystem
{
    public abstract class DamageFactory : ScriptableObject
    {
        public const string FactoryGroup = "Factory/";
        public const string CreateFactory = Cucu.CreateAsset + Cucu.DamageGroup + FactoryGroup;
        
        public abstract Damage CreateDamage();
    }
}