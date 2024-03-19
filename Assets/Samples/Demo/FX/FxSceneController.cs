using System;
using CucuTools;
using CucuTools.Attributes;
using CucuTools.FX;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Samples.Demo.FX
{
    public class FxSceneController : CucuBehaviour
    {
        public int calls;
        public int instances;
        public int released;

        [Space]
        [Range(1f, 10f)]
        [SerializeField] private float _frequency = 1f;
        public Vector2 centerSpawn = Vector2.zero;
        public Vector2 sizeSpawn = Vector2.one;
        
        [Space]
        public BaseFx prefab;

        [Space]
        public Text textCalls;
        public Text textInstances;
        public Text textReleased;
        
        private Vector2 GetSpawnPoint()
        {
            var random = new Vector2(Random.value, Random.value) - Vector2.one * 0.5f;
            return Vector2.Scale(random, sizeSpawn) + centerSpawn;
        }
        
        public float frequency
        {
            get => _frequency;
            set => _frequency = value;
        }
        public float period => 1f / frequency;
        
        private float timer;

        [DrawButton]
        public void ClearPool()
        {
            var count = Cucu.PoolManager.DestroyReleasedObjects();
            
            Debug.Log($"{count} released were destroyed");
            
            count = Cucu.PoolManager.RemoveDisposedObjects();
            
            Debug.Log($"{count} disposed were removed");
        }
        
        private void Call()
        {
            var fx = Cucu.Instantiate(prefab);
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
            timer = 0f;
        }

        private void Update()
        {
            if (timer > period) timer = period;
            
            if (timer <= 0)
            {
                Call();

                timer = period;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(centerSpawn, sizeSpawn);
        }
    }
}