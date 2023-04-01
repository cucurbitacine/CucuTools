using CucuTools.DamageSystem.Extended.Effects;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended
{
    public abstract class DamageSourceExtended : DamageSource
    {
        [Space]
        public ListDamageEffect effects = new ListDamageEffect();
        
        protected override void HandleDamage(DamageEvent e)
        {
            effects.HandleDamage(e);
        }
    }
}