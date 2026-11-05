using UnityEngine;

namespace CucuTools.SensorSystem
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class Sensor2D : BaseSensor<Collider2D, Collision2D>
    {
        private void Awake()
        {
            SetCollider(GetComponent<Collider2D>());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsValidGameObject(other.gameObject)) return;
            
            OnEnter(other);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsValidGameObject(other.gameObject)) return;
            
            OnExit(other);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!IsValidGameObject(other.gameObject)) return;
            
            OnEnter(other);
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            if (!IsValidGameObject(other.gameObject)) return;
            
            OnExit(other);
        }
    }
}