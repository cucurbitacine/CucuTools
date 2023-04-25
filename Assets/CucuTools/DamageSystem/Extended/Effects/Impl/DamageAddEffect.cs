using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects.Impl
{
    [CreateAssetMenu(menuName = CreateEffect + AssetName, fileName = AssetName, order = 0)]
    public class DamageAddEffect : DamageEffect
    {
        public const string AssetName = nameof(DamageAddEffect);

        public int addition = 0;
        
        [Space]
        public bool canBeZero = true;
        
        public override void HandleDamage(DamageEvent e)
        {
            e.damage.amount += addition;
            
            e.damage.amount = Mathf.Max(e.damage.amount, canBeZero ? 0 : 1);
        }
    }
}