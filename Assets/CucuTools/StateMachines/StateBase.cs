using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines
{
    [DisallowMultipleComponent]
    public class StateBase : CucuBehaviour
    {
        #region SerializeField

        [SerializeField] private bool _isActive = false;
        
        [Header("State")]
        [SerializeField]
        private bool _isDone = false;
        public bool needReset = true;
        
        #endregion

        #region Public API

        public bool isActive
        {
            get => _isActive;
            private set => _isActive = value;
        }
        
        public bool isDone
        {
            get => _isDone;
            set => _isDone = value;
        }
        
        public UnityEvent<StateEventType> onUpdated { get; } = new UnityEvent<StateEventType>();
        
        public void Enter()
        {
            if (isActive) return;
            
            if (needReset && isDone)
            {
                isDone = false;
            }
            
            isActive = true;
            
            OnEnter();
            
            onUpdated.Invoke(StateEventType.Enter);
        }

        public void UpdateState(float deltaTime)
        {
            if (isActive)
            {
                OnUpdate(deltaTime);
                
                onUpdated.Invoke(StateEventType.Update);
            }
        }

        public void Exit()
        {
            if (isActive)
            {
                isActive = false;

                OnExit();
                
                if (needReset && isDone)
                {
                    isDone = false;
                }
                
                onUpdated.Invoke(StateEventType.Exit);
            }
        }

        public void Toggle()
        {
            isDone = !isDone;
        }
        
        #endregion

        #region Virtual API

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnUpdate(float deltaTime)
        {
        }

        protected virtual void OnExit()
        {
        }

        #endregion
    }

    public enum StateEventType
    {
        Enter,
        Update,
        Exit,
    }

    public abstract class StateBase<TStateMachine> : StateBase where TStateMachine : StateMachineBase
    {
        [Space]
        [SerializeField] private TStateMachine _stateMachine = null;

        public TStateMachine stateMachine
        {
            get => _stateMachine;
            private set => _stateMachine = value;
        }
        
        public void Initialize(TStateMachine sm)
        {
            stateMachine = sm;

            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }
    }
}