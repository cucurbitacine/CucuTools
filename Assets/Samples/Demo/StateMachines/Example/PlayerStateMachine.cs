using CucuTools.StateMachines;
using Samples.Demo.Scripts;
using Samples.Demo.StateMachines.Example.States;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example
{
    public class PlayerStateMachine : StateMachineBase, IHavePosition
    {
        public DraggableSprite draggable;
        
        [Space]
        public WanderPositionState wander;
        public IdleState drag;
        
        public Vector2 position
        {
            get => transform.position;
            set => transform.position = value;
        }
        
        public override StateBase GetEntryState()
        {
            return wander;
        }

        public override StateBase GetNextState()
        {
            return wander;
        }

        private void Awake()
        {
            wander.Install(this);
            wander.move.Install(this);
        }

        private void LateUpdate()
        {
            if (isActive && draggable)
            {
                if (draggable.isDragging)
                {
                    if (activeState == wander)
                    {
                        SetState(drag);
                    }
                }
                else
                {
                    if (activeState == drag)
                    {
                        SetState(wander);
                    }
                }
            }
        }
    }
}