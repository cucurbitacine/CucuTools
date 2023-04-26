using CucuTools.DamageSystem.Extended.Effects;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended
{
    public abstract class DamageSourceExtended : DamageSource
    {
        [Header("Effects")]
        public ListDamageEffect effects = new ListDamageEffect();
        
        protected override void HandleDamage(DamageEvent e)
        {
            effects.HandleDamage(e);
        }
    }
}