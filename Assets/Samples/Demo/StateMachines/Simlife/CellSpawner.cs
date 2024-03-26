using CucuTools;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife
{
    public class CellSpawner : MonoBehaviour
    {
        [Min(0)] public int amountPerSpawn = 1;
        public CellType cellType = CellType.Food;
        [Min(0f)] public float frequencyMax = 1f;

        [Space]
        [Min(0)] public int numberCurrent;
        [Min(1)] public int numberMax = 20;
        public FixedArea fixedArea = new FixedArea();

        [Space] public CellProvider provider;
        public Transform rootSpawn;

        public float timeLastSpawn { get; private set; }
        public float time { get; private set; }

        public float frequency => frequencyMax * (numberMax - numberCurrent) / numberMax;
        public float period => frequency > 0f ? 1f / frequency : float.MaxValue;

        public void Spawn()
        {
            timeLastSpawn = Time.time;
            time = 0f;
            
            if (rootSpawn == null) rootSpawn = transform;

            if (provider)
            {
                for (var i = 0; i < amountPerSpawn; i++)
                {
                    if (provider.TrySpawn(cellType, out var cell))
                    {
                        var point = rootSpawn.position;

                        if (fixedArea.fixedArea)
                        {
                            point = fixedArea.GetRandom();
                        }
                        
                        cell.transform.SetParent(rootSpawn);
                        cell.transform.SetPositionAndRotation(point, Quaternion.identity);

                        cell.health = cell.totalHealth;
                        cell.divider.score = 0;
                        cell.fixedArea = fixedArea;
                        cell.stateMachine.Restart();
                    }
                }
            }
        }

        private void Update()
        {
            if (provider && provider.TryGetPrefab(cellType, out var cellPrefab))
            {
                var typeId = cellPrefab.gameObject.GetInstanceID();
                numberCurrent = Cucu.PoolManager.Count(t => !t.disposed && t.typeId == typeId && !t.released);
            }

            if (time > period)
            {
                Spawn();
            }

            if (numberCurrent < numberMax)
            {
                time += Time.deltaTime;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (fixedArea.fixedArea)
            {
                Gizmos.DrawWireCube(fixedArea.GetAreaCenter(), fixedArea.GetAreaSize());
            }
        }
    }
}