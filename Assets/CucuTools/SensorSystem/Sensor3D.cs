using UnityEngine;

namespace CucuTools.SensorSystem
{
    [RequireComponent(typeof(Collider))]
    public sealed class Sensor3D : Sensor<Collider, Collision>
    {
        private void Awake()
        {
            SetCollider(GetComponent<Collider>());
        }
        
        private void OnTriggerEnter(Collider other)
        {
            OnEnter(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            OnExit(other);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            OnEnter(other);
        }
        
        private void OnCollisionExit(Collision other)
        {
            OnExit(other);
        }
    }
}