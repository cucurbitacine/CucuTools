using System;
using CucuTools.Attributes;
using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Extended.Effects;
using CucuTools.DamageSystem.Extended.Effects.Impl;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    [CreateAssetMenu(menuName = "Create ElementalDamageEffectFactory", fileName = "ElementalDamageEffectFactory",
        order = 0)]
    public class ElementalDamageEffectFactory : DamageEffectFactory<ElementalDamageEffect>
    {
        public bool createInstance = true;
        public ElementalDamageEffect effectSeed = new ElementalDamageEffect();

        public override ElementalDamageEffect CreateEffect()
        {
            return createInstance ? new ElementalDamageEffect(effectSeed) : effectSeed;
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
    public class ElementalDamageEffect : DamageMultiplierEffect
    {
        [Header("Elemental")]
        public Elemental elementalReceived = Elemental.Fire;

        public ElementalDamageEffect()
        {
        }

        public ElementalDamageEffect(Elemental elementalReceived, float factor = 1f)
        {
            this.elementalReceived = elementalReceived;
            this.factor = factor;
        }

        public ElementalDamageEffect(ElementalDamageEffect elementalEffect) :
            this(elementalEffect.elementalReceived, elementalEffect.factor)
        {
        }

        public override void HandleDamage(DamageEvent e)
        {
            if (e.damage is ElementalDamage damage)
            {
                if (damage.elemental == elementalReceived)
                {
                    base.HandleDamage(e);
                }
            }
        }
    }
}