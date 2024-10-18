using System;
using UnityEngine;

namespace CucuTools.InventorySystem
{
    [DisallowMultipleComponent]
    public class InventoryController : MonoBehaviour, IInventory
    {
        [SerializeField] private Inventory inventory;
        
        [Header("New Inventory")]
        [SerializeField] [Min(1)] private int capacityDefault = 5;
        
        private Inventory Inventory => GetInventory();

        #region IInventory

        public virtual int CountSlots => Inventory.CountSlots;
        
        public virtual event Action<IInventory, ISlot> InventoryUpdated;
        
        public virtual ISlot GetSlot(int index)
        {
            return Inventory.GetSlot(index);
        }

        #endregion

        private Inventory GetInventory()
        {
            if (!inventory) return inventory = Inventory.Create(capacityDefault);

            if (!inventory.IsTemplate) return inventory;
            
            var copy = Inventory.Create(capacityDefault);
               
            copy.SilenceCopy(inventory);
                    
            return inventory = copy;
        }

        private void OnInventoryUpdated(IInventory inv, ISlot slt)
        {
            InventoryUpdated?.Invoke(this, slt);
        }
        
        private void OnEnable()
        {
            Inventory.InventoryUpdated += OnInventoryUpdated;
        }

        private void OnDisable()
        {
            Inventory.InventoryUpdated -= OnInventoryUpdated;
        }
    }
}