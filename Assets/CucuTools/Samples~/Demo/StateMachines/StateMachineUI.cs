using System;
using CucuTools;
using CucuTools.StateMachines;
using UnityEngine.Events;

namespace Samples.Demo.StateMachines
{
    public class StateMachineUI : CucuBehaviour
    {
        public StateMachineBase stateMachine;

        public UnityEvent<string> onStateNameChanged = new UnityEvent<string>();

        private string lastStateName = string.Empty;
        
        public string GetStateName(StateBase state)
        {
            if (state == null) return "[-]";

            return state.GetType().Name.Replace("state", "", StringComparison.OrdinalIgnoreCase);
        }
        
        public string GetFullStateName(StateBase state)
        {
            if (state == null) GetStateName(state);
            
            if (state.subState)
            {
                return $"{GetStateName(state)} > {GetFullStateName(state.subState)}";
            }
            
            return GetStateName(state);
        }

        private void CheckStateName(string stateNameText)
        {
            if (!stateNameText.Equals(lastStateName))
            {
                lastStateName = stateNameText;
                
                onStateNameChanged.Invoke(stateNameText);
            }
        }

        private void CheckStateName()
        {
            if (stateMachine && stateMachine.isActive)
            {
                var stateNameText = GetFullStateName(stateMachine.activeState);
                
                CheckStateName(stateNameText);
            }
        }

        private void Update()
        {
            CheckStateName();
        }
    }
}