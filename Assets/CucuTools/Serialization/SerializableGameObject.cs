using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.Serialization
{
    /// <summary>
    /// Serializable GameObject Entity
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SerializableGameObject : MonoBehaviour
    {
        private CucuIdentity _cuid;

        public bool IsValid() => Cuid != null;
        
        /// <summary>
        /// Unique Identificator of GameObject
        /// </summary>
        public CucuIdentity Cuid => _cuid != null ? _cuid : (_cuid = GameObjectRef?.GetComponent<CucuIdentity>());

        /// <summary>
        /// List of serializable components
        /// </summary>
        public List<SerializableComponent> SerializableComponents { get; } = new List<SerializableComponent>();

        /// <summary>
        /// Reference to GameObject
        /// </summary>
        public GameObject GameObjectRef => gameObject;
        
        /// <summary>
        /// Update List of Components
        /// </summary>
        public void UpdateComponents()
        {
            SerializableComponents.Clear();
            
            SerializableComponents.AddRange(GetComponents<SerializableComponent>());
        }
        
        /// <summary>
        /// Serialize GameObject
        /// </summary>
        /// <returns></returns>
        public SerializedGameObject Serialize()
        {
            return new SerializedGameObject(Cuid.Guid, SerializableComponents
                .Where(c => c.IsValid && c.IsEnabled)
                .Select(c => c.SerializeComponent()).ToArray());
        }

        /// <summary>
        /// Deserialize GameObject
        /// </summary>
        /// <param name="serializedGameObject"></param>
        public void Deserialize(SerializedGameObject serializedGameObject)
        {
            var serializedComponents = serializedGameObject.components.ToDictionary(c => c.typeName, c => c);

            foreach (var serializableComponent in SerializableComponents)
            {
                if (!serializableComponent.IsEnabled) continue;
                
                if (serializedComponents.TryGetValue(serializableComponent.ComponentType.FullName, out var serializedComponent))
                {
                    serializableComponent.DeserializeComponent(serializedComponent);
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