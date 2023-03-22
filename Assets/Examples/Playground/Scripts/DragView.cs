using CucuTools.PlayerSystem.Visions.Dragging;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class DragView : MonoBehaviour
    {
        public float damp = 16f;
        
        [Space] public CanvasGroup image = null;
        
        [Space]
        public DragController drag = null;
        
        private void Update()
        {
            if (image != null && drag != null)
            {
                var target = 0f;

                if (drag.isOn && !drag.isDragging && drag.touch.touchSomething)
                {
                    if (drag.TryGetDraggable(drag.touch.current.hit.rigidbody, out var draggable))
                    {
                        target = draggable.isOn && !draggable.isDragging ? 1 : 0;
                    }
                }
                
                image.alpha = Mathf.Lerp(image.alpha, target, Time.deltaTime * damp);
            }
        }
    }
}