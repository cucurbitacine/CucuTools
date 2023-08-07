using CucuTools.Others;
using UnityEngine;

namespace CucuTools.PlayerSystem.Visions.Hovering
{
    public class HoverController : MonoBehaviour
    {
        [Space]
        [SerializeField] private bool _isOn = true;
        [SerializeField] private bool _mute = false;
        
        [Header("References")]
        public TouchController touch = null;

        private readonly CachedComponent<Collider, HoverableBase> _hoverCache = new();

        public bool isOn
        {
            get => _isOn;
            set
            {
                if (_isOn == value) return;
                _isOn = value;
                
                UpdateHover(touch.current);
            }
        }

        public bool mute
        {
            get => _mute;
            set
            {
                if (_mute == value) return;
                _mute = value;
                
                UpdateHover(touch.current);
            }
        }
        
        public void Hover(HoverableBase hoverable, bool hovering)
        {
            if (isOn && !mute && hoverable.isOn)
            {
                if (hoverable.isHovering != hovering) hoverable.Hover(hovering);
            }
            else
            {
                if (hoverable.isHovering) hoverable.Hover(false);
            }
        }

        private void UpdateHover(TouchInfo info)
        {
            if (_hoverCache.TryGetValue(info.hit.collider, out var hoverable))
            {
                Hover(hoverable, info.inTouch);
            }
        }

        private void OnEnable()
        {
            touch.onTouchChanged.AddListener(UpdateHover);
        }

        private void OnDisable()
        {
            touch.onTouchChanged.RemoveListener(UpdateHover);
        }
    }
}