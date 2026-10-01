using UnityEngine;

namespace CucuTools.SensorSystem
{
    [RequireComponent(typeof(Collider))]
    public abstract class Sensor3D : Sensor
    {
        private Collider _collider;

        public Collider GetCollider()
        {
            return _collider;
        }
        
        public bool CheckCollider(Collider collider)
        {
            return CheckGameObject(collider.gameObject);
        }
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }
    }
}