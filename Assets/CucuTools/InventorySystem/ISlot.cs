using System;

namespace CucuTools.InventorySystem
{
    public interface ISlot
    {
        public event Action<ISlot> SlotUpdated;
        
        public int CountItems { get; }

        public ItemBase GetItem();
        public bool Contains(ItemBase item);
        public int Pick(ItemBase item, int amount = 1);
        public int Put(ItemBase item, int amount = 1);
    }
    
    public static class SlotExt
    {
        public static bool IsFree(this ISlot slot)
        {
            return slot.CountItems == 0;
        }
        
        public static int Available(this ISlot slot, ItemBase item)
        {
            if (slot.IsFree())
            {
                return item.StackMax;
            }
            
            if (slot.Contains(item))
            {
                return item.StackMax - slot.CountItems;
            }

            return 0;
        }
        
        public static bool TryPeek(this ISlot slot, out ItemBase item)
        {
            if (slot.IsFree())
            {
                item = null;
                return false;
            }
            
            item = slot.GetItem();
            return true;
        }
        
        public static bool CanPick(this ISlot slot, ItemBase item, int amount = 1)
        {
            if (amount <= 0) return false;
            
            if (slot.Contains(item))
            {
                return amount <= slot.CountItems;
            }

            return false;
        }
        
        public static bool CanPut(this ISlot slot, ItemBase item, int amount = 1)
        {
            if (amount <= 0) return false;
            
            return amount <= slot.Available(item);
        }
        
        public static bool TryPick(this ISlot slot, ItemBase item, int amount = 1)
        {
            if (slot.CanPick(item, amount))
            {
                slot.Pick(item, amount);
                return true;
            }
            
            return false;
        }
        
        public static bool TryPut(this ISlot slot, ItemBase item, int amount = 1)
        {
            if (slot.CanPut(item, amount))
            {
                slot.Put(item);
                return true;
            }

            return false;
        }

        public static bool TryPickItem(this ISlot slot, out ItemBase item)
        {
            return slot.TryPeek(out item) && slot.TryPick(item);
        }
    }
}