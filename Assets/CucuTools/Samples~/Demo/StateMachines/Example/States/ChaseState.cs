using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class ChaseState : StateBase<NpcStateMachine>
    {
        [Header("CHASE")]
        public MoveState move;

        protected override void OnSetContext()
        {
            move.SetContext(Context.npc);
        }

        protected override void OnStartState()
        {
            move.point = Context.targetPosition;
            SetSubState(move);
        }

        protected override void OnUpdateState(float deltaTime)
        {
            if (move.isDone)
            {
                isDone = true;
            }
            else
            {
                move.point = Context.targetPosition;
            }
        }

        private void OnDrawGizmos()
        {
            if (isActive && Context != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(Context.targetPosition, 1f);
            }
        }
    }
}