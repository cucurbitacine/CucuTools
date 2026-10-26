using UnityEngine;

namespace CucuTools.SensorSystem
{
    public abstract class SensorEvent : MonoBehaviour
    {
        [SerializeField] private SensorEventType _eventType;
        [SerializeField] private SensorFilterAsset _filter;
        
        public SensorEventType GetEventType()
        {
            return _eventType;
        }
        
        public void SetEventType(SensorEventType eventType)
        {
            _eventType = eventType;
        }
        
        public SensorFilterAsset GetFilter()
        {
            return _filter;
        }
        
        public void SetFilter(SensorFilterAsset newFilter)
        {
            _filter = newFilter;
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

    public abstract class SensorEvent2D : SensorEvent
    {
        [SerializeField] private Sensor2D _sensor;

        public Sensor2D GetSensor()
        {
            return _sensor;
        }
        
        private void Awake()
        {
            if (_sensor == null) _sensor = GetComponent<Sensor2D>();
            if (_sensor == null) _sensor = GetComponentInParent<Sensor2D>();
            if (_sensor == null) _sensor = GetComponentInChildren<Sensor2D>();
        }
    }
    
    public abstract class SensorEvent3D : SensorEvent
    {
        [SerializeField] private Sensor3D _sensor;

        public Sensor3D GetSensor()
        {
            return _sensor;
        }
        
        private void Awake()
        {
            if (_sensor == null) _sensor = GetComponent<Sensor3D>();
            if (_sensor == null) _sensor = GetComponentInParent<Sensor3D>();
            if (_sensor == null) _sensor = GetComponentInChildren<Sensor3D>();
        }
    }
}