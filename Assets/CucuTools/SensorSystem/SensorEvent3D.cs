using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public sealed class SensorEvent3D : SensorEvent
    {
        [SerializeField] private Sensor3D _sensor;

        [Space]
        [SerializeField] private UnityEvent<Collider> _onEvent = new UnityEvent<Collider>();

        public Sensor3D GetSensor()
        {
            return _sensor;
        }
        
        private void OnCollision(Collision other)
        {
            _onEvent.Invoke(other.collider);
        }
        
        private void OnCollision(SensorEventType eventType, Collision other)
        {
            if (GetEventType() == eventType)
            {
                OnCollision(other);
            }
        }
        
        private void OnSensorEnter(Collision other)
        {
            OnCollision(SensorEventType.OnEnter, other);
        }

        private void OnSensorExit(Collision other)
        {
            OnCollision(SensorEventType.OnExit, other);
        }
        
        private void OnTrigger(Collider other)
        {
            _onEvent.Invoke(other);
        }
        
        private void OnTrigger(SensorEventType eventType, Collider other)
        {
            if (GetEventType() == eventType)
            {
                OnTrigger(other);
            }
        }
        
        private void OnSensorEnter(Collider other)
        {
            OnTrigger(SensorEventType.OnEnter, other);
        }

        private void OnSensorExit(Collider other)
        {
            OnTrigger(SensorEventType.OnExit, other);
        }
        
        private void Awake()
        {
            if (_sensor == null) _sensor = GetComponent<Sensor3D>();
            if (_sensor == null) _sensor = GetComponentInParent<Sensor3D>();
            if (_sensor == null) _sensor = GetComponentInChildren<Sensor3D>();
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