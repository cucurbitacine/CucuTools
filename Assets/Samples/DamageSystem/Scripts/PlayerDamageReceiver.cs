using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    public class PlayerDamageReceiver : DamageReceiver
    {
        [Min(0)]
        public int health = 100;
        
        [Space]
        public Elemental elementalSelf = Elemental.Fire;

        [Space]
        [Min(1f)]
        public float factor = 2f;

        protected override void HandleDamage(DamageEvent e)
        {
            if (e.damage is ElementalDamage damage)
            {
                Debug.Log($"Damaged by {e.source.name} as {damage.elemental}");
                
                if (elementalSelf == damage.elemental)
                {
                    health += damage.amount;
                    
                    Debug.Log($"Health restore {damage.amount}");
                }
                else if (ElementalDamage.GetDisadvantage(elementalSelf) == damage.elemental)
                {
                    var amount = (int)(damage.amount * factor);
                    
                    health -= amount;
                    
                    Debug.Log($"Health damaged at {amount}");
                } 
                else if (ElementalDamage.GetAdvantage(elementalSelf) == damage.elemental)
                {
                    var amount = (int)(damage.amount / factor);
                    
                    health -= amount;
                    
                    Debug.Log($"Health damaged at {amount}");
                }
                else
                {
                    health -= damage.amount;
                    
                    Debug.Log($"Health damaged at {damage.amount}");
                }
            }
        }
    }
}