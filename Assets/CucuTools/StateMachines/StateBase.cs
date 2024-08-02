using System;
using UnityEngine;

namespace CucuTools.StateMachines
{
    /// <summary>
    /// State
    /// </summary>
    public class StateBase : MonoBehaviour
    {
        private float _enterUnscaledTime;
        
        #region SerializeField

        [SerializeField] private bool _isActive = false;
        
        [Header("State")]
        [SerializeField] private bool _isDone = false;
        [SerializeField] private bool _undoneOnStartState = true;
        
        #endregion
        
        #region Public API

        /// <summary>
        /// Current State Status
        /// </summary>
        public bool isActive
        {
            get => _isActive;
            private set => _isActive = value;
        }
        
        /// <summary>
        /// State is Done or Not
        /// </summary>
        public bool isDone
        {
            get => _isDone;
            set => _isDone = value;
        }

        /// <summary>
        /// Reset <see cref="isDone"/> after <see cref="StartState"/> or not
        /// </summary>
        public bool undoneOnStartState
        {
            get => _undoneOnStartState;
            set => _undoneOnStartState = value;
        }
        
        /// <summary>
        /// Time since the start state
        /// </summary>
        public float time { get; private set; }
        
        /// <summary>
        /// Unscaled Time since the start state
        /// </summary>
        public float unscaledTime => Time.unscaledTime - _enterUnscaledTime;
        
        /// <summary>
        /// Current Sub State
        /// </summary>
        public StateBase SubState { get; private set; }

        /// <summary>
        /// Event of State. Started or Stopped
        /// </summary>
        public event Action<StateEventType> OnStateUpdated;
        
        /// <summary>
        /// Start Current State. It is possible if ONLY State is Not Active
        /// </summary>
        [ContextMenu(nameof(StartState))]
        public void StartState()
        {
            if (isActive) return;
            
            isActive = true;
            if (undoneOnStartState && isDone)
            {
                isDone = false;
            }

            time = 0f;
            _enterUnscaledTime = Time.unscaledTime;
            
            OnStartState();
            OnStateUpdated?.Invoke(StateEventType.Start);
        }

        /// <summary>
        /// Update Current State. It is possible if ONLY State is Active
        /// </summary>
        /// <param name="deltaTime"></param>
        public void UpdateState(float deltaTime)
        {
            if (isActive)
            {
                SubState?.UpdateState(deltaTime);
                
                OnUpdateState(deltaTime);

                time += deltaTime;
            }
        }

        /// <summary>
        /// Stop Current State. It is possible if ONLY State is Active
        /// </summary>
        [ContextMenu(nameof(StopState))]
        public void StopState()
        {
            if (!isActive) return;

            SetSubState(null);
            
            isActive = false;
            isDone = true;
            
            OnStopState();
            OnStateUpdated?.Invoke(StateEventType.Stop);
        }
        
        #endregion

        /// <summary>
        /// Set new Sub State.
        /// If Current State is Active, Stop/Start methods will be called on Old and New Sub States.
        /// </summary>
        /// <param name="subState"></param>
        protected void SetSubState(StateBase subState)
        {
            if (subState == this)
            {
                Debug.LogWarning($"\"{name}\" has tried to set itself as Sub State. It's impossible!");
                return;
            }

            if (isActive)
            {
                SubState?.StopState();
            }
            
            SubState = subState;

            if (isActive)
            {
                SubState?.StartState();
            }
        }
        
        #region Virtual API

        /// <summary>
        /// Is called after <see cref="StartState"/>
        /// </summary>
        protected virtual void OnStartState()
        {
        }

        /// <summary>
        /// It is called after <see cref="UpdateState"/> of <see cref="SubState"/> and if only Current State is Active
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void OnUpdateState(float deltaTime)
        {
        }

        /// <summary>
        /// It is called after <see cref="StopState"/>
        /// </summary>
        protected virtual void OnStopState()
        {
        }

        #endregion
    }

    public enum StateEventType
    {
        Start,
        Stop,
    }

    /// <inheritdoc cref="CucuTools.StateMachines.StateBase" />
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