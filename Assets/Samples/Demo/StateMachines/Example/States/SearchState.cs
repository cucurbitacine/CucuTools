using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class SearchState : StateBase<NpcState>
    {
        [Header("SEARCH")]
        public MoveState move;
        public WanderState wander;

        protected override void OnSetContext()
        {
            move.SetContext(Context.npc);
            wander.SetContext(Context.npc);
        }

        protected override void OnEnter()
        {
            move.point = Context.lastTargetPosition;
            
            SetSubState(move);
        }

        protected override void OnExecute()
        {
            if (move.IsRunning)
            {
                if (move.IsDone)
                {
                    SetSubState(wander);
                }
            }
            else if (wander.IsRunning)
            {
                if (wander.IsDone)
                {
                    SetDone(true);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (IsRunning && Context != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(Context.lastTargetPosition, 1f);
            }
        }
    }
}