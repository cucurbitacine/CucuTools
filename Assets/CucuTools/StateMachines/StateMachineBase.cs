using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines
{
    public abstract class StateMachineBase : StateBase
    {
        #region SerializeField

        [Header("State Machine")]
        public bool isStateMachine = true;
        [Tooltip("Will be used only if it's a state machine")]
        public bool enterOnStart = true;
        
        [Space]
        [SerializeField] private StateBase _activeState;
        
        #endregion

        #region Public API
        
        public StateBase activeState => subState;
        
        public StateBase lastState { get; private set; }
        
        public UnityEvent<StateBase> onStateChanged { get; } = new UnityEvent<StateBase>();

        public bool isState
        {
            get => !isStateMachine;
            set => isStateMachine = !value;
        }
        
        #endregion

        #region StateBase

        protected override void OnEnter()
        {
            var entryState = GetEntryState();
            
            SetState(entryState);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (activeState && activeState.isDone)
            {
                var nextState = GetNextState();

                SetState(nextState);
            }
        }

        protected override void OnExit()
        {
            SetSubState(null);
        }

        #endregion

        #region State Machine
        
        public void SetState(StateBase state)
        {
            if (state == this)
            {
                Debug.LogWarning($"\"{name}\" has tried to set itself as active state.");
                
                return;
            }

            lastState = activeState;
            
            SetSubState(state);

            _activeState = state;
            
            if (state)
            {
                OnStateChange();
                
                onStateChanged.Invoke(state);
            }
            else
            {
                isDone = true;
            }
        }

        public abstract StateBase GetEntryState();
        public abstract StateBase GetNextState();

        protected virtual void OnStart()
        {
        }
        
        protected virtual void OnStateChange()
        {
        }
        
        #endregion

        #region MonoBehaviour

        private void Start()
        {
            if (isStateMachine && enterOnStart)
            {
                Enter();   
            }

            OnStart();
        }
        
        private void Update()
        {
            if (isActive && isStateMachine)
            {
                if (isDone)
                {
                    Exit();
                }
                else
                {
                    UpdateState(Time.deltaTime);
                }
            }
        }

        #endregion
    }
}