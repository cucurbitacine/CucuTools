using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines.Utils
{
    /// <summary>
    /// State Machine Events Invoker
    /// </summary>
    public sealed class StateMachineEvent : MonoBehaviour
    {
        [SerializeField] private StateMachineBase stateMachine;
        
        [Space]
        [SerializeField] private UnityEvent<StateBase> onStateChanged = new UnityEvent<StateBase>();
        
        private void HandleStateChange(StateBase state)
        {
            onStateChanged.Invoke(state);
        }

        private void Initialize()
        {
            if (stateMachine == null) stateMachine = GetComponent<StateMachineBase>();
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
            if (stateMachine)
            {
                stateMachine.OnStateChanged += HandleStateChange;
            }
        }
        
        private void OnDisable()
        {
            if (stateMachine)
            {
                stateMachine.OnStateChanged -= HandleStateChange;
            }
        }
    }
}