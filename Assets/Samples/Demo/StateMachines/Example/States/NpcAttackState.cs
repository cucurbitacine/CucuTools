using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class NpcAttackState : StateBase<NpcState>
    {
        [Header("ATTACK")]
        [Min(0f)] public float attackDistance = 1f;
        [Min(0f)] public float attackDelay = 1f;
        
        public bool CanAttack()
        {
            return Context.hasTarget && Vector2.Distance(Context.position, Context.targetPosition) < attackDistance;
        }
        
        protected override void OnExecute()
        {
            if (time > attackDelay)
            {
                if (CanAttack())
                {
                    Context.target.gameObject.SetActive(false);
                }

                SetDone(true);
            }
        }

        private void OnDrawGizmos()
        {
            if (IsRunning && Context != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(Context.position, attackDistance);
            }
        }
    }
}