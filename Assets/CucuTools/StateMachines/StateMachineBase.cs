using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines
{
    public abstract class StateMachineBase : StateBase
    {
        #region SerializeField

        [Header("State Machine")]
        [SerializeField] private StateBase _activeState;

        [Space]
        public bool autoDone = false;
        public bool isRoleState = false;
        
        [Space]
        [Tooltip("Will be used only if it's not a state role")]
        public bool reloadOnEnable = false;
        [Tooltip("Will be used only if it's not a state role")]
        public bool startOnStart = false;
        
        [Space]
        [SerializeField] private StateBase _entryState;
        
        #endregion

        #region Public API

        public StateBase activeState
        {
            get => _activeState;
            private set => _activeState = value;
        }

        public StateBase entryState
        {
            get => _entryState;
            set => _entryState = value;
        }

        public StateBase lastState { get; private set; }
        
        public UnityEvent<StateBase> onStateChanged { get; } = new UnityEvent<StateBase>();
        
        public bool isRoleStateMachine
        {
            get => !isRoleState;
            set => isRoleState = !value;
        }
        
        #endregion

        #region StateBase

        protected override void OnEnter()
        {
            activeState = entryState;

            if (activeState)
            {
                activeState.Enter();
            
                onStateChanged.Invoke(activeState);
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (activeState && activeState.isDone)
            {
                NextState();
            }

            if (activeState)
            {
                activeState.UpdateState(deltaTime);
            }
        }

        protected override void OnExit()
        {
            if (activeState)
            {
                activeState.Exit();   
            }

            activeState = null;
        }

        #endregion

        #region State Machine

        public void NextState()
        {
            var nextState = GetNextState();
            
            NextState(nextState);
        }
        
        public void NextState(StateBase state)
        {
            if (state == this) return;
            
            lastState = activeState;
            
            if (activeState)
            {
                activeState.Exit();
            }
            
            activeState = state;

            if (activeState)
            {
                activeState.Enter();
            
                onStateChanged.Invoke(activeState);
            }

            if (autoDone)
            {
                if (activeState == null)
                {
                    isDone = true;
                }
            }
            
            OnStateChange();
        }

        public abstract StateBase GetNextState();

        protected virtual void OnStateChange()
        {
        }
        
        #endregion

        #region MonoBehaviour

        protected virtual void OnEnable()
        {
            if (isRoleStateMachine && reloadOnEnable && isActive)
            {
                Exit();
                
                Enter();
            }
        }
        
        protected virtual void Start()
        {
            if (isRoleStateMachine && startOnStart)
            {
                Enter();   
            }
        }
        
        protected virtual void Update()
        {
            if (isRoleStateMachine && !isDone)
            {
                UpdateState(Time.deltaTime);
            }
        }

        #endregion
    }
}