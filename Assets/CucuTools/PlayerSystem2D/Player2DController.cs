using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public abstract class Player2DController : CucuBehaviour
    {
        private Rigidbody2D _rigid = null;
        
        public Rigidbody2D rigid => GetOrAddRigidbody();

        private Rigidbody2D GetOrAddRigidbody()
        {
            if (_rigid != null) return _rigid;
            _rigid = GetComponent<Rigidbody2D>();
            if (_rigid != null) return _rigid;
            _rigid = gameObject.AddComponent<Rigidbody2D>();
            return _rigid;
        }
    }
}