using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public class SensorEventCollision3D : SensorEvent
    {
        [Space]
        [SerializeField] private SensorCollision3D _sensor;

        [Space]
        [SerializeField] private UnityEvent<Collision> _onEvent = new UnityEvent<Collision>();

        private void OnEvent(Collision other)
        {
            _onEvent.Invoke(other);
        }
        
        private void OnEvent(SensorEventType eventType, Collision other)
        {
            if (GetEventType() == eventType)
            {
                OnEvent(other);
            }
        }
        
        private void OnSensorEnter(Collision other)
        {
            OnEvent(SensorEventType.OnEnter, other);
        }

        private void OnSensorExit(Collision other)
        {
            OnEvent(SensorEventType.OnExit, other);
        }
        
        private void Awake()
        {
            if (_sensor == null) _sensor = GetComponentInParent<SensorCollision3D>();
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