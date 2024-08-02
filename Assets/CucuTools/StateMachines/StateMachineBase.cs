using System;
using UnityEngine;

namespace CucuTools.StateMachines
{
    /// <summary>
    /// State Machine
    /// </summary>
    public abstract class StateMachineBase : StateBase
    {
        #region SerializeField

        [Header("State Machine")]
        [SerializeField] private bool _actAsStateMachine = true;
        [SerializeField] private bool _startStateMachineOnStart = true;
        [SerializeField] private StateBase entryState = null;
        
        #endregion

        #region Public API
        
        /// <summary>
        /// Current State which is Active
        /// </summary>
        public StateBase ActiveState => SubState;
        
        /// <summary>
        /// Previous State which was Active
        /// </summary>
        public StateBase PreviousState { get; private set; }
        
        /// <summary>
        /// State which will be started when Current State Machine starts
        /// </summary>
        public StateBase EntryState
        {
            get => entryState;
            set => entryState = value;
        }

        /// <summary>
        /// Execute State Machine role or not.
        /// If true - it will call <see cref="StateBase.UpdateState"/> in own <see cref="Update"/>
        /// </summary>
        public bool actAsStateMachine
        {
            get => _actAsStateMachine;
            set => _actAsStateMachine = value;
        }

        /// <summary>
        /// Will be State Machine started in <see cref="Start"/> or not?
        /// It is possible if only <see cref="actAsStateMachine"/> is true
        /// </summary>
        public bool startStateMachineOnStart
        {
            get => _startStateMachineOnStart;
            set => _startStateMachineOnStart = value;
        }
        
        /// <summary>
        /// Event of State Machine. Invokes when states change.
        /// </summary>
        public event Action<StateBase> OnStateChanged;
        
        #endregion

        #region StateBase

        /// <inheritdoc />
        protected sealed override void OnStartState()
        {
            OnStartStateMachine();
            
            SetNextState(EntryState);
        }

        /// <inheritdoc />
        protected sealed override void OnUpdateState(float deltaTime)
        {
            if (ActiveState && ActiveState.isDone)
            {
                var nextState = GetNextState();

                SetNextState(nextState);
            }

            if (isActive)
            {
                OnUpdateStateMachine(deltaTime);
            }
        }

        /// <inheritdoc />
        protected sealed override void OnStopState()
        {
            OnStopStateMachine();
        }
        
        #endregion

        #region State Machine
        
        /// <summary>
        /// Set next Active State. It is possible if only State Machine is Active.
        /// If <paramref name="nextState"/> is null, State Machine will be stopped.
        /// </summary>
        /// <param name="nextState"></param>
        public void SetNextState(StateBase nextState)
        {
            if (!isActive) return;
            
            if (nextState == this)
            {
                Debug.LogWarning($"\"{name}\" has tried to set itself as Active State. It's impossible!");
                
                return;
            }

            PreviousState = ActiveState;
            SetSubState(nextState);
            
            if (ActiveState)
            {
                OnNextState();
                OnStateChanged?.Invoke(ActiveState);
            }
            else
            {
                OnStateChanged?.Invoke(ActiveState);
                StopState();
            }
        }

        /// <summary>
        /// Get next possible State
        /// </summary>
        /// <returns></returns>
        public abstract StateBase GetNextState();
        
        /// <summary>
        /// It is called after <see cref="SetNextState"/> and if only the Next State is NOT null.
        /// </summary>
        protected virtual void OnNextState()
        {
        }

        /// <summary>
        /// It is called in <see cref="OnStartState"/> before <see cref="SetNextState"/> call.
        /// The best place to set <see cref="EntryState"/> 
        /// </summary>
        protected virtual void OnStartStateMachine()
        {
        }
        
        /// <summary>
        /// It is called in <see cref="OnUpdateState"/> after that <see cref="ActiveState"/> is checked
        /// and State Machine is still Active
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void OnUpdateStateMachine(float deltaTime)
        {
        }
        
        /// <summary>
        /// It is called in <see cref="OnStopState"/>
        /// </summary>
        protected virtual void OnStopStateMachine()
        {
        }
        
        #endregion

        #region MonoBehaviour
        
        private void Start()
        {
            if (actAsStateMachine && startStateMachineOnStart)
            {
                StartState();   
            }
        }
        
        private void Update()
        {
            if (actAsStateMachine && isActive)
            {
                if (isDone)
                {
                    StopState();
                }
                else
                {
                    UpdateState(Time.deltaTime);
                }
            }
        }

        #endregion
    }

    /// <inheritdoc cref="CucuTools.StateMachines.StateMachineBase" />
    public abstract class StateMachineBase<T> : StateMachineBase, IContextHolder<T>
    {
        [Header("Context")]
        [SerializeField] private T context = default;

        /// <inheritdoc />
        public T Context => context;

        /// <inheritdoc />
        public void SetContext(T newContext)
        {
            context = newContext;

            OnSetupContext();
        }

        /// <summary>
        /// It is called after <see cref="SetContext"/>
        /// </summary>
        protected virtual void OnSetupContext()
        {
        }
    }
}