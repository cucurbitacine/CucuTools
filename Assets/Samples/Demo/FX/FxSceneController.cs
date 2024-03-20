using CucuTools;
using CucuTools.Attributes;
using CucuTools.FX;
using Samples.Demo.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Samples.Demo.FX
{
    public class FxSceneController : CucuBehaviour
    {
        [Header("Spawning")]
        public BaseFx prefab;
        public bool instantiateNew = false;
        [Range(1f, 10f)]
        [SerializeField] private float _frequencySpawning = 1f;
        public Vector2 centerSpawn = Vector2.zero;
        public Vector2 sizeSpawn = Vector2.one;

        [Header("Info")]
        [Min(0f)]
        public float timoutCleaning = 5f;
        
        [Space]
        public int calls;
        public int instances;
        public int released;
        
        [Header("UI")]
        public Text textCalls;
        public Text textInstances;
        public Text textReleased;
        
        private Vector2 GetSpawnPoint()
        {
            var random = new Vector2(Random.value, Random.value) - Vector2.one * 0.5f;
            return Vector2.Scale(random, sizeSpawn) + centerSpawn;
        }
        
        public float frequencySpawning
        {
            get => _frequencySpawning;
            set => _frequencySpawning = value;
        }
        public float periodSpawning => 1f / frequencySpawning;
        
        private float _timerSpawning;
        private float _timerCleaning;

        [DrawButton]
        public void CleanPool()
        {
            var count = Cucu.PoolManager.DestroyReleasedObjects();
            
            Debug.Log($"{count} released were destroyed");
            
            count = Cucu.PoolManager.RemoveDisposedObjects();
            
            Debug.Log($"{count} disposed were removed");
        }
        
        private void Call()
        {
            BaseFx fx;

            if (instantiateNew)
            {
                fx = Instantiate(prefab);
                Cucu.PoolManager.Add(prefab, fx);
            }
            else
            {
                fx = Cucu.Instantiate(prefab);
            }

            fx.transform.SetParent(transform);
                
            fx.Play(GetSpawnPoint());
            
            calls++;
            instances = Cucu.PoolManager.Count(prefab);
            released = Cucu.PoolManager.Count(p => p.available && p.released); 
            if (textCalls)
            {
                textCalls.text = $"Calls: {calls}";
            }
            
            if (textInstances)
            {
                textInstances.text = $"Instances: {instances}";
            }

            if (textReleased)
            {
                textReleased.text = $"Released: {released}";
            }
        }

        private void Start()
        {
            _timerSpawning = 0f;
            _timerCleaning = timoutCleaning;
            
            DemoFPS.singleton.Hello();
        }

        private void Update()
        {
            if (prefab == null) return;
            
            if (_timerSpawning > periodSpawning) _timerSpawning = periodSpawning;
            
            if (_timerSpawning <= 0)
            {
                Call();

                _timerSpawning = periodSpawning;
            }
            else
            {
                _timerSpawning -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                instantiateNew = !instantiateNew;
            }

            if (timoutCleaning > 0)
            {
                if (_timerCleaning <= 0)
                {
                    CleanPool();
                    
                    _timerCleaning = timoutCleaning;
                }
                else
                {
                    _timerCleaning -= Time.deltaTime;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(centerSpawn, sizeSpawn);
        }
    }
}