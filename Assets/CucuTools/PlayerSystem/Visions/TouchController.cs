using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem.Visions
{
    public class TouchController : MonoBehaviour
    {
        [Space]
        public bool isOn = true;
        
        [Header("Info")]
        public bool touchSomething = false;
        [HideInInspector]
        public TouchInfo current = default;
        [HideInInspector]
        public TouchInfo last = default;
        
        [Header("Settings")]
        [Min(0f)]
        public float maxDistance = 2f;
        
        [Space]
        public UnityEvent<TouchInfo> onTouchChanged = new UnityEvent<TouchInfo>();
        
        [Header("References")]
        public VisionController vision = null;
        [SerializeField] private Transform _origin = null;

        public Transform origin
        {
            get => _origin != null ? _origin : (_origin = vision.eyes);
            set => _origin = value;
        }
        
        private void UpdateTouch()
        {
            last = current;
            current = new TouchInfo()
            {
                inTouch = vision.seeSomething && Vector3.Distance(origin.position, vision.current.point) <= maxDistance,
                hit = vision.current,
            };

            if (last.hit.collider != current.hit.collider) // looking if was changed visible object or not
            {
                if (last.inTouch) // if last was in touch...
                {
                    last.inTouch = false; // ... make sure that already is not...
                    onTouchChanged.Invoke(last); // ... and invoke!
                }
                
                if (current.inTouch) // if new visible object is in touch... 
                {
                    onTouchChanged.Invoke(current); // ... so invoke it!
                }
            }
            else // if visible object was not changed...
            {
                if (last.inTouch != current.inTouch) // ... but it changed its touch status...
                {
                    onTouchChanged.Invoke(current); // ... invoke it!
                }
            }

            touchSomething = current.inTouch;
        }
        
        private void FixedUpdate()
        {
            if (isOn) UpdateTouch();
        }

        private void OnDrawGizmos()
        {
            if (touchSomething)
            {
                var bounds = current.hit.collider.bounds;
                Gizmos.color = Color.yellow;
                CucuGizmos.DrawWireCube(bounds.center, bounds.size);
            }
            else
            {
                if (vision != null)
                {
                    if (vision.seeSomething)
                    {
                        var bounds = vision.current.collider.bounds;
                        Gizmos.color = Color.red;
                        CucuGizmos.DrawWireCube(bounds.center, bounds.size);
                    }
                }
            }
        }
    }

    [Serializable]
    public struct TouchInfo
    {
        public bool inTouch;
        public RaycastHit hit;
    }
}