using System.Collections.Generic;

namespace CucuTools.NodeSystem
{
    public interface INode : IGroupCollection, IFeatureCollection, ITransitionCollection, INextNode
    {
        public bool IsRunning();
        public float GetRunningTime();
        
        public void EnterNode(INode previous);
        public void ExitNode(INode next);
        public void UpdateNode(float deltaTime);
        public void FixedUpdateNode(float deltaTime);
    }
    
    public interface IGroupCollection
    {
        public ICollection<GroupNode> GetGroups();
    }
    
    public interface IFeatureCollection
    {
        public ICollection<FeatureNode> GetFeatures();
    }
    
    public interface ITransitionCollection
    {
        public ICollection<TransitionNode> GetTransitions();
    }
    
    public interface IConditionCollection
    {
        public ICollection<Condition> GetConditions();
    }
    
    public interface IDestination
    {
        public INode GetNextNode();
    }
    
    public interface INextNode
    {
        public bool TryGetNextNode(out INode next);
    }
    
    public interface IReady
    {
        public bool IsReady(INode node);
    }
    
    public interface IRunProcess
    {
        public void OnStartRun(StateMachineRunner runner);
        public void OnStopRun(StateMachineRunner runner);
    }

    public interface INodeProcess
    {
        public void OnEnterNode(INode node, INode previous);
        public void OnExitNode(INode node, INode next);
    }
}