using System;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects.Impl
{
    [CreateAssetMenu(menuName = CreateEffect + AssetName, fileName = AssetName, order = 0)]
    public class DamageMultiplierEffect : DamageEffect
    {
        public const string AssetName = nameof(DamageMultiplierEffect);

        [Min(0f)] public float factor = 1f;
        public RoundMode roundMode = RoundMode.Ceil;
        
        public override void HandleDamage(DamageEvent e)
        {
             var amount = e.damage.amount * factor;
             switch (roundMode)
             {
                 case RoundMode.Ceil:
                     e.damage.amount = Mathf.CeilToInt(amount);
                     break;
                 case RoundMode.Floor:
                     e.damage.amount = Mathf.FloorToInt(amount);
                     break;
                 default:
                     throw new ArgumentOutOfRangeException();
             }
        }
        
        public enum RoundMode
        {
            Ceil,
            Floor,
        }
    }
}