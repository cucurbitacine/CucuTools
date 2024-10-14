using System;
using UnityEngine;

namespace CucuTools.StateMachines
{
    [DisallowMultipleComponent]
    public class StateBase : MonoBehaviour, IStateMachine
    {
        public event Action<bool> Done;
        public event Action<bool> ExecutionUpdated;
        
        [field: SerializeField] public bool IsRunning { get; private set; }
        
        [field: Space]
        [field: SerializeField] public bool IsDone { get; private set; }
        [field: SerializeField] public bool UndoneOnEnter { get; set; } = true;
        
        public IState SubState { get; private set; }

        public float unscaledTime => Time.unscaledTime - _enterUnscaledTime;
        public float time => Time.time - _enterTime;

        private float _enterUnscaledTime;
        private float _enterTime;
        
        public void SetDone(bool value)
        {
            if (IsDone == value) return;
            
            IsDone = value;
            
            Done?.Invoke(IsDone);
        }
        
        public void Enter()
        {
            if (IsRunning) return;

            if (UndoneOnEnter)
            {
                SetDone(false);
            }

            _enterUnscaledTime = Time.unscaledTime;
            _enterTime = Time.time;
            
            OnEnter();
            
            IsRunning = true;
            
            SubState?.Enter();
            
            ExecutionUpdated?.Invoke(true);
        }

        public void Execute()
        {
            if (IsRunning)
            {
                OnExecute();
                
                SubState?.Execute();
            }
        }

        public void Exit()
        {
            if (!IsRunning) return;

            SubState?.Exit();

            OnExit();
            
            IsRunning = false;
            
            ExecutionUpdated?.Invoke(false);
        }

        public void SetSubState(IState state)
        {
            if (IsRunning)
            {
                SubState?.Exit();

                SubState = state;
                
                SubState?.Enter();
            }
            else
            {
                SubState = state;
            }

            OnSetSubState();
        }
        
        protected virtual void OnEnter()
        {
        }
        
        protected virtual void OnExecute()
        {
        }
        
        protected virtual void OnExit()
        {
        }

        protected virtual void OnSetSubState()
        {
        }
    }
    
    /// <inheritdoc cref="StateBase" />
    public abstract class StateBase<T> : StateBase, IContextHolder<T>
    {
        [Header("Context")]
        [SerializeField] private T context = default;

        /// <inheritdoc />
        public T Context => context;
        
        /// <inheritdoc />
        public void SetContext(T newContext)
        {
            context = newContext;

            OnSetContext();
        }

        /// <summary>
        /// It is called after <see cref="SetContext"/>
        /// </summary>
        protected virtual void OnSetContext()
        {
        }
    }
}