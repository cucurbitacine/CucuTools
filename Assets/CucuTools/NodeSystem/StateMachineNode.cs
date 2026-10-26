using UnityEngine;

namespace CucuTools.NodeSystem
{
    [DisallowMultipleComponent]
    public class StateMachineNode : StateNode
    {
        [Header("State Machine")]
        [SerializeField] private StateNode _firstNode;
        
        private INode _actualNode;

        public INode GetFirstNode()
        {
            return _firstNode;
        }
        
        public INode GetActualNode()
        {
            return _actualNode;
        }
        
        public override void EnterNode(INode previous)
        {
            base.EnterNode(previous);

            _actualNode = _firstNode;
            _actualNode.EnterNode(previous);
        }

        public override void ExitNode(INode next)
        {
            _actualNode.ExitNode(next);
            
            base.ExitNode(next);
        }

        public override void UpdateNode(float deltaTime)
        {
            if (_actualNode.TryGetNextNode(out var nextNode))
            {
                var previousNode = _actualNode;
                
                _actualNode.ExitNode(nextNode);
                _actualNode = nextNode;
                _actualNode.EnterNode(previousNode);
            }
            else
            {
                base.UpdateNode(deltaTime);
                
                _actualNode.UpdateNode(deltaTime);
            }
        }

        public override void FixedUpdateNode(float deltaTime)
        {
            base.FixedUpdateNode(deltaTime);
            
            _actualNode.FixedUpdateNode(deltaTime);
        }
    }
}