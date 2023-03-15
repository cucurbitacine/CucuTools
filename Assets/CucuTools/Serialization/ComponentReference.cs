using System;
using UnityEngine;

namespace CucuTools.Serialization
{
    /// <summary>
    /// Serializable Component Entity
    /// </summary>
    [Serializable]
    public class ComponentReference
    {
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private Component component = null;

        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public Component Component
        {
            get => component;
            protected set => component = value;
        }

        public virtual Type ComponentType => component.GetType();

        public ComponentReference()
        {
        }
        
        public ComponentReference(Component component)
        {
            Component = component;
        }
    }

    /// <summary>
    /// Typed Serializable Component Entity
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    [Serializable]
    public class ComponentReference<TComponent> : ComponentReference where TComponent : Component
    {
        /// <summary>
        /// Component
        /// </summary>
        public TComponent Reference
        {
            get => Component as TComponent;
            set => Component = value;
        }

        /// <inheritdoc />
        public sealed override Type ComponentType => typeof(TComponent);

        public ComponentReference() : base()
        {
        }

        public ComponentReference(TComponent component) : this()
        {
            Reference = component;
        }
    }
}