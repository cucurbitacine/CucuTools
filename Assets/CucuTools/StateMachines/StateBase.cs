using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines
{
    public class StateBase : CucuBehaviour
    {
        #region SerializeField

        [SerializeField] private bool _isActive = false;
        
        [Header("State")]
        [SerializeField]
        private bool _isDone = false;
        public bool needReset = true;
        
        #endregion

        private float _enterTime;
        private float _enterUnscaledTime;
        
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

        public float time => Time.time - _enterTime;
        public float unscaledTime => Time.unscaledTime - _enterTime;
        
        public StateBase subState { get; private set; }
        
        public UnityEvent<StateEventType> onUpdated { get; } = new UnityEvent<StateEventType>();
        
        public void Enter()
        {
            if (isActive) return;
            
            isActive = true;
            
            if (needReset && isDone)
            {
                isDone = false;
            }

            _enterTime = Time.time;
            _enterUnscaledTime = Time.unscaledTime;
            
            OnEnter();
            
            onUpdated.Invoke(StateEventType.Enter);
        }

        public void UpdateState(float deltaTime)
        {
            if (isActive)
            {
                if (subState)
                {
                    subState.UpdateState(deltaTime);
                }
                
                OnUpdate(deltaTime);
                
                onUpdated.Invoke(StateEventType.Update);
            }
        }

        public void Exit()
        {
            if (!isActive) return;

            isActive = false;
            
            if (subState)
            {
                subState.Exit();
            }
                
            OnExit();
            
            if (needReset && isDone)
            {
                isDone = false;
            }
                
            onUpdated.Invoke(StateEventType.Exit);
        }
        
        #endregion

        protected void SetSubState(StateBase state)
        {
            if (isActive)
            {
                if (subState)
                {
                    subState.Exit();
                }
            
                subState = state;
            
                if (subState)
                {
                    subState.Enter();
                }
            }
        }
        
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

    public abstract class StateBase<TCore> : StateBase
    {
        [Space]
        [SerializeField] private TCore _core = default;

        public TCore core
        {
            get => _core;
            private set => _core = value;
        }
        
        public void Install(TCore core)
        {
            this.core = core;

            OnInstall();
        }

        protected virtual void OnInstall()
        {
        }
    }
}