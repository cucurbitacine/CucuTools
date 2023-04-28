using CucuTools.DamageSystem.Extended.Effects;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended
{
    public class DamageManagerExtended : DamageManager
    {
        [Header("Effects")]
        [SerializeField] private EffectManagerFactory _sourceEffectManagerFactory = null;
        [SerializeField] private EffectManagerFactory _receiverEffectManagerFactory = null;

        public EffectManagerFactory sourceEffectManagerFactory => _sourceEffectManagerFactory ??= ScriptableObject.CreateInstance<EffectManagerFactory>();
        public EffectManagerFactory receiverEffectManagerFactory => _receiverEffectManagerFactory ??= ScriptableObject.CreateInstance<EffectManagerFactory>();
                                                                  
        public EffectManager sourceEffectManager => sourceEffectManagerFactory.GetManager(ref _sourceEffectManager);
        public EffectManager receiverEffectManager => receiverEffectManagerFactory.GetManager(ref _receiverEffectManager);
        
        private EffectManager _sourceEffectManager = null;
        private EffectManager _receiverEffectManager = null;
        
        public override void HandleDamageAsSource(DamageEvent e)
        {
            sourceEffectManager.HandleDamage(e);
        }

        public override void HandleDamageAsReceiver(DamageEvent e)
        {
            receiverEffectManager.HandleDamage(e);
        }
    }
}