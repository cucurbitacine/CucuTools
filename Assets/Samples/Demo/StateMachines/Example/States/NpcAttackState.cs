using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class NpcAttackState : StateBase<INpcCore>
    {
        [Header("ATTACK")]
        [Min(0f)] public float attackDistance = 1f;
        [Min(0f)] public float attackDelay = 1f;
        
        public bool CanAttack()
        {
            return core.hasTarget && Vector2.Distance(core.position, core.targetPosition) < attackDistance;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            if (time > attackDelay)
            {
                if (CanAttack())
                {
                    core.target.gameObject.SetActive(false);
                }

                isDone = true;
            }
        }

        private void OnDrawGizmos()
        {
            if (isActive && core != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(core.position, attackDistance);
            }
        }
    }
}