using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.StateMachines.Impl
{
    public sealed class StateMachineQueue : StateMachineBase
    {
        [Header("Queue")]
        public bool looped = false;
        public bool getStatesInChildren = false;
        
        [Space]
        public int selected = 0;
        public List<StateBase> states = new List<StateBase>();
        
        public override StateBase GetNextState()
        {
            var index = selected;
            
            if (looped)
            {
                index = (index + 1) % states.Count;
            }
            else
            {
                index++;
            }

            return 0 <= index && index < states.Count ? states[index] : null;
        }

        protected override void OnStateChange()
        {
            var index = states.IndexOf(activeState);

            if (index >= 0)
            {
                selected = index;
            }
        }

        private void Awake()
        {
            if (getStatesInChildren)
            {
                states.Clear();
                states.AddRange(GetComponentsInChildren<StateBase>());
            }
            
            states.RemoveAll(s => s == null || s == this);

            if (states.Count > 0)
            {
                selected %= states.Count;
                entryState = states[selected];
            }
        }
    }
}