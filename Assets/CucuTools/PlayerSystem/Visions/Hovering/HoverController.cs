using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.PlayerSystem.Visions.Hovering
{
    public class HoverController : MonoBehaviour
    {
        [Space]
        [SerializeField] private bool _isOn = true;

        [Header("References")]
        public TouchController touch = null;

        private readonly Dictionary<Rigidbody, HoverableBase> _hoverCache = new Dictionary<Rigidbody, HoverableBase>();

        public bool isOn
        {
            get => _isOn;
            set
            {
                if (_isOn == value) return;
                _isOn = value;
                TouchChange(touch.current);
            }
        }

        public void Hover(HoverableBase hoverable, bool hovering)
        {
            if (isOn && hoverable.isOn)
            {
                if (hoverable.isHovering != hovering) hoverable.Hover(hovering);
            }
            else
            {
                if (hoverable.isHovering) hoverable.Hover(false);
            }
        }
        
        private bool TryGetHoverable(Rigidbody rgb, out HoverableBase hoverable)
        {
            if (rgb == null)
            {
                hoverable = null;
                return false;
            }
            
            if (!_hoverCache.TryGetValue(rgb, out hoverable))
            {
                hoverable = rgb.GetComponent<HoverableBase>();
                    
                _hoverCache.TryAdd(rgb, hoverable);
            }

            return hoverable != null;
        }
        
        private void TouchChange(TouchInfo info)
        {
            if (TryGetHoverable(info.hit.rigidbody, out var hoverable))
            {
                Hover(hoverable, info.inTouch);
            }
        }

        private void OnEnable()
        {
            touch.onTouchChanged.AddListener(TouchChange);
        }

        private void OnDisable()
        {
            touch.onTouchChanged.RemoveListener(TouchChange);
        }
    }
}