using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public class SensorEventTrigger2D : SensorEvent
    {
        [Space]
        [SerializeField] private SensorTrigger2D _sensor;

        [Space]
        [SerializeField] private UnityEvent<Collider2D> _onEvent = new UnityEvent<Collider2D>();

        private void OnEvent(Collider2D other)
        {
            _onEvent.Invoke(other);
        }
        
        private void OnEvent(SensorEventType eventType, Collider2D other)
        {
            if (GetEventType() == eventType)
            {
                OnEvent(other);
            }
        }
        
        private void OnSensorEnter(Collider2D other)
        {
            OnEvent(SensorEventType.OnEnter, other);
        }

        private void OnSensorExit(Collider2D other)
        {
            OnEvent(SensorEventType.OnExit, other);
        }
        
        private void Awake()
        {
            if (_sensor == null) _sensor = GetComponentInParent<SensorTrigger2D>();
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