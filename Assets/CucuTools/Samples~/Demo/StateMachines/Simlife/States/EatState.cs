using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife.States
{
    public class EatState : StateBase<CellController>
    {
        [Header("Eating")]
        public int eatPower = 1;
        
        [Space]
        public CellController target;
        
        [Space]
        public WaitState wait;

        protected override void OnEnter()
        {
            core.divider.Eat(target, eatPower);
            
            SetSubState(wait);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (wait.isDone)
            {
                isDone = true;
            }
        }
    }
}