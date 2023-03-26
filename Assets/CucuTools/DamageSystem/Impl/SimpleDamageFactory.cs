using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    [CreateAssetMenu(menuName = Cucu.CreateAsset + Cucu.DamageGroup + AssetName, fileName = AssetName, order = 0)]
    public class SimpleDamageFactory : DamageFactory
    {
        public const string AssetName = nameof(SimpleDamageFactory);
        
        [Space] public DamageTemplate template = new DamageTemplate();
        
        public override Damage CreateDamage()
        {
            return template.Create();
        }
    }
}