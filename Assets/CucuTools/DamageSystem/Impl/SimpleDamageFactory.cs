using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    [CreateAssetMenu(menuName = CreateFactory + AssetName, fileName = AssetName, order = 0)]
    public class SimpleDamageFactory : DamageFactory
    {
        public const string AssetName = nameof(SimpleDamageFactory);
        
        [Header("Damage Settings")]
        public DamageGenerator generator = new DamageGenerator();
        
        public override Damage CreateDamage()
        {
            return generator.Generate();
        }
    }
}