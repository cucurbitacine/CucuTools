using System;
using System.Collections.Generic;
using System.Linq;

namespace CucuTools.InventorySystem
{
    public interface IInventory
    {
        public event Action<IInventory, ISlot> InventoryUpdated; 
        
        public int CountSlots { get; }
        
        public ISlot GetSlot(int index);
    }

    public static class InventoryExt
    {
        public static int CountItems(this IInventory inventory)
        {
            var amount = 0;

            for (var i = 0; i < inventory.CountSlots; i++)
            {
                amount += inventory.GetSlot(i).CountItems;
            }

            return amount;
        }
        
        public static int CountItems(this IInventory inventory, ItemBase item)
        {
            var count = 0;

            for (var i = 0; i < inventory.CountSlots; i++)
            {
                var slot = inventory.GetSlot(i);

                count += slot.Contains(item) ? slot.CountItems : 0;
            }

            return count;
        }
        
        public static int Available(this IInventory inventory, ItemBase item)
        {
            var available = 0;

            for (var i = 0; i < inventory.CountSlots; i++)
            {
                available += inventory.GetSlot(i).Available(item);
            }

            return available;
        }
        
        public static int Pick(this IInventory inventory, ItemBase item, int amount = 1)
        {
            if (amount <= 0) return 0;

            var picked = 0;

            /*
             * Pick from the end
             */
            
            for (var i = inventory.CountSlots - 1; i >= 0; i--)
            {
                var slot = inventory.GetSlot(i);

                if (!slot.Contains(item)) continue;
                
                picked += slot.Pick(item, amount - picked);
                    
                if (picked >= amount) return picked;
            }

            return picked;
        }
        
        public static int Put(this IInventory inventory, ItemBase item, int amount = 1)
        {
            if (amount <= 0) return 0;

            var put = 0;
            
            /*
             * Put in the beginning, into already existing slots with a current item
             */
            
            for (var i = 0; i < inventory.CountSlots; i++)
            {
                var slot = inventory.GetSlot(i);

                if (slot.Available(item) == 0) continue;

                if (!slot.Contains(item)) continue;
                
                put += slot.Put(item, amount - put);

                if (put == amount) return put;
            }
            
            /*
             * Put in the beginning, into available slots
             */
            
            for (var i = 0; i < inventory.CountSlots; i++)
            {
                var slot = inventory.GetSlot(i);

                if (slot.Available(item) == 0) continue;

                put += slot.Put(item, amount - put);

                if (put == amount) return put;
            }
            
            return put;
        }

        public static bool CanPick(this IInventory inventory, ItemBase item, int amount = 1)
        {
            if (amount <= 0) return false;

            return amount <= inventory.CountItems(item);
        }
        
        public static bool CanPut(this IInventory inventory, ItemBase item, int amount = 1)
        {
            if (amount <= 0) return false;

            return amount <= inventory.Available(item);
        }

        public static bool TryPick(this IInventory inventory, ItemBase item, int amount = 1)
        {
            if (inventory.CanPick(item, amount))
            {
                inventory.Pick(item, amount);
                return true;
            }

            return false;
        }
        
        public static bool TryPut(this IInventory inventory, ItemBase item, int amount = 1)
        {
            if (inventory.CanPut(item, amount))
            {
                inventory.Put(item, amount);
                return true;
            }

            return false;
        }
        
        public static IEnumerable<ISlot> GetSlotsWithItems(this IInventory inventory)
        {
            var slots = new HashSet<ISlot>();

            for (var i = 0; i < inventory.CountSlots; i++)
            {
                var slot = inventory.GetSlot(i);

                if (slot.IsFree()) continue;

                slots.Add(slot);
            }

            return slots;
        }

        public static IEnumerable<ItemBase> GetItems(this IInventory inventory)
        {
            return inventory.GetSlotsWithItems().Select(s => s.GetItem());
        }
        
        public static bool IsPossibleGiveAnything(this IInventory inventory, IInventory destination)
        {
            return inventory.GetItems().Any(i => destination.Available(i) > 0);
        }
        
        public static bool TryPickItem(this IInventory inventory, out ItemBase item)
        {
            for (var i = inventory.CountSlots - 1; i >=0 ; i--)
            {
                var slot = inventory.GetSlot(i);

                if (slot.TryPickItem(out item))
                {
                    return true;
                }
            }

            item = null;
            return false;
        }
    }
}
