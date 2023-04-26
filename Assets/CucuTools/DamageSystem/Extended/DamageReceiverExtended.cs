using CucuTools.DamageSystem.Extended.Effects;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended
{
    public class DamageReceiverExtended : DamageReceiver
    {
        [Header("Effects")]
        public DamageEffectManager effectManager = null;
        
        protected override void HandleDamage(DamageEvent e)
        {
            effectManager?.HandleDamage(e);
        }
    }
}