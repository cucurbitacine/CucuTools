using System.Collections.Generic;
using System.Linq;
using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class PatrolState : StateBase<Transform>
    {
        [Header("PATROL")]
        public MoveState move;
        public WaitState wait;
        
        [Space]
        public int selected = 0;
        public List<Vector2> points = new List<Vector2>();

        protected override void OnSetContext()
        {
            move.SetContext(Context);
        }

        protected override void OnStartState()
        {
            if (points.Count == 0)
            {
                points.Add(transform.position);
            }

            move.point = GetPoint();
            SetSubState(move);
        }

        protected override void OnUpdateState(float deltaTime)
        {
            if (SubState == wait)
            {
                if (wait.isDone)
                {
                    move.point = NextPoint();
                    SetSubState(move);
                }
            }
            else if (SubState == move)
            {
                if (move.isDone)
                {
                    SetSubState(wait);
                }
            }
        }
        
        private Vector2 GetPoint()
        {
            selected %= points.Count;
            return points[selected];
        }

        private Vector2 NextPoint()
        {
            selected = (selected + 1) % points.Count;
            return GetPoint();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLineStrip(points.Select(a => (Vector3)a).ToArray(), true);

            foreach (var anchor in points)
            {
                Gizmos.DrawWireSphere(anchor, 0.5f);
            }
        }
    }

    public abstract class StateWithName<TCore> : StateBase<TCore>
    {
        public abstract string GetName();
    } 
}