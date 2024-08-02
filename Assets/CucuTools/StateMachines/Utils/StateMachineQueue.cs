using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.StateMachines.Utils
{
    /// <summary>
    /// State Machine with List of States
    /// </summary>
    public sealed class StateMachineQueue : StateMachineBase
    {
        private int _selected = 0;
        
        [Header("Queue")]
        [SerializeField] private bool looped = false;
        [SerializeField] private bool searchStatesInChildren = true;
        
        [Space]
        [SerializeField] private List<StateBase> queue = new List<StateBase>();

        /// <inheritdoc />
        protected override void OnStartStateMachine()
        {
            EntryState = queue.Count > 0 ? queue[0] : null;
        }
        
        /// <inheritdoc />
        public override StateBase GetNextState()
        {
            var index = _selected;
            
            if (looped)
            {
                index = (index + 1) % queue.Count;
            }
            else
            {
                index++;
            }

            return 0 <= index && index < queue.Count ? queue[index] : null;
        }
        
        /// <inheritdoc />
        protected override void OnNextState()
        {
            var index = queue.IndexOf(ActiveState);

            if (index >= 0)
            {
                _selected = index;
            }
        }

        private void Awake()
        {
            if (searchStatesInChildren)
            {
                queue.Clear();
                queue.AddRange(GetComponentsInChildren<StateBase>());
                queue.RemoveAll(s => s.transform.parent != transform);
            }
            
            queue.RemoveAll(s => s == null || s == this); 
        }
    }
}