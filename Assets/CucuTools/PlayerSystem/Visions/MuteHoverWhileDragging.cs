using CucuTools.PlayerSystem.Visions.Dragging;
using CucuTools.PlayerSystem.Visions.Hovering;
using UnityEngine;

namespace CucuTools.PlayerSystem.Visions
{
    public class MuteHoverWhileDragging : MonoBehaviour
    {
        public HoverController hover = null;
        public DragController drag = null;
        
        private void DragChange(DragInfo info)
        {
            hover.mute = info.dragging;
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