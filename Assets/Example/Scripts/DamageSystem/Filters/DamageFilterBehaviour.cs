using CucuTools.DamageSystem;
using UnityEngine;

namespace Example.Scripts.DamageSystem.Filters
{
    public abstract class DamageFilterBehaviour : MonoBehaviour
    {
        [SerializeField] private DamageComputer computer = default;

        public DamageComputer Computer
        {
            get => computer;
            set => computer = value;
        }
        
        public abstract DamageFilter GetFilter();
        
        protected virtual void OnEnable()
        {
            Computer.AddFilter(GetFilter());
        }
        
        protected virtual void OnDisable()
        {
            Computer.RemoveFilter(GetFilter());
        }
    }
}