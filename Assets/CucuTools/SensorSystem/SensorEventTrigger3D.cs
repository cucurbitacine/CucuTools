using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public class SensorEventTrigger3D : SensorEvent
    {
        [Space]
        [SerializeField] private SensorTrigger3D _sensor;

        [Space]
        [SerializeField] private UnityEvent<Collider> _onEvent = new UnityEvent<Collider>();

        private void OnEvent(Collider other)
        {
            _onEvent.Invoke(other);
        }
        
        private void OnEvent(SensorEventType eventType, Collider other)
        {
            if (GetEventType() == eventType)
            {
                OnEvent(other);
            }
        }
        
        private void OnSensorEnter(Collider other)
        {
            OnEvent(SensorEventType.OnEnter, other);
        }

        private void OnSensorExit(Collider other)
        {
            OnEvent(SensorEventType.OnExit, other);
        }
        
        private void Awake()
        {
            if (_sensor == null) _sensor = GetComponentInParent<SensorTrigger3D>();
        }
        
        private void Start()
        {
            _sensor.OnEnter += OnSensorEnter;
            _sensor.OnExit += OnSensorExit;
        }
        
        private void OnDestroy()
        {
            _sensor.OnEnter -= OnSensorEnter;
            _sensor.OnExit -= OnSensorExit;
        }
    }
}