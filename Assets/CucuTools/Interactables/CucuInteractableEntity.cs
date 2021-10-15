using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Interactables
{
    /// <summary>
    /// Base realisation of Interactable behavior
    /// </summary>
    [AddComponentMenu(Cucu.AddComponent + Cucu.InteractableGroup + ObjectName, 0)]
    public class CucuInteractableEntity : CucuInteractable
    {
        #region SerializeField

        [Space]
        [SerializeField] private bool isEnabled = true;
        
        [Space]
        [SerializeField] private InteractInfo _interactInfo;
        [Space]
        [SerializeField] private InteractEvents _interactEvents;

        #endregion

        public const string ObjectName = "Interactable";
        
        public virtual bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        /// <inheritdoc />
        public override InteractInfo InteractInfo => _interactInfo ?? (_interactInfo = new InteractInfo());
        
        public InteractEvents InteractEvents => _interactEvents ?? (_interactEvents = new InteractEvents());
        
        /// <inheritdoc />
        public override void Idle()
        {
            if (!IsEnabled) return;
            
            if (InteractInfo.Idle) return;

            InteractInfo.Idle = true;
            
            if (InteractInfo.Hover)
            {
                InteractInfo.Hover = false;
                InteractEvents.OnHoverCancel.Invoke();
            }
            
            if (InteractInfo.Press)
            {
                InteractInfo.Press = false;
                InteractEvents.OnPressCancel.Invoke();
            }

            InteractEvents.OnIdleStart.Invoke();
        }

        /// <inheritdoc />
        public override void Hover(ICucuContext context)
        {
            if (!IsEnabled) return;
            
            if (InteractInfo.Hover) return;
            
            InteractInfo.Hover = true;
            
            if (InteractInfo.Idle)
            {
                InteractInfo.Idle = false;
                InteractEvents.OnIdleCancel.Invoke();
            }
            
            if (InteractInfo.Press)
            {
                InteractInfo.Press = false;
                InteractEvents.OnPressCancel.Invoke();
            }
            
            InteractEvents.OnHoverStart.Invoke(context);
        }

        /// <inheritdoc />
        public override void Press(ICucuContext context)
        {
            if (!IsEnabled) return;
            
            if (InteractInfo.Press) return;
            
            InteractInfo.Press = true;
            
            if (InteractInfo.Idle)
            {
                InteractInfo.Idle = false;
                InteractEvents.OnIdleCancel.Invoke();
            }
            
            if (InteractInfo.Hover)
            {
                InteractInfo.Hover = false;
                InteractEvents.OnHoverCancel.Invoke();
            }
            
            InteractEvents.OnPressStart.Invoke(context);
        }

        protected virtual void Start()
        {
            if (IsEnabled && InteractInfo.Idle)
            {
                InteractEvents.OnIdleStart.Invoke();
            }
        }
    }

    [Serializable]
    public class InteractEvents
    {
        public UnityEvent OnIdleStart => onIdleStart ?? (onIdleStart = new UnityEvent());

        public UnityEvent OnIdleCancel => onIdleCancel ?? (onIdleCancel = new UnityEvent());

        public UnityEvent<ICucuContext> OnHoverStart => onHoverStart ?? (onHoverStart = new UnityEvent<ICucuContext>());

        public UnityEvent OnHoverCancel => onHoverCancel ?? (onHoverCancel = new UnityEvent());

        public UnityEvent<ICucuContext> OnPressStart => onPressStart ?? (onPressStart = new UnityEvent<ICucuContext>());

        public UnityEvent OnPressCancel => onPressCancel ?? (onPressCancel = new UnityEvent());

        [Header("Idle")]
        [SerializeField] private UnityEvent onIdleStart;
        [SerializeField] private UnityEvent onIdleCancel;
        
        [Header("Hover")]
        [SerializeField] private UnityEvent<ICucuContext> onHoverStart;
        [SerializeField] private UnityEvent onHoverCancel;
        
        [Header("Press")]
        [SerializeField] private UnityEvent<ICucuContext> onPressStart;
        [SerializeField] private UnityEvent onPressCancel;
    }
}
