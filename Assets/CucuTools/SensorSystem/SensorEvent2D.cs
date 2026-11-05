using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public sealed class SensorEvent2D : SensorEvent
    {
        [SerializeField] private Sensor2D _sensor;

        [Space]
        [SerializeField] private UnityEvent<Collider2D> _onEvent = new UnityEvent<Collider2D>();

        public Sensor2D GetSensor()
        {
            return _sensor;
        }
        
        private void OnCollision(Collision2D other)
        {
            _onEvent.Invoke(other.collider);
        }
        
        private void OnCollision(SensorEventType eventType, Collision2D other)
        {
            if (GetEventType() == eventType)
            {
                OnCollision(other);
            }
        }
        
        private void OnSensorEnter(Collision2D other)
        {
            OnCollision(SensorEventType.OnEnter, other);
        }

        private void OnSensorExit(Collision2D other)
        {
            OnCollision(SensorEventType.OnExit, other);
        }
        
        private void OnTrigger(Collider2D other)
        {
            _onEvent.Invoke(other);
        }
        
        private void OnTrigger(SensorEventType eventType, Collider2D other)
        {
            if (GetEventType() == eventType)
            {
                OnTrigger(other);
            }
        }
        
        private void OnSensorEnter(Collider2D other)
        {
            OnTrigger(SensorEventType.OnEnter, other);
        }

        private void OnSensorExit(Collider2D other)
        {
            OnTrigger(SensorEventType.OnExit, other);
        }
        
        private void Awake()
        {
            if (_sensor == null) _sensor = GetComponent<Sensor2D>();
            if (_sensor == null) _sensor = GetComponentInParent<Sensor2D>();
            if (_sensor == null) _sensor = GetComponentInChildren<Sensor2D>();
        }
        
        private void Start()
        {
            _sensor.onCollisionEnter += OnSensorEnter;
            _sensor.onCollisionExit += OnSensorExit;
            _sensor.onTriggerEnter += OnSensorEnter;
            _sensor.onTriggerExit += OnSensorExit;
        }
        
        private void OnDestroy()
        {
            _sensor.onCollisionEnter -= OnSensorEnter;
            _sensor.onCollisionExit -= OnSensorExit;
            _sensor.onTriggerEnter -= OnSensorEnter;
            _sensor.onTriggerExit -= OnSensorExit;
        }
    }
}