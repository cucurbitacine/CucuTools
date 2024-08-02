using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.Utils
{
    public class ElementalDamageBlink : DamageBlink
    {
        protected override void HandleDamageEvent(DamageEvent damageEvent)
        {
            if (damageEvent.damage is ElementalDamage elementalDamage)
            {
                var color = elementalDamage.elemental switch
                {
                    ElementalType.Fire => Color.red,
                    ElementalType.Grass => Color.green,
                    ElementalType.Water => Color.blue,
                    _ => blinkColor,
                };
                
                Blink(color, blinkDuration);
            }
            else
            {
                base.HandleDamageEvent(damageEvent);
            }
        }
    }
}