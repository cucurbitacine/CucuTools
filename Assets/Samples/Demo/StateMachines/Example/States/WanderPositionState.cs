using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class WanderPositionState : StateBase<IHavePosition>
    {
        [Header("WANDER")]
        public bool countless = false;
        public int countRandomPoints = 5;

        public float radiusWandering = 2f;

        [Space]
        public Vector2 initPosition;
        public int selected = 0;
        
        [Space]
        public MovePositionState move;
        public WaitState wait;

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
                isDone = true;
            }
        }
        
        protected override void OnEnter()
        {
            initPosition = core.position;
            selected = -1;
            
            SetSubState(wait);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (subState == wait)
            {
                if (wait.isDone)
                {
                    NextPoint();
                }
            }else if (subState == move)
            {
                if (move.isDone)
                {
                    SetSubState(wait);
                }
            }
        }
    }
}