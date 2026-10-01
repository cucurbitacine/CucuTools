using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    public abstract class DamageHandler : MonoBehaviour
    {
        private struct ModificatorEntry
        {
            public DamageModificator Modificator;
            public int Sequence;
        }
        
        private readonly List<ModificatorEntry> list = new List<ModificatorEntry>();
        private int sequenceCounter;
        private bool dirty;
        
        public void AddModificator(DamageModificator modificator)
        {
            list.Add(new ModificatorEntry()
            {
                Modificator = modificator,
                Sequence = sequenceCounter++,
            });

            dirty = true;
        }

        public void RemoveModificator(DamageModificator modificator)
        {
            if (list.RemoveAll(e => e.Modificator == modificator) != 0)
            {
                dirty = true;
            }
        }

        public void Modify(DamageProcess process)
        {
            EnsureSorted();

            for (var i = 0; i < list.Count; i++)
            {
                list[i].Modificator.Modify(process);
            }
        }

        private void EnsureSorted()
        {
            if (!dirty) return;

            list.Sort(Compare);
            dirty = false;
        }

        private static int Compare(ModificatorEntry a, ModificatorEntry b)
        {
            var cmp = a.Modificator.ModificatorOrder.CompareTo(b.Modificator.ModificatorOrder);
            return cmp != 0 ? cmp : a.Sequence.CompareTo(b.Sequence);
        }
    }
}