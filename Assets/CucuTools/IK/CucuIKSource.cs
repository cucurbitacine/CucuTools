using UnityEngine;

namespace CucuTools.IK
{
    public abstract class CucuIKSource : CucuIKBehaviour
    {
        [Header("Source")]
        [SerializeField] private bool useWorldSpace = true;
        [SerializeField] private bool updatePoints = false;

        public bool UseWorldSpace
        {
            get => useWorldSpace;
            set => useWorldSpace = value;
        }

        public bool UpdatePoints
        {
            get => updatePoints;
            set => updatePoints = value;
        }

        public abstract Vector3[] GetPoints();

        public virtual void SetPoints()
        {
            Brain?.SetPoints(GetPoints(), UseWorldSpace);
        }
        
        protected virtual void Start()
        {
            SetPoints();
        }

        protected virtual void LateUpdate()
        {
            if (UpdatePoints) SetPoints();
        }
    }
}