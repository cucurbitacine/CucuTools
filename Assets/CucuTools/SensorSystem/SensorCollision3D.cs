using System;
using UnityEngine;

namespace CucuTools.SensorSystem
{
    [RequireComponent(typeof(Collider))]
    public sealed class SensorCollision3D : Sensor3D
    {
        public event Action<Collision> OnEnter;
        public event Action<Collision> OnExit;
        
        private void OnCollisionEnter(Collision other)
        {
            if (!CheckCollider(other.collider)) return;
            
            OnEnter?.Invoke(other);
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (!CheckCollider(other.collider)) return;
            
            OnExit?.Invoke(other);
        }
        
        private void Start()
        {
            GetCollider().isTrigger = false;
        }
    }
}