using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.SensorSystem
{
    [CreateAssetMenu(menuName = "CucuTools/Sensor System/Sensor Filter Asset", fileName = "SensorFilter", order = 0)]
    public class SensorFilterAsset : ScriptableObject
    {
        [SerializeField] private LayerMask sensorLayers = ~0;
        [Space]
        [SerializeField] private List<string> sensorTags = new List<string>();

        public int SensorTagsCount => sensorTags.Count;
        public string GetSensorTag(int number) => sensorTags[number];
        public LayerMask GetSensorLayer() => sensorLayers;
        
        public bool CheckLayer(int layer)
        {
            return (sensorLayers.value & (1 << layer)) != 0;
        }
        
        public bool CheckTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return true;
            if (sensorTags == null || sensorTags.Count == 0) return true;
            
            return sensorTags.Contains(tag);
        }
    }
}