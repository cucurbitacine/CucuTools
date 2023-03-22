using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem.Visions.Hovering
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class HoverableBase : MonoBehaviour
    {
        public abstract bool isOn { get; set; }
        public abstract bool isHovering { get; set; }
        public abstract UnityEvent<bool> onHoverChange { get; }
        
        public abstract void Hover(bool value);
    }

    public abstract class HoverableBehaviour : HoverableBase
    {
        [Space]
        [SerializeField] private bool _isOn = true;
        [Space]
        [SerializeField] private bool _isHovering = false;
        [Space]
        [SerializeField] private UnityEvent<bool> _onHovered = new UnityEvent<bool>();
        
        public override bool isOn
        {
            get => _isOn;
            set => _isOn = value;
        }

        public override bool isHovering
        {
            get => _isHovering;
            set => _isHovering = value;
        }

        public override UnityEvent<bool> onHoverChange => _onHovered ??= new UnityEvent<bool>();
    }
}