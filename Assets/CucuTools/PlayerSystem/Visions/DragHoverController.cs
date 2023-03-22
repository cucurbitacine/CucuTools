using CucuTools.PlayerSystem.Visions.Dragging;
using CucuTools.PlayerSystem.Visions.Hovering;
using UnityEngine;

namespace CucuTools.PlayerSystem.Visions
{
    public class DragHoverController : MonoBehaviour
    {
        public DragController drag;
        public HoverController hover;

        private void DragChange(DragInfo info)
        {
            hover.isOn = !info.dragging;
        }
        
        private void OnEnable()
        {
            drag.onDragChanged.AddListener(DragChange);
        }

        private void OnDisable()
        {
            drag.onDragChanged.RemoveListener(DragChange);
        }
    }
}