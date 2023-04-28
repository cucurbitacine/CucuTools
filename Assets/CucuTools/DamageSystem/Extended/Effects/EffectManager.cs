using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects
{
    public sealed class EffectManager
    {
        private readonly List<DamageEffect> effects = new List<DamageEffect>();

        public int Count => effects.Count;
        
        public int GetEffectsNonAlloc(DamageEffect[] output)
        {
            var count = 0;

            for (var i = 0; i < Mathf.Min(output.Length, effects.Count); i++)
            {
                if (effects[i] == null) continue;

                output[i] = effects[i];
                count++;
            }

            return count;
        }

        public void AddEffect(DamageEffect effect)
        {
            effects.Add(effect);
        }

        public void RemoveEffect(DamageEffect effect)
        {
            if (effects.Contains(effect))
            {
                effects[effects.IndexOf(effect)] = null;
            }
        }
        
        public void HandleDamage(DamageEvent e)
        {
            var index = 0;
            while (index < effects.Count)
            {
                if (effects[index] != null)
                {
                    effects[index].HandleDamage(e);
                    index++;
                }
                else
                {
                    effects.RemoveAt(index);
                }
            }
        }
    }
    
    public static class EffectManagerExtensions
    {
        public static EffectManager GetManager(this EffectManagerFactory factory, ref EffectManager manager)
        {
            if (factory.global)
            {
                return factory.GetManager();
            }

            if (manager == null)
            {
                manager = factory.CreateManager();
            }

            return manager;
        }
    }
}