using System;
using UnityEngine;

namespace CucuTools.SensorSystem
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class SensorTrigger2D : Sensor2D
    {
        public event Action<Collider2D> OnEnter;
        public event Action<Collider2D> OnExit;

        private void Start()
        {
            GetCollider().isTrigger = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnEnter?.Invoke(other);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            OnExit?.Invoke(other);
        }
    }
}