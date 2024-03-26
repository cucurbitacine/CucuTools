using System.Collections.Generic;
using System.Linq;
using CucuTools;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife
{
    public class SensorController : CucuBehaviour
    {
        [Min(0f)] public float sensorRadius = 3f;

        [Space] public List<CellController> targets = new List<CellController>();
        
        [Space] public CellController origin;
        
        private readonly Collider2D[] overlap = new Collider2D[64];

        public Vector2 originPosition => origin.position;

        public int Count()
        {
            return targets.Count;
        }
        
        public bool Contains(CellController target)
        {
            return targets.Contains(target);
        }

        public bool TryGetTarget(CellType cellType, out CellController specificTarget)
        {
            var validTargets = targets.Where(t => t && t.cellType == cellType).ToArray();

            if (validTargets.Length > 0)
            {
                specificTarget = validTargets[Random.Range(0, validTargets.Length)];
                return true;
            }

            specificTarget = null;
            return false;
        }
        
        private void FixedUpdate()
        {
            if (origin == null || origin.health == 0) return;
            
            var count = Physics2D.OverlapCircleNonAlloc(originPosition, sensorRadius, overlap);

            targets.Clear();
            
            if (count > 0)
            {
                var validCells = overlap.Take(count)
                    .Select(cld => cld.GetComponent<CellController>())
                    .Where(cell => cell && cell != origin && cell.health > 0).ToArray();

                targets.AddRange(validCells);
            }
        }
    }
}