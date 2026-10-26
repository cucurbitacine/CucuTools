using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.NodeSystem
{
    [DisallowMultipleComponent]
    public class StateNode : MonoBehaviour, INode
    {
        [Header("State")]
        [SerializeField] private bool _isRunning;
        [SerializeField] private float _runningTime;
        [SerializeField] private List<GroupNode> _groups = new List<GroupNode>();
        
        private readonly List<FeatureNode> _features = new List<FeatureNode>();
        private readonly List<TransitionNode> _transitions = new List<TransitionNode>();

        public event Action<StateEventType> OnStateEvent;
        
        public ICollection<GroupNode> GetGroups()
        {
            return _groups;
        }
        
        public ICollection<FeatureNode> GetFeatures()
        {
            return _features;
        }
        
        public ICollection<TransitionNode> GetTransitions()
        {
            return _transitions;
        }
        
        public bool TryGetNextNode(out INode next)
        {
            foreach (var group in GetGroups())
            {
                if (group.TryGetNextNode(this, out next))
                {
                    return true;
                }
            }
            
            foreach (var transition in GetTransitions())
            {
                if (transition.TryGetNextNode(this, out next))
                {
                    return true;
                }
            }
            
            next = null;
            return false;
        }

        public bool IsRunning()
        {
            return _isRunning;
        }

        public float GetRunningTime()
        {
            return _runningTime;
        }
        
        public virtual void EnterNode(INode previous)
        {
            _isRunning = true;
            _runningTime = 0f;
            
            foreach (var group in GetGroups())
            {
                group.OnEnterNode(this, previous);
            }
            
            foreach (var feature in GetFeatures())
            {
                feature.OnEnterNode(this, previous);
            }

            OnStateEvent?.Invoke(StateEventType.Enter);
        }
        
        public virtual void ExitNode(INode next)
        {
            _isRunning = false;
            
            foreach (var group in GetGroups())
            {
                group.OnExitNode(this, next);
            }
            
            foreach (var feature in GetFeatures())
            {
                feature.OnExitNode(this, next);
            }
            
            OnStateEvent?.Invoke(StateEventType.Exit);
        }

        public virtual void UpdateNode(float deltaTime)
        {
            _runningTime += deltaTime;
            
            foreach (var group in GetGroups())
            {
                foreach (var feature in group.GetFeatures())
                {
                    if (feature.UseUpdate())
                    {
                        feature.OnUpdateNode(this, deltaTime);
                    }
                }
            }
            
            foreach (var feature in GetFeatures())
            {
                if (feature.UseUpdate())
                {
                    feature.OnUpdateNode(this, deltaTime);
                }
            }
        }

        public virtual void FixedUpdateNode(float deltaTime)
        {
            foreach (var group in GetGroups())
            {
                foreach (var feature in group.GetFeatures())
                {
                    if (feature.UseFixedUpdate())
                    {
                        feature.OnUpdateNode(this, deltaTime);
                    }
                }
            }
            
            foreach (var feature in GetFeatures())
            {
                if (feature.UseFixedUpdate())
                {
                    feature.OnUpdateNode(this, deltaTime);
                }
            }
        }
    }

    public enum StateEventType
    {
        Enter,
        Exit,
    }

    public static class SlotNodeExt
    {
        public static bool TryGet<T>(this ICollection<FeatureNode> collection, out T feature) where T : FeatureNode
        {
            foreach (var item in collection)
            {
                if (item is not T t) continue;
                
                feature = t;
                return true;
            }

            feature = null;
            return false;
        } 
    }
}