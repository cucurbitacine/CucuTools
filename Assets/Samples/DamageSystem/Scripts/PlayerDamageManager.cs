using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    public class PlayerDamageManager : DamageManager
    {
        [Min(1)]
        public int level = 1;

        public override void HandleDamageAsSource(DamageEvent e)
        {
            e.damage.amount += (level - 1);
        }

        protected override void Awake()
        {
            sources.Clear();
            sources.AddRange(GetComponentsInChildren<DamageSource>());
            
            receivers.Clear();
            receivers.AddRange(GetComponentsInChildren<DamageReceiver>());
            
            base.Awake();
        }
    }
}