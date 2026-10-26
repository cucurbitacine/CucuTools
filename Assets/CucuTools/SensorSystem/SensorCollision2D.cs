using System;
using UnityEngine;

namespace CucuTools.SensorSystem
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class SensorCollision2D : Sensor2D
    {
        public event Action<Collision2D> OnEnter;
        public event Action<Collision2D> OnExit;
        
        private void Start()
        {
            GetCollider().isTrigger = false;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnEnter?.Invoke(other);
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            OnExit?.Invoke(other);
        }
    }
}