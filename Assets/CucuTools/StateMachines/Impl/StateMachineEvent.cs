using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines.Impl
{
    public class StateMachineEvent : CucuBehaviour
    {
        public UnityEvent<StateBase> onStateChanged = new UnityEvent<StateBase>();
        
        [Space]
        public StateMachineBase stateMachine;

        public void Invoke(StateBase state)
        {
            onStateChanged.Invoke(state);
        }
        
        private void Awake()
        {
            if (stateMachine == null) stateMachine = GetComponent<StateMachineBase>();
        }

        private void OnEnable()
        {
            if (stateMachine)
            {
                stateMachine.onStateChanged.AddListener(Invoke);
            }
        }
        
        private void OnDisable()
        {
            if (stateMachine)
            {
                stateMachine.onStateChanged.RemoveListener(Invoke);
            }
        }
    }
}