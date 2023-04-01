using CucuTools.DamageSystem.Extended.Effects;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended
{
    public class DamageManagerExtended : DamageManager
    {
        [Space]
        public ListDamageEffect sourceEffects = new ListDamageEffect();
        public ListDamageEffect receiverEffects = new ListDamageEffect();
        
        public override void HandleDamageAsSource(DamageEvent e)
        {
            sourceEffects.HandleDamage(e);
        }

        public override void HandleDamageAsReceiver(DamageEvent e)
        {
            receiverEffects.HandleDamage(e);
        }
    }
}