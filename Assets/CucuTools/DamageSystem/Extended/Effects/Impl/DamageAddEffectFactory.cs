using System;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects.Impl
{
    [CreateAssetMenu(menuName = CreateEffectMenu + AssetName, fileName = AssetName, order = 0)]
    public class DamageAddEffectFactory : DamageEffectFactory<DamageAddEffect>
    {
        public const string AssetName = nameof(DamageAddEffectFactory);

        public bool createInstance = true;
        public DamageAddEffect effectSeed = new DamageAddEffect();
        
        public override DamageAddEffect CreateEffect()
        {
            return createInstance ? new DamageAddEffect(effectSeed) : effectSeed;
        }
        
#if UNITY_EDITOR
        [Header("Debug")]
        [Min(0)] [SerializeField] private int damageInput = 1;
        [ReadOnlyField] [SerializeField] private int damageOutput = 1;

        private void OnValidate()
        {
            damageOutput = effectSeed.Add(damageInput);
        }
#endif
    }

    [Serializable]
    public class DamageAddEffect : DamageEffect
    {
        public int addition = 0;
        
        [Space]
        public bool canBeZero = true;

        public DamageAddEffect()
        {
        }
        
        public DamageAddEffect(DamageAddEffect addEffect)
        {
            addition = addEffect.addition;
            canBeZero = addEffect.canBeZero;
        }

        public int Add(int value)
        {
            var result = value + addition;
            
            return Mathf.Max(result, canBeZero ? 0 : 1);
        }
        
        public override void HandleDamage(DamageEvent e)
        {
            e.damage.amount = Add(e.damage.amount);
        }
    }
}