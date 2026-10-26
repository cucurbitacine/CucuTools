using UnityEngine;

namespace CucuTools.NodeSystem
{
    [CreateAssetMenu(menuName = "CucuTools/Node System/Value Asset", fileName = "ValueAsset", order = 0)]
    public class ValueAsset : ScriptableObject
    {
        [SerializeField] private ValueData _valueData;
        
        public ValueData GetData()
        {
            return _valueData;
        }
    }
}