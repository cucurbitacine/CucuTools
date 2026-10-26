using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.NodeSystem
{
    public class GroupNode : MonoBehaviour, IFeatureCollection, ITransitionCollection, INodeProcess
    {
        private readonly List<FeatureNode> _features = new List<FeatureNode>();
        private readonly List<TransitionNode> _transitions = new List<TransitionNode>();
        
        public ICollection<FeatureNode> GetFeatures()
        {
            return _features;
        }
        
        public ICollection<TransitionNode> GetTransitions()
        {
            return _transitions;
        }
        
        public bool TryGetNextNode(INode node, out INode next)
        {
            foreach (var transition in GetTransitions())
            {
                if (transition.TryGetNextNode(node, out next))
                {
                    return true;
                }
            }

            next = null;
            return false;
        }

        public void OnEnterNode(INode node, INode previous)
        {
            foreach (var feature in GetFeatures())
            {
                feature.OnEnterNode(node, previous);
            }
        }

        public void OnExitNode(INode node, INode next)
        {
            foreach (var feature in GetFeatures())
            {
                feature.OnExitNode(node, next);
            }
        }
    }
}