using CucuTools.DamageSystem.Buffs;
using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    public class DamageReceiverExtended : DamageReceiver
    {
        [Header("Buffs")]
        [SerializeField] private DamageBuffsProfile buffsProfile = null;

        private DamageBuffManager _buffsManager = null;
        
        public DamageBuffsProfile BuffsProfile => buffsProfile ??= DamageBuffsProfile.CreateEmpty();
        public DamageBuffManager BuffsManager => _buffsManager ??= BuffsProfile.CreateManager();

        protected override void HandleDamage(DamageEvent e)
        {
            BuffsManager.HandleDamage(e);
        }
    }
}