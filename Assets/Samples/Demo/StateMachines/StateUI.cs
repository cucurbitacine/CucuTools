using System;
using CucuTools.StateMachines;
using UnityEngine;
using UnityEngine.Events;

namespace Samples.Demo.StateMachines
{
    public class StateUI : MonoBehaviour
    {
        [SerializeField] private GameObject stateSource;
        
        [SerializeField] private UnityEvent<string> onStateNameChanged = new UnityEvent<string>();

        private string lastStateName = string.Empty;
        
        private IState _state;
        
        public string GetStateName(IState state)
        {
            if (state == null) return "[-]";

            return state.GetType().Name.Replace("state", "", StringComparison.OrdinalIgnoreCase);
        }
        
        public string GetFullStateName(IState state)
        {
            if (state == null) GetStateName(null);
            
            if (state is IStateMachine stateMachine)
            {
                return $"{GetStateName(state)} > {GetFullStateName(stateMachine.SubState)}";
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
            if (_state != null && _state.IsRunning)
            {
                var stateNameText = GetFullStateName(_state);
                
                CheckStateName(stateNameText);
            }
        }

        private void Awake()
        {
            stateSource.TryGetComponent(out _state);
        }

        private void Update()
        {
            CheckStateName();
        }
    }
}