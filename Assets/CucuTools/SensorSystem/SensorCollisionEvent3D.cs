using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public class SensorCollisionEvent3D : MonoBehaviour
    {
        [SerializeField] private SensorCollision3D sensor;
        [SerializeField] private SensorEventType eventType;

        [Space]
        [SerializeField] private UnityEvent<Collision> onEvent = new UnityEvent<Collision>();

        private void OnEvent(Collision other)
        {
            onEvent.Invoke(other);
        }
        
        private void OnEvent(SensorEventType type, Collision other)
        {
            if (type == eventType)
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
            sensor.OnEnter += OnSensorEnter;
            sensor.OnExit += OnSensorExit;
        }
        
        private void OnDestroy()
        {
            sensor.OnEnter -= OnSensorEnter;
            sensor.OnExit -= OnSensorExit;
        }
    }
}