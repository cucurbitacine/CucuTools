using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Interactables
{
    /// <summary>
    /// Extended Base realisation of Interactable object
    /// </summary>
    [AddComponentMenu(Cucu.AddComponent + Cucu.InteractableGroup + ObjectName, 0)]
    public class CucuInteractableExtended : CucuInteractableEntity
    {
        public new const string ObjectName = "Interactable Extended";
        
        [Space]
        [SerializeField] private TransitEvents _transitEvents;

        public TransitEvents TransitEvents => _transitEvents ?? (_transitEvents = new TransitEvents());
        
        private bool wasIdle;
        private bool wasHover;
        private bool wasPress;

        private void HandleEvents()
        {
            if (wasIdle)
            {
                if (InteractInfo.Hover) TransitEvents.OnIdleToHover.Invoke();
            }

            if (wasHover)
            {
                if (InteractInfo.Idle) TransitEvents.OnHoverToIdle.Invoke();
                if (InteractInfo.Press) TransitEvents.OnHoverToPress.Invoke();
            }

            if (wasPress)
            {
                if (InteractInfo.Hover) TransitEvents.OnPressToHover.Invoke();
            }
            
            wasIdle = InteractInfo.Idle;
            wasHover = InteractInfo.Hover;
            wasPress = InteractInfo.Press;
        }

        protected void Update()
        {
            if (IsEnabled) HandleEvents();
        }
    }

    [Serializable]
    public class TransitEvents
    {
        [Header("Idle")]
        [SerializeField] private UnityEvent onIdleToHover;
        [Header("Hover")]
        [SerializeField] private UnityEvent onHoverToIdle;
        [SerializeField] private UnityEvent onHoverToPress;
        [Header("Press")]
        [SerializeField] private UnityEvent onPressToHover;

        public UnityEvent OnPressToHover => onPressToHover ?? (onPressToHover = new UnityEvent());
        public UnityEvent OnHoverToPress => onHoverToPress ?? (onHoverToPress = new UnityEvent());
        public UnityEvent OnHoverToIdle => onHoverToIdle ?? (onHoverToIdle = new UnityEvent());
        public UnityEvent OnIdleToHover => onIdleToHover ?? (onIdleToHover = new UnityEvent());
    }
}