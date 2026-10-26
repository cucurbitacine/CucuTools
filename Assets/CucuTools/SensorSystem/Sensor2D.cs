using UnityEngine;

namespace CucuTools.SensorSystem
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Sensor2D : Sensor
    {
        private Collider2D _collider;

        public Collider2D GetCollider()
        {
            return _collider;
        }
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
    }
}