using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Others
{
    public class CucuCollision : MonoBehaviour
    {
        public bool isEnabled = true;
        public bool invokeOnce = false;
        public CollisionMode mode = CollisionMode.Enter;
        public LayerMask layerMask = 1;

        [Space] public UnityEvent<Collision> onChanged = new UnityEvent<Collision>();

        public List<Collider> whiteList = new List<Collider>();

        [SerializeField] private bool debugLog = true;

        public void SetEnable(bool value)
        {
            isEnabled = value;
        }
        
        public void SwitchEnable()
        {
            SetEnable(!isEnabled);
        }

        public void Enable()
        {
            SetEnable(true);
        }
        
        public void Disable()
        {
            SetEnable(false);
        }
        
        private void Invoke(Collision other)
        {
            if (debugLog) Debug.Log($"[{name}] On Collision {mode} with [{other.gameObject.name}]");

            onChanged.Invoke(other);

            if (invokeOnce) Disable();
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