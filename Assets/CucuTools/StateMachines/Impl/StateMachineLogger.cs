using UnityEngine;

namespace CucuTools.StateMachines.Impl
{
    public class StateMachineLogger : CucuBehaviour
    {
        public bool paused = false;
        public string customName = string.Empty;

        [Space] public StateMachineBase stateMachine;

        private const string DefaultStateMachineName = "???";
        private const string DefaultStateName = "-";
        
        private StateBase lastState { get; set; }

        private string GetNameStateMachine()
        {
            var stateMachineName = customName;

            if (string.IsNullOrWhiteSpace(customName))
            {
                stateMachineName = stateMachine ? stateMachine.name : DefaultStateMachineName;
            }

            return $"\"{stateMachineName}\"";
        }

        private string GetNameState(StateBase state)
        {
            var stateName = state ? state.name : DefaultStateName;
            
            return $"\"{stateName}\"";
        }
        
        private string GetNameLastState()
        {
            return GetNameState(lastState);
        }

        private void StateUpdated(StateEventType eventType)
        {
            if (eventType != StateEventType.Update)
            {
                if (!paused)
                {
                    Debug.Log($"{GetNameStateMachine()} :: {eventType} event");
                }
            }
        }

        private void StateChanged(StateBase nextState)
        {
            if (stateMachine)
            {
                if (!paused)
                {
                    Debug.Log($"{GetNameStateMachine()} :: {GetNameLastState()} -> {GetNameState(nextState)}");
                }

                lastState = nextState;
            }
        }

        private void Validate()
        {
            if (stateMachine == null) stateMachine = GetComponent<StateMachineBase>();
        }

        private void Awake()
        {
            Validate();
        }

        private void OnEnable()
        {
            if (stateMachine)
            {
                stateMachine.onUpdated.AddListener(StateUpdated);
                stateMachine.onStateChanged.AddListener(StateChanged);
            }
        }

        private void OnDisable()
        {
            if (stateMachine)
            {
                stateMachine.onUpdated.RemoveListener(StateUpdated);
                stateMachine.onStateChanged.RemoveListener(StateChanged);
            }
        }

        private void OnValidate()
        {
            Validate();
        }
    }
}