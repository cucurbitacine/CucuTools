using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class SearchTargetState : StateBase<IHaveTargetPosition>
    {
        [Header("SEARCH")]
        public MovePositionState move;
        public WanderPositionState wander;
        
        protected override void OnEnter()
        {
            move.point = core.lastTargetPosition;
            SetSubState(move);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (subState == move)
            {
                if (move.isDone)
                {
                    SetSubState(wander);
                }
            }
            else if (subState == wander)
            {
                if (wander.isDone)
                {
                    isDone = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (isActive && core != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(core.lastTargetPosition, 1f);
            }
        }
    }
}