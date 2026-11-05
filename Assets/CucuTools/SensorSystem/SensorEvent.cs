using UnityEngine;

namespace CucuTools.SensorSystem
{
    public abstract class SensorEvent : MonoBehaviour
    {
        [SerializeField] private SensorEventType _eventType;
        
        public SensorEventType GetEventType()
        {
            return _eventType;
        }
        
        public void SetEventType(SensorEventType eventType)
        {
            _eventType = eventType;
        }
    }
    
    public enum SensorEventType
    {
        OnEnter,
        OnExit,
    }
}