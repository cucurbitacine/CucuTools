using System;
using UnityEngine;

namespace CucuTools.InventorySystem
{
    [Serializable]
    public sealed class Slot : ISlot
    {
        [field: SerializeField, Min(0)] public int CountItems { get; private set; } = 0;
        [field: SerializeField] private ItemBase Item { get; set; }
        
        public event Action<ISlot> SlotUpdated;
        
        public ItemBase GetItem()
        {
            return Item;
        }
        
        private void SetItem(ItemBase item)
        {
            Item = item;
        }
        
        public bool Contains(ItemBase item)
        {
            if (CountItems == 0) return false;

            return GetItem() == item;
        }

        public int Pick(ItemBase item, int amount = 1)
        {
            if (amount <= 0) return 0;
            
            if (!Contains(item)) return 0;

            amount = Mathf.Min(amount, CountItems);

            CountItems -= amount;

            if (CountItems == 0)
            {
                SetItem(null);
            }
            
            SlotUpdated?.Invoke(this);
            
            return amount;
        }

        public int Put(ItemBase item, int amount = 1)
        {
            if (amount <= 0) return 0;

            if (CountItems != 0 && !Contains(item)) return 0;
            
            amount = Mathf.Min(amount, item.StackMax - CountItems);

            if (CountItems == 0 && amount > 0)
            {
                SetItem(item);
            }

            CountItems += amount;

            SlotUpdated?.Invoke(this);
            
            return amount;
        }

        public void SilenceCopy(ISlot slot)
        {
            if (slot == this) return;

            CountItems = slot.CountItems;
            SetItem(slot.GetItem());
        }
    }
}