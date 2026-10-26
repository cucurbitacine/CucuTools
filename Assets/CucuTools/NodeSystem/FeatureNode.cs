using UnityEngine;

namespace CucuTools.NodeSystem
{
    public abstract class FeatureNode : MonoBehaviour, IRunProcess, INodeProcess
    {
        [SerializeField] private bool _useFixedUpdate = false;
        public bool UseFixedUpdate() => _useFixedUpdate;
        public bool UseUpdate() => !UseFixedUpdate();
        
        public virtual void OnStartRun(StateMachineRunner runner)
        {
            var parent = GetComponentInParent<IFeatureCollection>();
            parent.GetFeatures().Add(this);
        }
        
        public virtual void OnStopRun(StateMachineRunner runner)
        {
        }

        public abstract void OnEnterNode(INode node, INode previous);
        public abstract void OnExitNode(INode node, INode next);
        public abstract void OnUpdateNode(INode node, float deltaTime);
    }
}