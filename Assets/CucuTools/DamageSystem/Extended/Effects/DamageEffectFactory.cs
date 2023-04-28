using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects
{
    public abstract class DamageEffectFactory : ScriptableObject
    {
        public const string EffectGroup = "Effect/";
        public const string CreateEffectMenu = Cucu.CreateAsset + Cucu.DamageGroup + EffectGroup;

        public abstract DamageEffect GetEffect();
    }

    public abstract class DamageEffectFactory<TEffect> : DamageEffectFactory
        where TEffect : DamageEffect
    {
        public abstract TEffect CreateEffect();

        public override DamageEffect GetEffect()
        {
            return CreateEffect();
        }
    }
}