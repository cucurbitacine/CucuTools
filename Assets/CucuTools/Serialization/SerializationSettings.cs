using System.Collections.Generic;
using System.Linq;
using CucuTools.Serialization.Impl;
using UnityEngine;

namespace CucuTools.Serialization
{
    [CreateAssetMenu(menuName = Cucu.AddComponent + Cucu.SerializationGroup + ObjectName, fileName = ObjectName, order = 0)]
    public class SerializationSettings : ScriptableObject
    {
        public const string ObjectName = nameof(SerializationSettings);
        
        private static SerializationSettings _instance = null;
        
        public static SerializationSettings Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = Resources.Load<SerializationSettings>(ObjectName);
                
                if (_instance != null) return _instance;
                
                _instance = CreateInstance<SerializationSettings>();
                
                return _instance;
            }
        }

        [SerializeField] private Serializator serializator = null;

        public ComponentSerializator[] ComponentSerializators;
        
        public Serializator Serializator
        {
            get
            {
                if (serializator != null) return serializator;

                serializator = CreateInstance<SerializatorByJsonUtility>();
                
                return serializator;
            }
        }

        public void UpdateReferences(GameObject gameObject, List<ComponentReference> references)
        {
            foreach (var ser in ComponentSerializators)
            {
                if (references.Any(r => r.ComponentType == ser.ComponentType)) continue;

                if (ser.TryGetComponent(gameObject, out var component))
                {
                    var reference = new ComponentReference(component);
                    references.Add(reference);
                }
            }
        }
        
        public List<ComponentReference> GetReferences(GameObject gameObject)
        {
            var list = new List<ComponentReference>();

            foreach (var ser in ComponentSerializators)
            {
                if (ser.TryGetComponent(gameObject, out var component))
                {
                    var reference = new ComponentReference(component);
                    list.Add(reference);
                }
            }

            return list;
        }

        public bool TrySerializeComponent(ComponentReference reference, out SerializedComponent serializedComponent)
        {
            foreach (var ser in ComponentSerializators)
            {
                if (ser.TrySerialize(reference.Component, out serializedComponent))
                {
                    return true;
                }
            }

            serializedComponent = null;
            return false;
        }
        
        public bool TryDeserialize(SerializedComponent serializedComponent, ComponentReference reference)
        {
            foreach (var ser in ComponentSerializators)
            {
                var component = reference.Component;
                
                if (ser.TryDeserialize(serializedComponent, ref component))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
