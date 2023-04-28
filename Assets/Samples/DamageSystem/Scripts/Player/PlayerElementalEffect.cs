using System;
using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Extended.Effects;
using UnityEngine;

namespace Samples.DamageSystem.Scripts.Player
{
    [Serializable]
    public class PlayerElementalEffect : DamageEffect
    {
        public PlayerController player = null;
        [Min(0f)] public float factor = 1f;
        
        public PlayerElementalEffect(PlayerController player) : base()
        {
            this.player = player;
        }
        
        public PlayerElementalEffect(PlayerController player, float factor) : this(player)
        {
            this.factor = factor;
        }
        
        public override void HandleDamage(DamageEvent e)
        {
            if (e.damage is ElementalDamage damage)
            {
                Debug.Log($"Player {player.name} received [{damage.elemental.ToString()}] damage");
                
                var advantageElement = ElementalDamage.GetAdvantage(player.elementalSelf);
                var disadvantageElement = ElementalDamage.GetDisadvantage(player.elementalSelf);

                if (damage.elemental == advantageElement)
                {
                    damage.amount = (int)(damage.amount / factor);
                }
                else if (damage.elemental == disadvantageElement)
                {
                    damage.amount = (int)(damage.amount * factor);
                }
            }
        }
    }
}