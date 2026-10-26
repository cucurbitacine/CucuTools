using UnityEngine;

namespace CucuTools.SensorSystem
{
    public abstract class SensorEvent : MonoBehaviour
    {
        [SerializeField] private SensorFilterAsset _filter;
        [SerializeField] private SensorEventType _eventType;
        
        public SensorFilterAsset GetFilter()
        {
            return _filter;
        }
        
        public void SetFilter(SensorFilterAsset newFilter)
        {
            _filter = newFilter;
        }

        public SensorEventType GetEventType()
        {
            return _eventType;
        }
        
        public void SetEventType(SensorEventType eventType)
        {
            _eventType = eventType;
        }
        
        public bool IsValidGameObject(GameObject other)
        {
            var filter = GetFilter();
            
            if (filter == null) return true;
            
            return filter.CheckGameObject(other);
        }
        
        public bool IsValidCollider(Collider2D other)
        {
            return IsValidGameObject(other.gameObject);
        }
        
        public bool IsValidCollider(Collider other)
        {
            return IsValidGameObject(other.gameObject);
        }
    }
}