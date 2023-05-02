using CucuTools.DamageSystem.Buffs;
using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    public class DamageManagerExtended : DamageManager
    {
        [Header("Buffs")]
        [SerializeField] private DamageBuffsProfile sourceBuffsProfile = null;
        [SerializeField] private DamageBuffsProfile receiverBuffsProfile = null;

        private DamageBuffManager _sourceBuffsManager = null;
        private DamageBuffManager _receiverBuffsManager = null;
        
        public DamageBuffsProfile SourceBuffsProfile => sourceBuffsProfile ??= DamageBuffsProfile.CreateEmpty();
        public DamageBuffsProfile ReceiverBuffsProfile => receiverBuffsProfile ??= DamageBuffsProfile.CreateEmpty();
        
        public DamageBuffManager SourceBuffsManager => _sourceBuffsManager ??= SourceBuffsProfile.CreateManager();
        public DamageBuffManager ReceiverBuffsManager => _receiverBuffsManager ??= ReceiverBuffsProfile.CreateManager();

        public override void HandleDamageAsSource(DamageEvent e)
        {
            SourceBuffsManager.HandleDamage(e);
        }

        public override void HandleDamageAsReceiver(DamageEvent e)
        {
            ReceiverBuffsManager.HandleDamage(e);
        }
    }
}