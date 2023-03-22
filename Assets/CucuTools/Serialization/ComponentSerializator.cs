using System;
using UnityEngine;

namespace CucuTools.Serialization
{
    public abstract class ComponentSerializator : ScriptableObject
    {
        public const string Serializators = "Component Serializators/";
        
        public abstract Type ComponentType { get; } 
        public abstract bool TryGetComponent(GameObject gameObject, out Component component);
        public abstract bool TrySerialize(Component component, out SerializedComponent serializedComponent);
        public abstract bool TryDeserialize(SerializedComponent serializedComponent, ref Component component);
    }
    
    public abstract class ComponentSerializator<TComponent> : ComponentSerializator where TComponent : Component
    {
        public abstract byte[] Serialize(TComponent component);
        public abstract void Deserialize(byte[] bytes, ref TComponent component);
        
        #region ComponentSerializator

        public override Type ComponentType => typeof(TComponent);
        
        public override bool TryGetComponent(GameObject gameObject, out Component component)
        {
            component = gameObject.GetComponent<TComponent>();

            return component != null;
        }
        
        public override bool TrySerialize(Component component, out SerializedComponent serializedComponent)
        {
            if (ComponentType != component.GetType())
            {
                serializedComponent = null;
                return false;
            }

            var tComponent = component as TComponent;
            
            serializedComponent = new SerializedComponent()
            {
                typeName = ComponentType.FullName,
                bytes = Serialize(tComponent),
            };
            return true;
        }

        public override bool TryDeserialize(SerializedComponent serializedComponent, ref Component component)
        {
            if (ComponentType != component.GetType())
            {
                return false;
            }

            var tComponent = component as TComponent;
            
            Deserialize(serializedComponent.bytes, ref tComponent);
            
            return true;
        }

        #endregion
    }

    public abstract class ComponentSerializator<TComponent, TData> : ComponentSerializator<TComponent> where TComponent : Component
    {
        [SerializeField] private Serializator serializator = null;
        
        public Serializator Serializator => serializator ??= SerializationSettings.Instance.Serializator;

        public abstract TData GetData(TComponent component);
        public abstract void SetData(TData data, ref TComponent component);
        
        #region ComponentSerializator

        public override byte[] Serialize(TComponent component)
        {
            return Serializator.Serialize(GetData(component));
        }

        public override void Deserialize(byte[] bytes, ref TComponent component)
        {
            SetData(Serializator.Deserialize<TData>(bytes), ref component);
        }

        #endregion
        

    }
}