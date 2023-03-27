using CucuTools.PlayerSystem;
using CucuTools.Scenes;
using UnityEngine;
using UnityEngine.Events;

namespace Examples.Playground.Scripts
{
    [SceneController]
    public class PlaygroundController : SceneController
    {
        public Transform lastSpawnPoint = null;

        [Space]
        public UnityEvent onGameStarted = new UnityEvent();
        public UnityEvent onGameStopped = new UnityEvent();
        public UnityEvent onPlayerSpawned = new UnityEvent();

        [Space]
        public PlayerController player = null;
        public Transform startSpawnPoint = null;

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
            player.LookIn(spawnPoint.rotation * Vector3.forward);
            
            onPlayerSpawned.Invoke();
        }
        
        public void SpawnPlayer()
        {
            SpawnPlayer(lastSpawnPoint);
        }

        public void SetSpawnPoint(Transform spawnPoint)
        {
            lastSpawnPoint = spawnPoint;
        }

        private void Start()
        {
            StartGame();
        }
    }
}
