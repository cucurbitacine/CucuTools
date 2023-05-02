using System;
using CucuTools.Attributes;
using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Buffs;
using CucuTools.DamageSystem.Buffs.Impl;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    [CreateAssetMenu(menuName = "Create ElementalDamageEffectFactory", fileName = "ElementalDamageEffectFactory",
        order = 0)]
    public class ElementalDamageBuffFactory : DamageBuffFactory
    {
        public bool createInstance = true;
        public ElementalDamageBuff buffSeed = new ElementalDamageBuff();

        public override DamageBuff CreateBuff()
        {
            return createInstance ? new ElementalDamageBuff(buffSeed) : buffSeed;
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
    public class ElementalDamageBuff : DamageMultiplierBuff
    {
        [Header("Elemental")]
        public Elemental elementalReceived = Elemental.Fire;

        public ElementalDamageBuff()
        {
        }

        public ElementalDamageBuff(Elemental elementalReceived, float factor = 1f)
        {
            this.elementalReceived = elementalReceived;
            this.factor = factor;
        }

        public ElementalDamageBuff(ElementalDamageBuff elementalBuff) :
            this(elementalBuff.elementalReceived, elementalBuff.factor)
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