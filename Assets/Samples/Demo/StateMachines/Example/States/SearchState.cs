using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class SearchState : StateBase<NpcStateMachine>
    {
        [Header("SEARCH")]
        public MoveState move;
        public WanderState wander;

        protected override void OnSetContext()
        {
            move.SetContext(Context.npc);
            wander.SetContext(Context.npc);
        }

        protected override void OnStartState()
        {
            move.point = Context.lastTargetPosition;
            SetSubState(move);
        }

        protected override void OnUpdateState(float deltaTime)
        {
            if (SubState == move)
            {
                if (move.isDone)
                {
                    SetSubState(wander);
                }
            }
            else if (SubState == wander)
            {
                if (wander.isDone)
                {
                    isDone = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (isActive && Context != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(Context.lastTargetPosition, 1f);
            }
        }
    }
}