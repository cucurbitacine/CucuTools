using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects
{
    [CreateAssetMenu(menuName = "Create DamageEffectManagerFactory", fileName = "DamageEffectManagerFactory", order = 0)]
    public class EffectManagerFactory : ScriptableObject
    {
        public bool global = false;
        
        [SerializeField] private List<DamageEffectFactory> effectFactories = new List<DamageEffectFactory>();

        private EffectManager _manager = null;

        public EffectManager GetManager()
        {
            if (_manager == null)
            {
                _manager = CreateManager();
            }

            return _manager;
        }

        public EffectManager CreateManager()
        {
            var manager = new EffectManager();

            foreach (var effect in effectFactories)
            {
                if (effect != null) manager.AddEffect(effect.GetEffect());
            }

            return manager;
        } 
    }
}