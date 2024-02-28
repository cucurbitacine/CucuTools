using UnityEngine;
using UnityEngine.Events;

namespace Samples.Demo.Scripts.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class SpriteButton : MonoBehaviour
    {
        public SpriteRenderer targetSprite;

        [Space]
        public bool lockHue = false;
        public bool lockSaturation = false;
        public bool lockValue = false;
        public bool lockAlpha = false;
        
        [Space]
        public Color normalColor = new Color(1.00f, 1.00f, 1.00f);
        public Color hoverColor  = new Color(0.80f, 0.80f, 0.80f);
        public Color pressColor  = new Color(0.70f, 0.70f, 0.70f);
        public Color selectColor = new Color(0.90f, 0.90f, 0.90f);
        
        [Space]
        public UnityEvent onClicked = new UnityEvent();
        
        [Space]
        public bool hovered;
        public bool pressed;
        public bool selected;

        public void Click()
        {
            onClicked.Invoke();
        }

        public void Select(bool value)
        {
            selected = value;

            UpdateColor();
        }
        
        public void Select()
        {
            Select(true);
        }
        
        public void Deselect()
        {
            Select(false);
        }
        
        public void UpdateColor()
        {
            if (targetSprite)
            {
                var color = normalColor;
                
                if (selected)
                {
                    color = selectColor;
                }
                
                if (hovered)
                {
                    color = hoverColor;
                }
                
                if (pressed)
                {
                    color = pressColor;
                }
                
                Color.RGBToHSV(targetSprite.color, out var hueLocked, out var sLocked, out var vLocked);
                var alphaLocked = targetSprite.color.a;

                Color.RGBToHSV(color, out var hue, out var saturation, out var value);
                var alpha = color.a;

                if (lockHue)
                {
                    hue = hueLocked;
                }
                
                if (lockSaturation)
                {
                    saturation = sLocked;
                }
                
                if (lockValue)
                {
                    value = vLocked;
                }

                color = Color.HSVToRGB(hue, saturation, value);
                color.a = lockAlpha ? alphaLocked : alpha;
                
                targetSprite.color = color;
            }
        }
        
        private void OnMouseEnter()
        {
            hovered = true;
            
            UpdateColor();
        }

        private void OnMouseDown()
        {
            pressed = true;
            
            UpdateColor();
            
            if (hovered)
            {
                Click();
            }
        }

        private void OnMouseUp()
        {
            pressed = false;
            
            UpdateColor();
        }
        
        private void OnMouseExit()
        {
            hovered = false;

            UpdateColor();
        }

        private void Awake()
        {
            UpdateColor();
        }

        private void OnValidate()
        {
            UpdateColor();
        }
    }
}