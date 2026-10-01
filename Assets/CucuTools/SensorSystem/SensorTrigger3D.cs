using System;
using UnityEngine;

namespace CucuTools.SensorSystem
{
    [RequireComponent(typeof(Collider))]
    public sealed class SensorTrigger3D : Sensor3D
    {
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnExit;

        private void OnTriggerEnter(Collider other)
        {
            if (!CheckCollider(other)) return;
            
            OnEnter?.Invoke(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!CheckCollider(other)) return;
            
            OnExit?.Invoke(other);
        }
        
        private void Start()
        {
            GetCollider().isTrigger = true;
        }
    }
}