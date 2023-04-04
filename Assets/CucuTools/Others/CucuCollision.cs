using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Others
{
    public class CucuCollision : CucuBehaviour
    {
        public bool isEnabled = true;
        public bool invokeOnce = false;
        public CollisionMode mode = CollisionMode.Enter;
        public LayerMask layerMask = 1;

        [Space] public UnityEvent<Collision> onCollisionChanged = new UnityEvent<Collision>();

        public List<Collider> whiteList = new List<Collider>();

        [SerializeField] private bool debugLog = true;

        public void SetEnableCollision(bool value)
        {
            isEnabled = value;
        }
        
        public void SwitchEnableCollision()
        {
            SetEnableCollision(!isEnabled);
        }

        public void EnableCollision()
        {
            SetEnableCollision(true);
        }
        
        public void DisableCollision()
        {
            SetEnableCollision(false);
        }
        
        private void Invoke(Collision other)
        {
            if (debugLog) Debug.Log($"[{name}] On Collision {mode} with [{other.gameObject.name}]");

            onCollisionChanged.Invoke(other);

            if (invokeOnce) DisableCollision();
        }

        private void OnCollision(CollisionMode collisionMode, Collision other)
        {
            if (!isEnabled) return;
            if (mode != collisionMode) return;

            if (whiteList.Count > 0)
            {
                if (whiteList.Contains(other.collider))
                {
                    Invoke(other);
                }
            }
            else if (layerMask.Contains(other.gameObject.layer))
            {
                Invoke(other);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            OnCollision(CollisionMode.Enter, other);
        }

        private void OnCollisionStay(Collision other)
        {
            OnCollision(CollisionMode.Stay, other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnCollision(CollisionMode.Exit, other);
        }
    }

    public enum CollisionMode
    {
        Enter,
        Stay,
        Exit,
    }
}