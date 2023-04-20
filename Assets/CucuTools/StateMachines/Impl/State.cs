using System;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.StateMachines.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(Cucu.AddComponent + Cucu.StateMachineGroup + ComponentName, 1)]
    public class State : StateBase
    {
        private const string ComponentName = "State";
        
        private TransitionBase[] _transitions = null;

        #region SerializeField

        [Space]
        [SerializeField] private bool deadState = false;

        #endregion

        #region StateBase

        /// <inheritdoc />
        public override bool Dead => deadState;

        /// <inheritdoc />
        public override bool NextState(out StateBase state)
        {
            state = null;

            foreach (var transition in _transitions)
            {
                if (transition.NextState(out state))
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <inheritdoc />
        public override void Launch()
        {
            IsRunning = true;
            Events.onEnter.Invoke();
        }

        /// <inheritdoc />
        public override void Stop()
        {
            IsRunning = false;
            Events.onExit.Invoke();
        }

        /// <inheritdoc />
        public override TransitionBase[] GetTransitions()
        {
            if (_transitions == null || _transitions.Length == 0)
            {
                return _transitions = GetComponentsInChildren<TransitionBase>();
            }
            
            return _transitions;
        }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                Key = $"{nameof(State)}-{Guid.NewGuid()}";
            }
            
            _transitions = GetTransitions();

            deadState = _transitions.Length == 0;
            
            UpdateStateName();
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            Validate();
        }

        private void OnValidate()
        {
            Validate();
        }

        #endregion

        [DrawButton("Update Name", group:"State")]
        private void UpdateStateName()
        {
#if UNITY_EDITOR
            gameObject.name = $"< {Key} >";
#endif
        }
        
        [DrawButton("new Transition", group:"Create")]
        private void AddTransition()
        {
            var number = GetComponentsInChildren<Transition>().Length;

            var transition = new GameObject($"{nameof(Transition)} {number}").AddComponent<Transition>();
            transition.transform.SetParent(transform);
            transition.Key = $"{nameof(Transition)} {transition.transform.GetSiblingIndex()}";
            transition.Validate();
        }
    }
}