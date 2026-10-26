using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.NodeSystem
{
    public class ActionState : MonoBehaviour
    {
        [SerializeField] private StateNode _state;
        [SerializeField] private StateEventType _stateEventType;
        [Space]
        [SerializeField] private UnityEvent _onStateEvent = new UnityEvent();

        public StateEventType GetStateEventType()
        {
            return _stateEventType;
        }
        
        private void OnStateEventInvoke()
        {
            _onStateEvent.Invoke();
        }
        
        private void OnStateEventInvoke(StateEventType stateEventType)
        {
            if (GetStateEventType() == stateEventType)
            {
                OnStateEventInvoke();
            }
        }

        private void Awake()
        {
            if (_state == null) _state = GetComponentInParent<StateNode>();
        }

        private void OnEnable()
        {
            _state.OnStateEvent += OnStateEventInvoke;
        }

        private void OnDisable()
        {
            _state.OnStateEvent -= OnStateEventInvoke;
        }
    }
}