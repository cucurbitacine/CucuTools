using CucuTools.PlayerSystem.Visions.Dragging;
using CucuTools.PlayerSystem.Visions.Hovering;
using UnityEngine;

namespace CucuTools.PlayerSystem.Tools
{
    public class HoverAndDrag : MonoBehaviour
    {
        public HoverableBase hoverable;
        public DraggableBase draggable;

        private void Awake()
        {
            if (hoverable == null) hoverable = GetComponent<HoverableBase>();
            if (draggable == null) draggable = GetComponent<DraggableBase>();
        }

        private void Update()
        {
            hoverable.isOn = draggable.isOn;

            if (!hoverable.isOn && hoverable.isHovering)
            {
                hoverable.Hover(false);
            }
        }
    }
}