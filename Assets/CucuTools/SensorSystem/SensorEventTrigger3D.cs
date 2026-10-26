using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public sealed class SensorEventTrigger3D : SensorEvent3D
    {
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