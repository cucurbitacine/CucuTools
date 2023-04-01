using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem.Extended.Effects
{
    [Serializable]
    public class ListDamageEffect
    {
        [Space] [SerializeField] private List<DamageEffect> list = new List<DamageEffect>();

        public ListEvents events = new ListEvents();

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
            if (list.Contains(effect)) return;

            list.Add(effect);

            events.onAdded.Invoke(effect);
        }

        public void RemoveEffect(DamageEffect effect)
        {
            if (!list.Contains(effect)) return;

            list[list.IndexOf(effect)] = null;

            events.onRemoved.Invoke(effect);
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

        [Serializable]
        public class ListEvents
        {
            public UnityEvent<DamageEffect> onAdded = new UnityEvent<DamageEffect>();
            public UnityEvent<DamageEffect> onRemoved = new UnityEvent<DamageEffect>();
        }
    }
}