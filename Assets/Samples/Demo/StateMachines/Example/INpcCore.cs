using UnityEngine;

namespace Samples.Demo.StateMachines.Example
{
    public interface INpcCore : IHavePosition, IHaveTarget
    {
    }
    
    public interface IHavePosition
    {
        public Vector2 position { get; set; }
    }
    
    public interface IHaveTarget : IHaveTargetPosition
    {
        public bool hasTarget { get; }
        public Transform target { get; }
    }
    
    public interface IHaveTargetPosition
    {
        public Vector2 targetPosition { get; }
        public Vector2 lastTargetPosition { get; }
    }
}