using CucuTools;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife
{
    public class CellController : CucuBehaviour
    {
        public CellType cellType = CellType.Unknown;
        
        [Header("Health")]
        public float lifetime = 0f;
        [Min(0)] public int health = 5;
        [Min(0)] public int totalHealth = 5;
        [Min(0)] public float totalLifetime = 10f;
        
        [Header("Movement")]
        public float speed = 1f;
        public FixedArea fixedArea = new FixedArea();

        [Header("References")]
        public CellStateMachine stateMachine;
        public SensorController sensor;
        public CellDivider divider;
        
        private float timeStart;
        
        public float time => Time.time - timeStart;
        
        public Vector2 position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        private void OnEnable()
        {
            timeStart = Time.time;
        }

        private void Update()
        {
            lifetime = time;
            
            if (lifetime > totalLifetime)
            {
                health = 0;
            }
        }
    }

    public enum CellType
    {
        Unknown,
        Food,
        Victim,
        Predator,
    }
}