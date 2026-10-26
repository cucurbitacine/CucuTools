using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public class SensorEventCollision2D : SensorEvent
    {
        [Space]
        [SerializeField] private SensorCollision2D _sensor;
        
        [Space]
        [SerializeField] private UnityEvent<Collision2D> _onEvent = new UnityEvent<Collision2D>();

        private void OnEvent(Collision2D other)
        {
            _onEvent.Invoke(other);
        }
        
        private void OnEvent(SensorEventType eventType, Collision2D other)
        {
            if (GetEventType() == eventType)
            {
                OnEvent(other);
            }
        }
        
        private void OnSensorEnter(Collision2D other)
        {
            OnEvent(SensorEventType.OnEnter, other);
        }

        private void OnSensorExit(Collision2D other)
        {
            OnEvent(SensorEventType.OnExit, other);
        }

        private void Awake()
        {
            if (_sensor == null) _sensor = GetComponentInParent<SensorCollision2D>();
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