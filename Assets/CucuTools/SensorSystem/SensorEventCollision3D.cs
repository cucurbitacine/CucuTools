using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public sealed class SensorEventCollision3D : SensorEvent3D
    {
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