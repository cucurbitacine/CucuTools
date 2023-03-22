using UnityEngine;

namespace CucuTools.PlayerSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class GroundPlatform : MonoBehaviour
    {
        private Rigidbody _rigid = null;

        public Rigidbody rigid => _rigid ??= GetComponent<Rigidbody>();
    }
}