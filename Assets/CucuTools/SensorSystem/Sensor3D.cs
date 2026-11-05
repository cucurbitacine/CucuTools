using UnityEngine;

namespace CucuTools.SensorSystem
{
    [RequireComponent(typeof(Collider))]
    public sealed class Sensor3D : BaseSensor<Collider, Collision>
    {
        private void Awake()
        {
            SetCollider(GetComponent<Collider>());
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!IsValidGameObject(other.gameObject)) return;
            
            OnEnter(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!IsValidGameObject(other.gameObject)) return;
            
            OnExit(other);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (!IsValidGameObject(other.gameObject)) return;
            
            OnEnter(other);
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (!IsValidGameObject(other.gameObject)) return;
            
            OnExit(other);
        }
    }
}