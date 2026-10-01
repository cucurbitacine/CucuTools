using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public class SensorCollisionEvent2D : MonoBehaviour
    {
        [SerializeField] private SensorCollision2D sensor;
        [SerializeField] private SensorEventType eventType;

        [Space]
        [SerializeField] private UnityEvent<Collision2D> onEvent = new UnityEvent<Collision2D>();

        private void OnEvent(Collision2D other)
        {
            onEvent.Invoke(other);
        }
        
        private void OnEvent(SensorEventType type, Collision2D other)
        {
            if (type == eventType)
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