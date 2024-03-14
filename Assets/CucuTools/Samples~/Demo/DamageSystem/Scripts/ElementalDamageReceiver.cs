using CucuTools.DamageSystem;
using Samples.Demo.Scripts;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts
{
    public class ElementalDamageReceiver : DamageReceiver
    {
        public int damageBonus;

        [Space] public FlashEffect flashEffect; 
        
        protected override void HandleDamage(DamageEvent e)
        {
            Debug.Log("Begin HandleDamage");
            
            var flashColor = Color.white;

            e.damage.amount += damageBonus;
            
            if (e.damage is ElementalDamage eDamage)
            {
                switch (eDamage.elemental)
                {
                    case Elemental.Fire:
                        flashColor = Color.red;
                        break;
                    case Elemental.Grass:
                        flashColor = Color.green;
                        break;
                    case Elemental.Water:
                        flashColor = Color.blue;
                        break;
                    default:
                        flashColor = Color.white;
                        break;
                }
            }

            Debug.Log("End HandleDamage");
            
            if (flashEffect)
            {
                Debug.Log("SetColor HandleDamage");
                
                flashEffect.SetColor(flashColor);
            }
        }
    }
}