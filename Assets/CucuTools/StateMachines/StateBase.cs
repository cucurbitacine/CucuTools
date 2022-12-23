using System;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.StateMachines
{
    /// <summary>
    /// State
    /// </summary>
    public abstract class StateBase : CucuBehaviour
    {
        [Header("State Info")]
        [SerializeField] private string key = string.Empty;
        [SerializeField] private bool isRunning = false;
        [SerializeField] private StateEvents stateEvents = new StateEvents();

        /// <summary>
        /// State key
        /// </summary>
        public string Key
        {
            get => key;
            set => key = value;
        }
        
        /// <summary>
        /// Is running state or not
        /// </summary>
        public bool IsRunning
        {
            get => isRunning;
            protected set => isRunning = value;
        }
        
        /// <summary>
        /// State event
        /// </summary>
        public StateEvents Events => stateEvents ??= new StateEvents();
        
        /// <summary>
        /// Dead state or not. Dead if state has not any valid transition
        /// </summary>
        public abstract bool Dead { get; }
        
        /// <summary>
        /// Process transition to the next state
        /// </summary>
        /// <param name="state">Next state</param>
        /// <returns>Transition completed</returns>
        public abstract bool NextState(out StateBase state);
        
        /// <summary>
        /// Launch state. State will be running
        /// </summary>
        public abstract void Launch();
        
        /// <summary>
        /// Stop state. State will be not running
        /// </summary>
        public abstract void Stop();
        
        /// <summary>
        /// Get all state's transitions
        /// </summary>
        /// <returns></returns>
        public abstract TransitionBase[] GetTransitions();
        
        /// <summary>
        /// Validate behaviour
        /// </summary>
        public abstract void Validate();
    }

    [Serializable]
    public class StateEvents
    {
        public UnityEvent onEnter;
        public UnityEvent onExit;
    }
}