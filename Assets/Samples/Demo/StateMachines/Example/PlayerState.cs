using CucuTools.StateMachines;
using Samples.Demo.Scripts;
using Samples.Demo.StateMachines.Example.States;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example
{
    public class PlayerState : StateBase
    {
        [Header("PLAYER")]
        public DraggableSprite player;
        
        [Space]
        public WanderState wander;
        public DragState drag;
        
        protected override void OnEnter()
        {
            wander.SetContext(player.transform);
            
            SetSubState(wander);
        }

        protected override void OnExecute()
        {
            if (SubState.IsDone)
            {
                SetSubState(wander);
                
                return;
            }
            
            if (player.isDragging)
            {
                if (wander.IsRunning)
                {
                    SetSubState(drag);
                }
            }
            else
            {
                if (drag.IsRunning)
                {
                    SetSubState(wander);
                }
            }
        }
    }
}