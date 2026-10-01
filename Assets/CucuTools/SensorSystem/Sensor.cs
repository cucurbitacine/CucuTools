using UnityEngine;

namespace CucuTools.SensorSystem
{
    [DisallowMultipleComponent]
    public abstract class Sensor : MonoBehaviour
    {
        [SerializeField] private SensorFilterAsset filter;

        public SensorFilterAsset GetFilter()
        {
            return filter;
        }
        
        public void ChangeFilter(SensorFilterAsset newFilter)
        {
            filter = newFilter;
        }
        
        protected bool CheckGameObject(GameObject other)
        {
            return filter.CheckLayer(other.layer) && filter.CheckTag(other.tag);
        }
    }
}