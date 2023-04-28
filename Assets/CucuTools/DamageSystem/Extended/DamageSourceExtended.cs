using CucuTools.DamageSystem.Extended.Effects;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended
{
    public abstract class DamageSourceExtended : DamageSource
    {
        [Header("Effects")]
        [SerializeField] private EffectManagerFactory _effectManagerFactory = null;

        public EffectManagerFactory effectManagerFactory => _effectManagerFactory ??= ScriptableObject.CreateInstance<EffectManagerFactory>();
        public EffectManager effectManager => effectManagerFactory.GetManager(ref _effectManager);

        private EffectManager _effectManager = null;
        
        protected override void HandleDamage(DamageEvent e)
        {
            effectManager.HandleDamage(e);
        }
    }
}