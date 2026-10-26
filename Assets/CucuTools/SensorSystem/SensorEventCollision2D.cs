using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public sealed class SensorEventCollision2D : SensorEvent2D
    {
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
        
        private void Start()
        {
            GetSensor().onCollisionEnter += OnSensorEnter;
            GetSensor().onCollisionExit += OnSensorExit;
        }
        
        private void OnDestroy()
        {
            GetSensor().onCollisionEnter -= OnSensorEnter;
            GetSensor().onCollisionExit -= OnSensorExit;
        }
    }
}