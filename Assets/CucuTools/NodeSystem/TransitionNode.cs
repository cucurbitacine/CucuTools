using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.NodeSystem
{
    public class TransitionNode : MonoBehaviour, IRunProcess, IConditionCollection, IReady, IDestination
    {
        [SerializeField] private StateNode _nextNode;
        [SerializeField] private List<Condition> _condition = new List<Condition>();
        [Space]
        [SerializeField] private bool _anyTrue = false;
        [SerializeField] private bool _mute = false;
        [SerializeField] private bool _solo = false;
        
        public void OnStartRun(StateMachineRunner runner)
        {
            var parent = GetComponentInParent<ITransitionCollection>();
            parent.GetTransitions().Add(this);
        }

        public void OnStopRun(StateMachineRunner runner)
        {
        }
        
        public ICollection<Condition> GetConditions()
        {
            return _condition;
        }
        
        public bool IsReady(INode node)
        {
            if (_mute) return false;
            if (_solo) return true;
            
            return _anyTrue ? GetConditions().Any(c => c.IsReady(node)) : GetConditions().All(c => c.IsReady(node));
        }
        
        public INode GetNextNode()
        {
            return _nextNode;
        }
        
        public bool TryGetNextNode(INode node, out INode next)
        {
            if (IsReady(node))
            {
                next = GetNextNode();
                return true;
            }

            next = null;
            return false;
        }
        
        private void OnValidate()
        {
            foreach (var condition in GetConditions())
            {
                condition?.OnValidate();
            }
        }
    }
}