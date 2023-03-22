using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem.Visions.Dragging
{
    public class DraggableBehaviour : DraggableBase
    {
        [SerializeField] private bool _temp;
        
        [Space]
        [SerializeField] private bool _isOn = true;
        
        [Space]
        [SerializeField] private bool _isDragging = false;
        
        [Space]
        [SerializeField] private UnityEvent<bool> _onDragChanged = new UnityEvent<bool>();
        
        [Space]
        [SerializeField] private DragController _controller = null;
        
        private Rigidbody _rigid = null;

        public override bool isOn
        {
            get => _isOn;
            set
            {
                if (_isOn == value) return;
                _isOn = value;
                if (isDragging) controller.Drop();
            }
        }

        public override bool isDragging => _isDragging;

        public override UnityEvent<bool> onDragChanged => _onDragChanged ??= new UnityEvent<bool>();

        public override DragController controller => _controller;
        
        public override Rigidbody rigid => _rigid ??= GetComponent<Rigidbody>();
        
        public override void Pick(DragController drag)
        {
            _controller = drag;
            _isDragging = true;
            
            onDragChanged.Invoke(true);
        }

        public override void Drop()
        {
            _controller = null;
            _isDragging = false;
            
            onDragChanged.Invoke(false);
        }

        private void OnDrawGizmos()
        {
            if (rigid != null) Gizmos.DrawLine(position, position + rigid.velocity);
        }
    }
}
