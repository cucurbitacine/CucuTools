using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public class SensorTriggerEvent2D : MonoBehaviour
    {
        [SerializeField] private SensorTrigger2D sensor;
        [SerializeField] private SensorEventType eventType;

        [Space]
        [SerializeField] private UnityEvent<Collider2D> onEvent = new UnityEvent<Collider2D>();

        private void OnEvent(Collider2D other)
        {
            onEvent.Invoke(other);
        }
        
        private void OnEvent(SensorEventType type, Collider2D other)
        {
            if (type == eventType)
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