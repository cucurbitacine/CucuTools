using CucuTools.DamageSystem.Extended.Effects;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended
{
    public class DamageManagerExtended : DamageManager
    {
        [Header("Effects")]
        public DamageEffectManager sourceEffectManager = null;
        public DamageEffectManager receiverEffectManager = null;
        
        public override void HandleDamageAsSource(DamageEvent e)
        {
            sourceEffectManager?.HandleDamage(e);
        }

        public override void HandleDamageAsReceiver(DamageEvent e)
        {
            receiverEffectManager?.HandleDamage(e);
        }
    }
}