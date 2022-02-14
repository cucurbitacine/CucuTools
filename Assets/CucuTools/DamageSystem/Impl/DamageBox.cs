using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    public abstract class DamageBox : MonoBehaviour
    {
        [SerializeField] private bool isEnabled = true;
        
        [Space]
        [SerializeField] private DamageSource source = default;
        
        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public DamageSource Source
        {
            get => source;
            set => source = value;
        }
    }
}