using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class WanderState : StateBase<Transform>
    {
        [Header("WANDER")]
        public bool countless = false;
        public int countRandomPoints = 5;

        public float radiusWandering = 2f;

        [Space]
        public Vector2 initPosition;
        public int selected = 0;

        [Space]
        public MoveState move;
        public WaitState wait;

        protected override void OnSetContext()
        {
            move.SetContext(Context);
        }

        protected override void OnEnter()
        {
            initPosition = Context.position;
            selected = -1;

            SetSubState(wait);
        }

        protected override void OnExecute()
        {
            if (wait.IsRunning)
            {
                if (wait.IsDone)
                {
                    NextPoint();
                }
            }
            else if (move.IsRunning)
            {
                if (move.IsDone)
                {
                    SetSubState(wait);
                }
            }
        }
        
        private void NextPoint()
        {
            if (countless)
            {
                move.point = initPosition + Random.insideUnitCircle * radiusWandering;
                SetSubState(move);

                return;
            }

            selected++;

            if (selected < countRandomPoints)
            {
                move.point = initPosition + Random.insideUnitCircle * radiusWandering;
                SetSubState(move);
            }
            else
            {
                SetDone(true);
            }
        }
    }
}