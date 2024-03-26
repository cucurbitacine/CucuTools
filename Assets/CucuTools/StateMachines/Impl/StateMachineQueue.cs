using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.StateMachines.Impl
{
    public sealed class StateMachineQueue : StateMachineBase
    {
        [Header("Queue")]
        public bool looped = false;
        public bool getInChildren = true;
        
        [Space]
        public List<StateBase> queue = new List<StateBase>();
        
        private int _selected = 0;

        public override StateBase GetEntryState()
        {
            return queue.Count > 0 ? queue[0] : null;
        }

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

        protected override void OnStateChange()
        {
            var index = queue.IndexOf(activeState);

            if (index >= 0)
            {
                _selected = index;
            }
        }

        private void Awake()
        {
            if (getInChildren)
            {
                queue.Clear();
                queue.AddRange(GetComponentsInChildren<StateBase>());
                queue.RemoveAll(s => s.transform.parent != transform);
            }
            
            queue.RemoveAll(s => s == null || s == this); 
        }
    }
}