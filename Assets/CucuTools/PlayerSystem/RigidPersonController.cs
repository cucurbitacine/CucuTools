using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public abstract class RigidPersonController : PersonController
    {
        #region SerializeField

        [Header("Person Settings")]
        [SerializeField] private PersonInfo _info = new PersonInfo();
        [SerializeField] private PersonSettings _settings = new PersonSettings();
        [SerializeField] private GroundController _ground = null;
        [SerializeField] private Transform _head = null;

        [Header("Rigid Settings")]
        [Min(0f)] public float height = 2f;
        [Min(0f)] public float radius = 0.5f;
        
        [Space]
        [Min(0f)] public float speedChangeRate = 24f;
        [Min(0f)] public float rotationChangeRate = 24f;
        [Min(0f)] public float inertionSpeedFade = 0.05f;
        
        [Space]
        [Range(0f, 90f)] public float fovUpper = 89;
        [Range(0f, 90f)] public float fovBottom = 89;
        
        [Space]
        [Min(0f)] public float jumpTimeout = 0.05f;

        [Space]
        [Range(0, 1)]
        public float airMoveControl = 1f;
        public Vector3 velocityAdditional = Vector3.zero;

        #endregion

        private Rigidbody _rigid = null;
        private CapsuleCollider _capsule = null;

        public abstract Vector3 velocitySelf { get; }
        public abstract Vector3 velocityExternal { get; }
        
        public override PersonInfo info => _info;
        public override PersonSettings settings => _settings;
        public override GroundController ground => GetOrAddGroundController();
        public override Transform head => _head != null ? _head : (_head = rigid.transform);
        
        public Vector3 velocity => velocitySelf + velocityExternal;
        public Vector3 normal => rigid.transform.up;
        
        public Rigidbody rigid => GetOrAddRigidbody();
        public CapsuleCollider capsule => GetOrAddCapsule();
        
        protected virtual GroundController GetOrAddGroundController()
        {
            if (_ground != null) return _ground;
            _ground = GetComponent<GroundController>();
            if (_ground != null) return _ground;
            _ground = gameObject.AddComponent<GroundController>();
            return _ground;
        }
        
        protected virtual Rigidbody GetOrAddRigidbody()
        {
            if (_rigid != null) return _rigid;
            _rigid = GetComponent<Rigidbody>();
            if (_rigid != null) return _rigid;
            _rigid = gameObject.AddComponent<Rigidbody>();
            return _rigid;
        }
        
        protected virtual CapsuleCollider GetOrAddCapsule()
        {
            if (_capsule != null) return _capsule;
            _capsule = GetComponent<CapsuleCollider>();
            if (_capsule != null) return _capsule;
            _capsule = gameObject.AddComponent<CapsuleCollider>();
            return _capsule;
        }
    }
}