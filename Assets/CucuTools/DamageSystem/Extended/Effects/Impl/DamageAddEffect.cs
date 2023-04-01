using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects.Impl
{
    [CreateAssetMenu(menuName = CreateEffect + AssetName, fileName = AssetName, order = 0)]
    public class DamageAddEffect : DamageEffect
    {
        public const string AssetName = nameof(DamageAddEffect);

        public int addition = 0;
        
        public override void HandleDamage(DamageEvent e)
        {
            e.damage.amount += addition;
        }
    }
}