using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.Serialization
{
    /// <summary>
    /// Serializable GameObject Entity
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CucuIdentity))]
    public sealed class SerializableGameObject : CucuBehaviour
    {
        private CucuIdentity _cuid;
        [SerializeField] private List<ComponentReference> references = new List<ComponentReference>();

        /// <summary>
        /// Unique Identificator of GameObject
        /// </summary>
        public CucuIdentity Cuid => _cuid ??= CucuIdentity.GetOrAdd(gameObject);

        /// <summary>
        /// List of serializable components
        /// </summary>
        public List<ComponentReference> References => references;

        /// <summary>
        /// Update List of Components
        /// </summary>
        public void UpdateComponents()
        {
            SerializationSettings.Instance.UpdateReferences(gameObject, references);
        }
        
        /// <summary>
        /// Serialize GameObject
        /// </summary>
        /// <returns></returns>
        public SerializedGameObject Serialize()
        {
            var list = new List<SerializedComponent>();

            foreach (var reference in References)
            {
                if (SerializationSettings.Instance.TrySerializeComponent(reference, out var serializedComponent))
                {
                    list.Add(serializedComponent);
                }
            }

            return new SerializedGameObject(Cuid.Guid, list.ToArray());
        }

        /// <summary>
        /// Deserialize GameObject
        /// </summary>
        /// <param name="serializedGameObject"></param>
        public void Deserialize(SerializedGameObject serializedGameObject)
        {
            var serializedComponents = serializedGameObject.components.ToDictionary(c => c.typeName, c => c);

            foreach (var reference in References)
            {
                if (!reference.IsEnabled) continue;
                
                if (serializedComponents.TryGetValue(reference.ComponentType.FullName, out var serializedComponent))
                {
                    SerializationSettings.Instance.TryDeserialize(serializedComponent, reference);
                }
            }
        }

        private void OnEnable()
        {
            UpdateComponents();
        }

        private void OnValidate()
        {
            UpdateComponents();
        }
    }
}