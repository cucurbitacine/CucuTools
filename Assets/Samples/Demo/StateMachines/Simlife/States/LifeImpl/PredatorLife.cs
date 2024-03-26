using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife.States.LifeImpl
{
    public class PredatorLife : LifeState
    {
        [Header("Predator's Life")]
        public CellType foodType = CellType.Victim;
        public CellController target;
        
        [Space]
        public WanderState wander;
        public ChaseState chase;

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
            if (subState == wander)
            {
                wander.around = core.position;

                if (core.sensor.TryGetTarget(foodType, out target))
                {
                    chase.target = target;
                    SetSubState(chase);
                }
            }
            else if (subState == chase)
            {
                if (!core.sensor.Contains(target))
                {
                    SetSubState(wander);
                }
            }
        }
    }
}