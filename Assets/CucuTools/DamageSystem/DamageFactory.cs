using UnityEngine;

namespace CucuTools.DamageSystem
{
    /// <summary>
    /// Factory of damage
    /// </summary>
    public abstract class DamageFactory : ScriptableObject
    {
        public const string FactoryGroup = "Factory/";
        public const string CreateFactory = Cucu.CreateAsset + Cucu.DamageGroup + FactoryGroup;
        
        /// <summary>
        /// Create Damage
        /// </summary>
        /// <returns></returns>
        public abstract Damage CreateDamage();
    }
}