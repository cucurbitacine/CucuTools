using System;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects.Impl
{
    [CreateAssetMenu(menuName = CreateEffectMenu + AssetName, fileName = AssetName, order = 0)]
    public sealed class DamageMultiplierEffectFactory : DamageEffectFactory<DamageMultiplierEffect>
    {
        public const string AssetName = nameof(DamageMultiplierEffectFactory);

        public bool createInstance = true;
        public DamageMultiplierEffect effectSeed = new DamageMultiplierEffect();

        public override DamageMultiplierEffect CreateEffect()
        {
            return createInstance ? new DamageMultiplierEffect(effectSeed) : effectSeed;
        }
        
#if UNITY_EDITOR
        [Header("Debug")]
        [Min(0)] [SerializeField] private int damageInput = 1;
        [ReadOnlyField] [SerializeField] private int damageOutput = 1;

        private void OnValidate()
        {
            damageOutput = effectSeed.Multiply(damageInput);
        }
#endif
    }

    [Serializable]
    public class DamageMultiplierEffect : DamageEffect
    {
        [Min(0f)] public float factor = 1f;

        [Space]
        public RoundMode roundMode = RoundMode.Upper;
        public bool canBeZero = true;

        public DamageMultiplierEffect()
        {
        }

        public DamageMultiplierEffect(DamageMultiplierEffect multiplierEffect)
        {
            factor = multiplierEffect.factor;
            roundMode = multiplierEffect.roundMode;
            canBeZero = multiplierEffect.canBeZero;
        }

        public int Multiply(int value)
        {
            var amount = value * factor;
            var result = (int)amount;

            switch (roundMode)
            {
                case RoundMode.Upper:
                    result = Mathf.CeilToInt(amount);
                    break;
                case RoundMode.Lower:
                    result = Mathf.FloorToInt(amount);
                    break;
            }

            return Mathf.Max(result, canBeZero ? 0 : 1);
        }

        public override void HandleDamage(DamageEvent e)
        {
            e.damage.amount = Multiply(e.damage.amount);
        }

        public enum RoundMode
        {
            Upper,
            Lower,
        }
    }
}