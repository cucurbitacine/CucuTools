using UnityEngine;

namespace CucuTools.StateMachines
{
    /// <summary>
    /// Transition
    /// </summary>
    public abstract class TransitionBase : CucuBehaviour
    {
        [Header("Transition Info")]
        [SerializeField] private string key = string.Empty;

        /// <summary>
        /// Transition key
        /// </summary>
        public string Key
        {
            get => key;
            set => key = value;
        }
        
        public abstract string ConditionKey { get; set; }
        public abstract bool ConditionValue { get; set; }

        /// <summary>
        /// Target state
        /// </summary>
        /// <returns></returns>
        public abstract StateBase TargetState { get; set; }

        /// <summary>
        /// Process transition to the next state
        /// </summary>
        /// <param name="state">Next state</param>
        /// <returns>Transition completed</returns>
        public abstract bool NextState(out StateBase state);

        /// <summary>
        /// Validate behaviour
        /// </summary>
        public abstract void Validate();
    }
}