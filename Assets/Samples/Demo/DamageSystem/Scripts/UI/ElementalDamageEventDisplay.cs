using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.UI
{
    public class ElementalDamageEventDisplay : DamageEventDisplay
    {
        protected override void HandlePopupText(PopupText popupText, DamageEvent damageEvent)
        {
            if (damageEvent.damage is ElementalDamage elementalDamage)
            {
                var color = elementalDamage.elemental switch
                {
                    ElementalType.Fire => Color.red,
                    ElementalType.Grass => Color.green,
                    ElementalType.Water => Color.blue,
                    _ => Color.white,
                };
                
                popupText.SetColor(color);
            }
            
            base.HandlePopupText(popupText, damageEvent);
        }
    }
}