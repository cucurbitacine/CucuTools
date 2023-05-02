using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem.Buffs
{
    public sealed class DamageBuffManager
    {
        private readonly List<DamageBuff> buffs = new List<DamageBuff>();

        public delegate void BuffEvent(DamageBuff buff);

        public event BuffEvent onAddedBuff;
        public event BuffEvent onRemovedBuff;
        
        public int Count => buffs.Count;
        
        public int GetBuffsNonAlloc(DamageBuff[] output)
        {
            var count = 0;

            for (var i = 0; i < Mathf.Min(output.Length, buffs.Count); i++)
            {
                if (buffs[i] == null) continue;

                output[i] = buffs[i];
                count++;
            }

            return count;
        }

        public void AddBuff(DamageBuff buff)
        {
            if (buff == null) throw new ArgumentNullException();
            if (buffs.Contains(buff)) throw new ArgumentException();
            
            buffs.Add(buff);
            buff.Start(this);

            onAddedBuff?.Invoke(buff);
        }

        public void RemoveBuff(DamageBuff buff)
        {
            if (buff == null) throw new ArgumentNullException();
            
            if (buffs.Contains(buff))
            {
                buffs[buffs.IndexOf(buff)] = null;
                buff.Stop(this);

                onRemovedBuff?.Invoke(buff);
            }
        }
        
        public void HandleDamage(DamageEvent e)
        {
            var index = 0;
            while (index < buffs.Count)
            {
                if (buffs[index] != null)
                {
                    buffs[index].HandleDamage(e);
                    index++;
                }
                else
                {
                    buffs.RemoveAt(index);
                }
            }
        }
    }
}