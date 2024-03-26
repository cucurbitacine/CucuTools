using CucuTools;
using CucuTools.StateMachines;
using Samples.Demo.Scripts;
using Samples.Demo.StateMachines.Simlife.States;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife
{
    public class CellStateMachine : StateMachineBase
    {
        [Header("Cell")]
        public CellController cell;
        public DraggableSprite draggable;
        
        [Space]
        public LifeState life;
        public DeadState dead;
        public IdleState idle;

        public override StateBase GetEntryState()
        {
            return life;
        }

        public override StateBase GetNextState()
        {
            return dead;
        }

        private void Awake()
        {
            if (cell == null) cell = GetComponentInParent<CellController>();

            if (cell)
            {
                foreach (var state in GetComponentsInChildren<StateBase>())
                {
                    state.TryInstall(cell);
                }
            }
        }

        private void LateUpdate()
        {
            if (isActive)
            {
                if (draggable.isDragging)
                {
                    if (activeState != idle)
                    {
                        SetState(idle);
                    }
                }
                else
                {
                    if (cell.health == 0)
                    {
                        if (activeState != dead)
                        {
                            SetState(dead);
                        }
                    }
                    else
                    {
                        if (activeState != life)
                        {
                            SetState(life);
                        }
                    }
                }
            }
        }
    }
}