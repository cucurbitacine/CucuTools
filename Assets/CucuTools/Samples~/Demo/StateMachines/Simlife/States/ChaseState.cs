using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife.States
{
    public class ChaseState : StateBase<CellController>
    {
        [Header("Chasing")]
        [Min(0f)]
        public float eatDistance = 0.2f;

        public CellController target;
        [Space]
        public MoveState move;
        public EatState eat;

        public SensorController sensor => core.sensor;
        
        protected override void OnInstall()
        {
            move.Install(core);
            eat.Install(core);
        }

        protected override void OnEnter()
        {
            move.destination = target.position;
            SetSubState(move);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (subState == move)
            {
                if (sensor.Contains(target))
                {
                    move.destination = target.position;
                    
                    var distanceToTarget = Vector2.Distance(target.position, sensor.originPosition);

                    if (move.isDone || distanceToTarget < eatDistance)
                    {
                        eat.target = target;
                        SetSubState(eat);
                    }
                }
                else
                {
                    isDone = true;
                }
            }
            else if (subState == eat)
            {
                if (eat.isDone)
                {
                    if (sensor.Contains(target))
                    {
                        move.destination = target.position;
                        SetSubState(move);
                    }
                    else
                    {
                        isDone = true;
                    }
                }
            }
        }
    }
}