using UnityEngine.Events;

namespace CucuTools.PlayerSystem.Visions.Hovering
{
    public class HoverableReference : HoverableBase
    {
        public HoverableBase target = null;

        public override bool isOn
        {
            get => target.isOn;
            set => target.isOn = value;
        }

        public override bool isHovering
        {
            get => target.isHovering;
            set => target.isHovering = value;
        }

        public override UnityEvent<bool> onHoverChange => target.onHoverChange;
        
        public override void Hover(bool value)
        {
            target.Hover(value);
        }
    }
}