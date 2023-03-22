using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem.Visions.Dragging
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class DraggableBase : MonoBehaviour
    {
        public abstract bool isOn { get; set; }
        public abstract bool isDragging { get; }
        
        public Vector3 position => rigid.position;
        public Quaternion rotation => rigid.rotation;
        
        public abstract UnityEvent<bool> onDragChanged { get; }
        public abstract DragController controller { get; }
        public abstract Rigidbody rigid { get; }

        public abstract void Pick(DragController drag);
        public abstract void Drop();
    }
}