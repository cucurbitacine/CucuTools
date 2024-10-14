using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines.Utils
{
    /// <summary>
    /// State Events Invoker
    /// </summary>
    public sealed class StateEvent : MonoBehaviour
    {
        public enum StateEventType
        {
            Enter,
            Exit,
        }
        
        [SerializeField] private GameObject state;

        [Space]
        [SerializeField] private StateEventType eventType = StateEventType.Enter;

        [Space]
        [SerializeField] private UnityEvent onStateUpdated = new UnityEvent();

        private IState _state;
        
        private void OnExecutionUpdated(bool isExecuting)
        {
            if (isExecuting && (eventType == StateEventType.Enter))
            {
                onStateUpdated.Invoke();
            }
            
            if (!isExecuting && (eventType == StateEventType.Exit))
            {
                onStateUpdated.Invoke();
            }
        }
        
        private void Awake()
        {
            TryGetComponent(out _state);
        }

        private void OnEnable()
        {
            _state.ExecutionUpdated += OnExecutionUpdated;

            if (_state.IsRunning) OnExecutionUpdated(true);
        }
        
        private void OnDisable()
        {
            _state.ExecutionUpdated -= OnExecutionUpdated;
        }
    }
}