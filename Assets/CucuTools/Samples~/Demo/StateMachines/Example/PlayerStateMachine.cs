using CucuTools.StateMachines;
using Samples.Demo.Scripts;
using Samples.Demo.StateMachines.Example.States;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example
{
    public class PlayerStateMachine : StateMachineBase
    {
        [Header("PLAYER")]
        public DraggableSprite player;
        
        [Space]
        public WanderState wander;
        public DragState drag;

        protected override void OnStartStateMachine()
        {
            wander.SetContext(player.transform);
            
            EntryState = wander;
        }
        
        public override StateBase GetNextState()
        {
            return wander;
        }

        protected override void OnUpdateStateMachine(float deltaTime)
        {
            if (player)
            {
                if (player.isDragging)
                {
                    if (ActiveState == wander)
                    {
                        SetNextState(drag);
                    }
                }
                else
                {
                    if (ActiveState == drag)
                    {
                        SetNextState(wander);
                    }
                }
            }
        }
    }
}