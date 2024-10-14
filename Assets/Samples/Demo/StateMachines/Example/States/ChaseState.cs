using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class ChaseState : StateBase<NpcState>
    {
        [Header("CHASE")]
        public MoveState move;

        protected override void OnSetContext()
        {
            move.SetContext(Context.npc);
        }

        protected override void OnEnter()
        {
            move.point = Context.targetPosition;
            SetSubState(move);
        }

        protected override void OnExecute()
        {
            if (move.IsDone)
            {
                SetDone(true);
            }
            else
            {
                move.point = Context.targetPosition;
            }
        }

        private void OnDrawGizmos()
        {
            if (IsRunning && Context != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(Context.targetPosition, 1f);
            }
        }
    }
}