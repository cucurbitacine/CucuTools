using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    [DisallowMultipleComponent]
    public class HitBox : MonoBehaviour
    {
        [Space]
        public DamageReceiver receiver;

        private void Awake()
        {
            if (receiver == null) receiver = GetComponent<DamageReceiver>();
        }
        
        private void OnValidate()
        {
            if (receiver == null) receiver = GetComponent<DamageReceiver>();
        }
    }
}