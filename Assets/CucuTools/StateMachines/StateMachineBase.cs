using System.Collections.Generic;
using System.Linq;

namespace CucuTools.StateMachines
{
    /// <summary>
    /// State Machine
    /// </summary>
    public abstract class StateMachineBase : StateBase
    {
        /// <summary>
        /// Get initial state. First state which will be launched
        /// </summary>
        /// <returns></returns>
        public abstract StateBase GetInitialState();
        
        /// <summary>
        /// Get condition value by key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract bool GetCondition(string key);
        
        /// <summary>
        /// Set condition value by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public abstract void SetCondition(string key, bool value);

        /// <summary>
        /// Get all possible states. 
        /// </summary>
        /// <returns></returns>
        public StateBase[] GetStates()
        {
            var states = new HashSet<StateBase>();

            UpdateStateList(GetInitialState(), states);

            return states.ToArray();
        }
        
        /// <summary>
        /// Recursive searching all possible states 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="states"></param>
        private static void UpdateStateList(StateBase root, ISet<StateBase> states)
        {
            states.Add(root);
            
            var transitions = root.GetTransitions();

            foreach (var transition in transitions)
            {
                var nextState = transition.TargetState;
                if (nextState == null || states.Contains(nextState)) continue;
                UpdateStateList(nextState, states);
            }
        }
    }
}