using System;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.DamageSystem.Buffs.Impl
{
    [CreateAssetMenu(menuName = CreateEffectMenu + AssetName, fileName = AssetName, order = 0)]
    public sealed class DamageMultiplierBuffFactory : DamageBuffFactory
    {
        public const string AssetName = nameof(DamageMultiplierBuffFactory);

        public bool createInstance = true;
        public DamageMultiplierBuff buffSeed = new DamageMultiplierBuff();

        public override DamageBuff CreateBuff()
        {
            return createInstance ? new DamageMultiplierBuff(buffSeed) : buffSeed;
        }
        
#if UNITY_EDITOR
        [Header("Debug")]
        [Min(0)] [SerializeField] private int damageInput = 1;
        [ReadOnlyField] [SerializeField] private int damageOutput = 1;

        private void OnValidate()
        {
            damageOutput = buffSeed.Multiply(damageInput);
        }
#endif
    }

    [Serializable]
    public class DamageMultiplierBuff : DamageBuff
    {
        [Min(0f)] public float factor = 1f;

        [Space]
        public RoundMode roundMode = RoundMode.Upper;
        public bool canBeZero = true;

        public DamageMultiplierBuff()
        {
        }

        public DamageMultiplierBuff(DamageMultiplierBuff multiplierBuff)
        {
            factor = multiplierBuff.factor;
            roundMode = multiplierBuff.roundMode;
            canBeZero = multiplierBuff.canBeZero;
        }

        public int Multiply(int value)
        {
            if (value == 0) return 0;
            
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