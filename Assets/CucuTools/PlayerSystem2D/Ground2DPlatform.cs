using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ground2DPlatform : MonoBehaviour
    {
        private Rigidbody2D _rigid = null;

        public Rigidbody2D rigid => _rigid ??= GetComponent<Rigidbody2D>();
    }
}