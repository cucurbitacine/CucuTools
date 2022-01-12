using UnityEngine;

namespace CucuTools.IK
{
    public abstract class CucuIKTarget : CucuIKBehaviour
    {
        [Header("Target")]
        [SerializeField] private bool useWorldSpace = true;

        public bool UseWorldSpace
        {
            get => useWorldSpace;
            set => useWorldSpace = value;
        }
        public abstract Vector3 Target { get; set; }

        public virtual void UpdateTarget()
        {
            Brain?.SetTarget(Target, UseWorldSpace);
        }

        protected virtual void Start()
        {
            UpdateTarget();
        }

        protected virtual void LateUpdate()
        {
            UpdateTarget();
        }
    }
}