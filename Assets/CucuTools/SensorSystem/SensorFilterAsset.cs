using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.SensorSystem
{
    [CreateAssetMenu(menuName = "CucuTools/Sensor System/Sensor Filter Asset", fileName = "SensorFilter", order = 0)]
    public class SensorFilterAsset : ScriptableObject
    {
        [SerializeField] private LayerMask _layerMask = ~0;
        [Space]
        [SerializeField] private List<string> _tags = new List<string>();

        public LayerMask GetSensorMask() => _layerMask;
        public IReadOnlyList<string> GetTags() => _tags;

        public bool IsValidGameObject(GameObject gameObject)
        {
            return IsValidLayer(gameObject.layer) && IsValidTag(gameObject.tag);
        }
        
        public bool IsValidLayer(int layer)
        {
            var mask = GetSensorMask();
            return (mask.value & (1 << layer)) != 0;
        }
        
        public bool IsValidTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return true;

            var tags = GetTags();
            if (tags == null || tags.Count == 0) return true;
            
            return tags.Contains(tag);
        }
    }
}