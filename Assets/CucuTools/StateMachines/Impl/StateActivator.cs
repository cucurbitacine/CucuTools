using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.StateMachines.Impl
{
    public class StateActivator : CucuBehaviour
    {
        public StateBase state;
        
        [Space]
        public StateMachineBase stateMachine;

        [DrawButton]
        public void Activate()
        {
            if (state && stateMachine && state != stateMachine)
            {
                stateMachine.NextState(state);
            }
        }
        
        private void Validate()
        {
            if (state == null) state = GetComponent<StateBase>();
            if (stateMachine == null) stateMachine = GetComponentInParent<StateMachineBase>();
        }
        
        private void Awake()
        {
            Validate();
        }

        private void OnValidate()
        {
            Validate();
        }
    }
}