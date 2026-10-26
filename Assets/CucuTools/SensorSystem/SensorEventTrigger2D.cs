using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public sealed class SensorEventTrigger2D : SensorEvent2D
    {
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
        
        private void Start()
        {
            GetSensor().onTriggerEnter += OnSensorEnter;
            GetSensor().onTriggerExit += OnSensorExit;
        }
        
        private void OnDestroy()
        {
            GetSensor().onTriggerEnter -= OnSensorEnter;
            GetSensor().onTriggerExit -= OnSensorExit;
        }
    }
}