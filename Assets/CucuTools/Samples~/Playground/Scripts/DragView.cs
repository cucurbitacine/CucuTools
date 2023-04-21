using CucuTools.PlayerSystem.Visions.Dragging;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class DragView : MonoBehaviour
    {
        public float damp = 16f;
        
        [Space] public CanvasGroup image = null;
        
        [Space]
        public PlaygroundController playground = null;

        private PlayerManager player => playground?.player;
        private DragController drag => player?.drag;
        
        
        
        private void Update()
        {
            if (image != null && drag != null)
            {
                var target = 0f;

                if (drag.isOn && !drag.isDragging && drag.touch.touchSomething)
                {
                    var rigid = drag.touch.current.hit.rigidbody;
                    if (rigid != null && drag.TryGetDraggable(rigid, out var current))
                    {
                        target = current.isOn && !current.isDragging ? 1 : 0; 
                    }
                }
                
                image.alpha = Mathf.Lerp(image.alpha, target, Time.deltaTime * damp);
            }
        }
    }
}