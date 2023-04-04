using CucuTools;
using CucuTools.Attributes;
using CucuTools.Others;
using UnityEngine;
using UnityEngine.Events;

namespace Examples.Playground.Scripts
{
    public class CheckPoint : CucuBehaviour
    {
        public UnityEvent onCheckPointSaved = new UnityEvent();

        [Space]
        public CheckPoint nextCheckPoint = null;
        
        [Space]
        public CucuTrigger trigger;
        public PathTracker path;
        public PlaygroundController playground;
        public Transform spawnPoint;

        public void Next()
        {
            path.target = playground.player.transform;
            path.StartTracking();
        }
        
        [Button()]
        public void SaveCheckPoint()
        {
            trigger.DisableTrigger();
            
            playground.SetSpawnPoint(spawnPoint);
            
            path.StopTracking();
            path.DrawTrack();

            if (nextCheckPoint != null) nextCheckPoint.Next();
            
            onCheckPointSaved.Invoke();

            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        private void CheckPlayer(Collider cld)
        {
            if (playground.player.capsule == cld)
            {
                SaveCheckPoint();
            }
        }

        private void Awake()
        {
            if (trigger == null) trigger = GetComponent<CucuTrigger>();
            if (path == null) path = GetComponent<PathTracker>();
            if (playground == null) playground = FindObjectOfType<PlaygroundController>();
            if (spawnPoint == null) spawnPoint = transform;
        }

        private void OnEnable()
        {
            trigger.onColliderChanged.AddListener(CheckPlayer);
            
            playground.onPlayerSpawned.AddListener(path.ResetTrack);
        }
        
        private void OnDisable()
        {
            trigger.onColliderChanged.RemoveListener(CheckPlayer);
            
            playground.onPlayerSpawned.RemoveListener(path.ResetTrack);
        }
    }
}
