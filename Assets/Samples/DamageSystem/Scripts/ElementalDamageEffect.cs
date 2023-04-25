using CucuTools.Attributes;
using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Extended.Effects;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    [CreateAssetMenu(menuName = "Create ElementalDamageEffect", fileName = "ElementalDamageEffect", order = 0)]
    public class ElementalDamageEffect : DamageEffect
    {
        public Elemental elemental = Elemental.Fire;

        [Min(0f)] public float factor = 1f;

        [Header("Example")]
        [Min(0)]
        [SerializeField] private int damageInput = 1;
        [ReadOnlyField]
        [SerializeField] private int damageOutput = 1;
        
        public override void HandleDamage(DamageEvent e)
        {
            if (e.damage is ElementalDamage damage)
            {
                if (damage.elemental == elemental)
                {
                    damage.amount = HandleDamage(damage.amount);
                }
            }
        }

        private int HandleDamage(int damageAmount)
        {
            return (int)(damageAmount * factor);
        }

        private void OnValidate()
        {
            damageOutput = HandleDamage(damageInput);
        }
    }
}