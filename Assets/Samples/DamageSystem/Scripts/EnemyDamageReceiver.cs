using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Extended;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    public class EnemyDamageReceiver : DamageReceiverExtended
    {
        [Min(0f)]
        public int health = 100;

        protected override void HandleDamage(DamageEvent e)
        {
            base.HandleDamage(e);

            health -= e.damage.amount;
        }
    }
}