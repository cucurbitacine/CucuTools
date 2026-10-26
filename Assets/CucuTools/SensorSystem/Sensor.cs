using System;
using UnityEngine;

namespace CucuTools.SensorSystem
{
    [DisallowMultipleComponent]
    public abstract class Sensor<TCollider, TCollision> : MonoBehaviour
    {
        public event Action<TCollider> onTriggerEnter;
        public event Action<TCollider> onTriggerExit;
        
        public event Action<TCollision> onCollisionEnter;
        public event Action<TCollision> onCollisionExit;
        
        private TCollider _collider;

        public TCollider GetCollider()
        {
            return _collider;
        }

        protected void SetCollider(TCollider collider)
        {
            _collider = collider;
        }

        protected void OnEnter(TCollider other) => onTriggerEnter?.Invoke(other);
        protected void OnExit(TCollider other) => onTriggerExit?.Invoke(other);
        protected void OnEnter(TCollision other) => onCollisionEnter?.Invoke(other);
        protected void OnExit(TCollision other) => onCollisionExit?.Invoke(other);
    }
}