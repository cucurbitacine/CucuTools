using CucuTools.Attributes;
using CucuTools.Scenes;
using UnityEngine;
using UnityEngine.Events;

namespace Examples.Playground.Scripts
{
    [SceneController]
    public class PlaygroundController : SceneController
    {
        public Transform startSpawnPoint = null;
        public Transform lastSpawnPoint = null;

        [Space]
        public UnityEvent onGameStarted = new UnityEvent();
        public UnityEvent onGameStopped = new UnityEvent();
        public UnityEvent onPlayerSpawned = new UnityEvent();

        [Space]
        public PlayerController player = null;
        
        public void StartGame()
        {
            if (lastSpawnPoint == null) SetSpawnPoint(startSpawnPoint);
                
            SpawnPlayer();
            
            onGameStarted.Invoke();
        }

        public void StopGame()
        {
            onGameStopped.Invoke();
        }

        public void SpawnPlayer(Transform spawnPoint)
        {
            player.transform.position = spawnPoint.position;
            player.person.LookInDirection(spawnPoint.rotation * Vector3.forward);
            
            onPlayerSpawned.Invoke();
        }
        
        [Button()]
        public void SpawnPlayer()
        {
            SpawnPlayer(lastSpawnPoint);
        }

        public void SetSpawnPoint(Transform spawnPoint)
        {
            lastSpawnPoint = spawnPoint;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            player = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            StartGame();
        }
    }
}
