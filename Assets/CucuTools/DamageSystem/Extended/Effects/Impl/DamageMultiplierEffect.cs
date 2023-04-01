using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects.Impl
{
    [CreateAssetMenu(menuName = CreateEffect + AssetName, fileName = AssetName, order = 0)]
    public class DamageMultiplierEffect : DamageEffect
    {
        public const string AssetName = nameof(DamageMultiplierEffect);

        [Min(0f)] public float factor = 1f;
        
        public override void HandleDamage(DamageEvent e)
        {
            e.damage.amount = (int)(e.damage.amount * factor);
        }
    }
}