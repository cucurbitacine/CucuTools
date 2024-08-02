using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines.Utils
{
    /// <summary>
    /// State Events Invoker
    /// </summary>
    public sealed class StateEvent : MonoBehaviour
    {
        [SerializeField] private StateBase state;

        [Space]
        [SerializeField] private StateEventType eventType = StateEventType.Start;

        [Space]
        [SerializeField] private UnityEvent onStateUpdated = new UnityEvent();
        
        private void HandleStateUpdate(StateEventType stateEvent)
        {
            if (eventType == stateEvent)
            {
                onStateUpdated.Invoke();
            }
        }

        private void Initialize()
        {
            if (state == null) state = GetComponent<StateBase>();
        }
        
        private void Awake()
        {
            Initialize();
        }

        private void OnValidate()
        {
            Initialize();
        }

        private void OnEnable()
        {
            if (state)
            {
                state.OnStateUpdated += HandleStateUpdate;
            }
        }
        
        private void OnDisable()
        {
            if (state)
            {
                state.OnStateUpdated -= HandleStateUpdate;
            }
        }
    }
}