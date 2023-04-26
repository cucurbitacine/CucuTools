using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects
{
    public class DamageEffectManager : MonoBehaviour
    {
        [SerializeField] private List<DamageEffect> list = new List<DamageEffect>();

        public int Count => list.Count;
        
        public int GetEffectsNonAlloc(DamageEffect[] effects)
        {
            var count = 0;

            for (var i = 0; i < Mathf.Min(effects.Length, list.Count); i++)
            {
                if (list[i] == null) continue;

                effects[i] = list[i];
                count++;
            }

            return count;
        }

        public void AddEffect(DamageEffect effect)
        {
            list.Add(effect);
        }

        public void RemoveEffect(DamageEffect effect)
        {
            if (list.Contains(effect))
            {
                list[list.IndexOf(effect)] = null;
            }
        }
        
        public void HandleDamage(DamageEvent e)
        {
            var index = 0;
            while (index < list.Count)
            {
                if (list[index] != null)
                {
                    list[index].HandleDamage(e);
                    index++;
                }
                else
                {
                    list.RemoveAt(index);
                }
            }
        }
    }
}