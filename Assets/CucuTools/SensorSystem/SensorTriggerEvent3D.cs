using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.SensorSystem
{
    public class SensorTriggerEvent3D : MonoBehaviour
    {
        [SerializeField] private SensorTrigger3D sensor;
        [SerializeField] private SensorEventType eventType;

        [Space]
        [SerializeField] private UnityEvent<Collider> onEvent = new UnityEvent<Collider>();

        private void OnEvent(Collider other)
        {
            onEvent.Invoke(other);
        }
        
        private void OnEvent(SensorEventType type, Collider other)
        {
            if (type == eventType)
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