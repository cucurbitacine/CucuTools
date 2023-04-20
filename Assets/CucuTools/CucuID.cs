using System;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools
{
    [DisallowMultipleComponent]
    public sealed class CucuID : CucuBehaviour
    {
        public static implicit operator Guid(CucuID cuid) => cuid.Guid;
        
        [ReadOnlyField]
        [SerializeField] private string _guid = string.Empty;

        public Guid Guid
        {
            get => Guid.TryParse(_guid, out var guid) ? guid : (Guid = Guid.NewGuid());
            set => _guid = value.ToString();
        }
        
        private void Validate()
        {
            if (Guid == Guid.Empty) Guid = Guid.NewGuid();
        }

        private void Awake()
        {
            Validate();
        }

        private void Reset()
        {
            Validate();
        }
        
        private void OnValidate()
        {
            Validate();
        }

        public static CucuID GetOrAdd(GameObject gameObject)
        {
            if (gameObject == null) return null;
            var cuid = gameObject.GetComponent<CucuID>();
            if (cuid == null) cuid = gameObject.AddComponent<CucuID>();
            return cuid;
        }
    }
}