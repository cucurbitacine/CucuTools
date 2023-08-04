using UnityEngine;

namespace CucuTools.DamageSystem
{
    /// <summary>
    /// Hit Box which interacting with <see cref="DamageBox"/> as Receiver of damage
    /// <seealso cref="DamageReceiver"/>
    /// <seealso cref="DamageBox"/>
    /// </summary>
    [DisallowMultipleComponent]
    public class HitBox : MonoBehaviour
    {
        [Space]
        public DamageReceiver receiver;

        protected virtual void OnValidate()
        {
            if (receiver == null) receiver = GetComponent<DamageReceiver>();
        }
    }
}