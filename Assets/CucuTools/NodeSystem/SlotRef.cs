using UnityEngine;

namespace CucuTools.NodeSystem
{
    [CreateAssetMenu(menuName = "CucuTools/Node System/Slot Reference", fileName = "SlotRef", order = 0)]
    public class SlotRef : ScriptableObject
    {
        [SerializeField] private string _slotName;
        
        public string GetSlotName()
        {
            return _slotName;
        }
    }
}