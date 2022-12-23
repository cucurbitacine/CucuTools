using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines
{
    /// <summary>
    /// Trigger which monitors state's change 
    /// </summary>
    [AddComponentMenu(Cucu.AddComponent + Cucu.StateMachineGroup + ComponentName, 1)]
    public class TriggerState : CucuBehaviour
    {
        private const string ComponentName = "Trigger State";

        #region SerializeField

        [SerializeField] private UnityEvent triggerEvent = new UnityEvent();

        [Header("Settings")]
        [SerializeField] private StateBase state = null;
        [SerializeField] private EventInvokeMode triggerMode = EventInvokeMode.OnEnter;

        #endregion

        #region Public API

        public UnityEvent Event => triggerEvent ??= new UnityEvent();

        public StateBase State
        {
            get => state;
            private set => state = value;
        }

        public EventInvokeMode TriggerMode
        {
            get => triggerMode;
            set => triggerMode = value;
        }

        public void Invoke()
        {
            Event.Invoke();
        }

        #endregion

        #region Private API

        private void OnStateEnter()
        {
            if (TriggerMode == EventInvokeMode.OnEnter) Invoke();
        }

        private void OnStateExit()
        {
            if (TriggerMode == EventInvokeMode.OnExit) Invoke();
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (State == null) State = GetComponentInParent<StateBase>();
        }

        private void OnEnable()
        {
            if (State != null)
            {
                State.Events.onEnter.AddListener(OnStateEnter);
                State.Events.onExit.AddListener(OnStateExit);
            }
        }

        private void OnDisable()
        {
            if (State != null)
            {
                State.Events.onEnter.RemoveListener(OnStateEnter);
                State.Events.onExit.RemoveListener(OnStateExit);
            }
        }

        #endregion

        public enum EventInvokeMode
        {
            OnEnter,
            OnExit,
        }
    }
}