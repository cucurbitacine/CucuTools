using System;
using System.Linq;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.StateMachines.Impl
{
    /// <inheritdoc />
    [AddComponentMenu(Cucu.AddComponent + Cucu.StateMachineGroup + ComponentName, 1)]
    public class StateMachine : StateMachineBase
    {
        private const string ComponentName = "State Machine";
        
        private TransitionBase[] _transitions = null;

        #region SerializeField

        [Header("State Machine Info")]
        [SerializeField] private bool deadStateMachine = false;
        [SerializeField] private StateBase currentState = null;
        
        [Header("Settings")]
        [SerializeField] private bool launchOnStart = false;
        [SerializeField] private StateBase initialState = null;
        [SerializeField] private ConditionContext conditionContext = null;
        
        [Space]
        [SerializeField] private bool debugLog = true;

        #endregion

        #region Public API

        public StateBase InitialState
        {
            get => initialState;
            set => initialState = value;
        }
        
        public StateBase CurrentState
        {
            get => currentState;
            private set => currentState = value;
        }
        
        public bool LaunchOnStart
        {
            get => launchOnStart;
            set => launchOnStart = value;
        }

        public ConditionContext ConditionContext
        {
            get => conditionContext;
            set => conditionContext = value;
        }

        #endregion
        
        #region StateBase

        /// <inheritdoc />
        public override bool Dead => deadStateMachine;

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
            Log($"Launched");
            
            IsRunning = true;

            CurrentState = InitialState;
            
            Events.onEnter.Invoke();
            
            Log($"Enter to <{CurrentState.Key}>");
            
            CurrentState.Launch();
        }

        /// <inheritdoc />
        public override void Stop()
        {
            IsRunning = false;
            
            Log($"Exit from <{CurrentState.Key}>");
            
            CurrentState.Stop();
            
            Events.onExit.Invoke();
            
            Log($"Stopped");
        }

        /// <inheritdoc />
        public override TransitionBase[] GetTransitions()
        {
            if (_transitions == null || _transitions.Length == 0)
            {
                _transitions = GetComponentsInChildren<TransitionBase>()
                    .Where(t => t.transform.parent == transform).ToArray();
            }
            
            return _transitions;
        }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                Key = $"{nameof(StateMachine)}-{Guid.NewGuid()}";
            }

            _transitions = GetTransitions();
            
            deadStateMachine = _transitions.Length == 0;
            
            InitialState = GetInitialState();
            
            UpdateStateMachineName();
        }

        #endregion

        #region StateMachineBase

        /// <inheritdoc />
        public override StateBase GetInitialState()
        {
            if (InitialState == null)
            {
                return InitialState = GetComponentsInChildren<StateBase>()
                    .Where(s => s.transform.parent == transform)
                    .Select(s => (state: s, siblingIndexs: s.transform.GetSiblingIndex()))
                    .OrderBy(p => p.siblingIndexs)
                    .FirstOrDefault().state;
            }
            
            return InitialState;
        }
        
        /// <inheritdoc />
        public override bool GetCondition(string key)
        {
            return ConditionContext.GetOrCreate(key);
        }

        /// <inheritdoc />
        public override void SetCondition(string key, bool value)
        {
            ConditionContext.SetOrCreate(key, value);
        }

        #endregion

        #region Private API

        private void UpdateStateMachine()
        {
            if (!CurrentState.Dead && CurrentState.NextState(out var nextState))
            {
                Log($"Exit from <{CurrentState.Key}>");
                
                CurrentState.Stop();
                
                Log($"Transition to <{nextState.Key}> from <{CurrentState.Key}>");
                
                CurrentState = nextState;
                
                Log($"Enter to <{CurrentState.Key}>");
                
                CurrentState.Launch();
            }
        }

        private void Log(string msg)
        {
            if (debugLog)
            {
                Debug.Log($"[{Key}] :: {msg}");
            }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            Validate();
            
            if (ConditionContext == null)
            {
                ConditionContext = ScriptableObject.CreateInstance<ConditionContext>();
            }
        }

        private void Start()
        {
            if (LaunchOnStart) Launch();
        }

        private void Update()
        {
            if (IsRunning) UpdateStateMachine();
        }

        private void OnValidate()
        {
            Validate();
        }

        #endregion

        [DrawButton("Launch", group:"Control")]
        private void LaunchStateMachine()
        {
            Launch();
        }
        
        [DrawButton("Stop", group:"Control")]
        private void StopStateMachine()
        {
            Stop();
        }
        
        [DrawButton("Update Name", group:"State Machine")]
        private void UpdateStateMachineName()
        {
            gameObject.name = $"[ {Key} ]";
        }
        
        [DrawButton("new State", group:"Create")]
        private void AddState()
        {
            var number = GetComponentsInChildren<State>().Length;

            var state = new GameObject($"{nameof(State)} {number}").AddComponent<State>();
            state.transform.SetParent(transform);
            state.Validate();
        }

        [DrawButton("new Transition", group:"Create")]
        private void AddTransition()
        {
            var number = GetComponentsInChildren<Transition>().Length;

            var transition = new GameObject($"{nameof(Transition)} {number}").AddComponent<Transition>();
            transition.transform.SetParent(transform);
            transition.Validate();
        }
    }
}