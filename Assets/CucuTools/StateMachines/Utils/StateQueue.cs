using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.StateMachines.Utils
{
    /// <summary>
    /// State Machine with List of States
    /// </summary>
    public sealed class StateQueue : StateBase
    {
        private int _selected = 0;

        [Header("Queue")]
        [SerializeField] private bool looped = false;
        [SerializeField] private bool searchStatesInChildren = true;

        [Space]
        [SerializeField] private List<StateBase> queue = new List<StateBase>();

        private bool TryGetNextState(int current, out int next)
        {
            next = current;

            if (looped)
            {
                next = (next + 1) % queue.Count;
            }
            else
            {
                next++;
            }

            return 0 <= next && next < queue.Count;
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

        protected override void OnEnter()
        {
            var entryState = queue.Count > 0 ? queue[0] : null;

            SetSubState(entryState);
        }

        protected override void OnExecute()
        {
            if (SubState.IsDone)
            {
                if (TryGetNextState(_selected, out var next))
                {
                    _selected = next;
                    
                    SetSubState(queue[_selected]);
                }
                else
                {
                    SetDone(true);
                }
            }
        }
    }
}