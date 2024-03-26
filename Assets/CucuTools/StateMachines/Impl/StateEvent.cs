using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines.Impl
{
    public class StateEvent : CucuBehaviour
    {
        public StateEventType eventType;

        [Space]
        public UnityEvent onUpdated = new UnityEvent();
        
        [Space]
        public StateBase state;

        public void Invoke(StateEventType stateEventType)
        {
            if (eventType == stateEventType)
            {
                onUpdated.Invoke();
            }
        }

        private void Awake()
        {
            if (state == null) state = GetComponent<StateBase>();
        }

        private void OnEnable()
        {
            if (state)
            {
                state.onUpdated.AddListener(Invoke);
            }
        }
        
        private void OnDisable()
        {
            if (state)
            {
                state.onUpdated.RemoveListener(Invoke);
            }
        }
    }
}