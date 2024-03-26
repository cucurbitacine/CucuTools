using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife.States.LifeImpl
{
    public class VictimLife : LifeState
    {
        [Header("Victim's Life")]
        public CellType foodType = CellType.Food;
        public CellController food;
        
        [Space]
        public CellType dangerType = CellType.Predator;
        public CellController danger;
        
        [Space]
        public WanderState wander;
        public ChaseState chase;
        public AvoidState avoid;

        protected override void OnInstall()
        {
            chase.Install(core);
        }

        protected override void OnEnter()
        {
            wander.around = core.position; 
            SetSubState(wander);
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            if (subState == wander) // WANDER -> AVOID / CHASE
            {
                wander.around = core.position;

                if (core.sensor.TryGetTarget(dangerType, out danger))
                {
                    avoid.target = danger;
                    SetSubState(avoid);
                }
                else if (core.sensor.TryGetTarget(foodType, out food))
                {
                    chase.target = food;
                    SetSubState(chase);
                }
            }
            else if (subState == chase) // CHASE -> AVOID / WANDER
            {
                if (core.sensor.TryGetTarget(dangerType, out danger))
                {
                    avoid.target = danger;
                    SetSubState(avoid);
                }
                else if (!core.sensor.Contains(food) || chase.isDone)
                {
                    SetSubState(wander);
                }
            }
            else if (subState == avoid) // AVOID -> WANDER
            {
                if (!core.sensor.Contains(danger) || avoid.isDone)
                {
                    SetSubState(wander);
                }
            }
        }
    }
}