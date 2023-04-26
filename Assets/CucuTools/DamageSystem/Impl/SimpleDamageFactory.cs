using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    [CreateAssetMenu(menuName = CreateFactory + AssetName, fileName = AssetName, order = 0)]
    public class SimpleDamageFactory : DamageFactory
    {
        public const string AssetName = nameof(SimpleDamageFactory);
        
        [Header("Template Settings")]
        public DamageTemplate template = new DamageTemplate();
        
        public override Damage CreateDamage()
        {
            return template.Create();
        }
    }
}