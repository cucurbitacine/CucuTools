using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.StateMachines.Impl
{
    public class StateInitializer : CucuBehaviour
    {
        public bool initializeOnStart = true;
        
        [Space]
        public StateBase state;
        public StateMachineBase stateMachine;

        public const string InitializeMethodName = "Initialize";

        [DrawButton]
        public void Initialize()
        {
            if (state == null) state = GetComponent<StateBase>();
            if (stateMachine == null) stateMachine = GetComponentInParent<StateMachineBase>();

            if (state && stateMachine && state != stateMachine)
            {
                var initialize = state.GetType().GetMethod(InitializeMethodName);
                if (initialize != null)
                {
                    var parameters = initialize.GetParameters();
                    if (parameters.Length == 1)
                    {
                        if (parameters[0].ParameterType.IsInstanceOfType(stateMachine))
                        {
                            initialize.Invoke(state, new object[] { stateMachine });
                        }
                    }
                }
            }
        }

        private void Start()
        {
            if (initializeOnStart)
            {
                Initialize();
            }
        }
    }
}