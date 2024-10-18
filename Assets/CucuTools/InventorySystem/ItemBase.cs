using UnityEngine;

namespace CucuTools.InventorySystem
{
    public abstract class ItemBase : ScriptableObject
    {
        [field: SerializeField] public int StackMax { get; protected set; } = 1;
    }
}