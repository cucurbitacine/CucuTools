using CucuTools;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife
{
    [CreateAssetMenu(menuName = "Create CellSpawner", fileName = "CellSpawner", order = 0)]
    public class CellProvider : ScriptableObject
    {
        public CellController foodPrefab;
        public CellController victimPrefab;
        public CellController predatorPrefab;
        
        public CellController GetPrefab(CellType cellType)
        {
            if (cellType == CellType.Food) return foodPrefab;
            if (cellType == CellType.Victim) return victimPrefab;
            if (cellType == CellType.Predator) return predatorPrefab;
            
            return null;
        }

        public bool TryGetPrefab(CellType cellType, out CellController cellPrefab)
        {
            return (cellPrefab = GetPrefab(cellType)) != null;
        }
        
        public bool TrySpawn(CellType cellType, out CellController cell)
        {
            if (TryGetPrefab(cellType, out var prefab))
            {
                cell = Cucu.Instantiate(prefab);
                cell.health = cell.totalHealth;

                return true;
            }

            cell = null;
            return false;
        }

        public CellController Spawn(CellType cellType)
        {
            return TrySpawn(cellType, out var cell) ? cell : null;
        }
    }
}
