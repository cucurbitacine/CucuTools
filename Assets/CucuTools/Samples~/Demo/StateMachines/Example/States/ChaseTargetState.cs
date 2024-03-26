using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class ChaseTargetState : StateBase<IHaveTargetPosition>
    {
        [Header("CHASE")]
        public MovePositionState move;

        protected override void OnEnter()
        {
            move.point = core.targetPosition;
            SetSubState(move);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (move.isDone)
            {
                isDone = true;
            }
            else
            {
                move.point = core.targetPosition;
            }
        }

        private void OnDrawGizmos()
        {
            if (isActive && core != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(core.targetPosition, 1f);
            }
        }
    }
}