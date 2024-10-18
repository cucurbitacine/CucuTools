using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.InventorySystem
{
    [CreateAssetMenu(menuName = Cucu.CreateAsset + "Inventory/Create Inventory", fileName = "Inventory", order = 0)]
    public sealed class Inventory : ScriptableObject, IInventory
    {
        [SerializeField] private bool isTemplate = false;
        
        [Space]
        [SerializeField] private List<Slot> slots = new List<Slot>();

        [Header("Editor")]
        [SerializeField] private bool clearInventory = false;

        private bool isInitialized = false;
        
        public bool IsTemplate => isTemplate;
        
        public static Inventory Create(int capacity)
        {
            capacity = Mathf.Max(1, capacity);

            var inventory = CreateInstance<Inventory>();
            inventory.name = $"{nameof(Inventory)} {capacity} slots";
            
            inventory.SetCapacity(capacity);

            return inventory;
        }

        #region IInventory

        public event Action<IInventory, ISlot> InventoryUpdated;

        public int CountSlots => slots.Count;
        
        public ISlot GetSlot(int index)
        {
            if (0 <= index && index < CountSlots)
            {
                return slots[index];
            }

            return null;
        }

        #endregion

        public void SetCapacity(int capacity)
        {
            var wasInitialized = isInitialized;
            
            if (isInitialized)
            {
                Deinitialize();
            }
            
            slots.Clear();
            for (var i = 0; i < Mathf.Max(1, capacity); i++)
            {
                slots.Add(new Slot());
            }

            if (wasInitialized)
            {
                Initialize();
            }
        }

        public void SilenceCopy(IInventory inventory)
        {
            if (inventory is Inventory inventoryBase && inventoryBase == this) return;
            
            SetCapacity(inventory.CountSlots);

            for (var i = 0; i < inventory.CountSlots; i++)
            {
                slots[i].SilenceCopy(inventory.GetSlot(i));
            }
        }
        
        private void Initialize()
        {
            if (isInitialized)
            {
                Debug.LogWarning($"\"{name}\" is already Initialized");
                return;
            }

            foreach (var slot in slots)
            {
                slot.SlotUpdated += OnSlotUpdated;
            }

            isInitialized = true;
            
            //Debug.Log($"\"{name}\" has Initialized");
        }
        
        private void Deinitialize()
        {
            if (!isInitialized)
            {
                Debug.LogWarning($"\"{name}\" is not Initialized yet");
                return;
            }
            
            foreach (var slot in slots)
            {
                slot.SlotUpdated -= OnSlotUpdated;
            }
            
            isInitialized = false;
            
            //Debug.Log($"\"{name}\" has Deinitialized");
        }
        
        private void OnSlotUpdated(ISlot slot)
        {
            InventoryUpdated?.Invoke(this, slot);
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Deinitialize();
        }

        private void OnValidate()
        {
            if (clearInventory)
            {
                clearInventory = false;

                SetCapacity(CountSlots);
            }
        }
    }
}