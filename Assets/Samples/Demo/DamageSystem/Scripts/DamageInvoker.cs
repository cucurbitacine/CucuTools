using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts
{
    public class DamageInvoker : MonoBehaviour
    {
        public DamageSource source;
        public DamageReceiver receiver;

        public void Invoke()
        {
            if (source && receiver)
            {
                source.SendDamage(receiver);
            }
        }
    }
}
