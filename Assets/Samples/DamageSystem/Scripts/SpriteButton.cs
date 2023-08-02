using UnityEngine;
using UnityEngine.Events;

namespace Samples.DamageSystem.Scripts
{
    public class SpriteButton : MonoBehaviour
    {
        public SpriteRenderer targetSprite;
        
        [Space]
        public Color normalColor = Color.white;
        public Color selectColor = new Color(0.9f,0.9f,0.9f);
        public Color hoverColor = new Color(0.8f,0.8f,0.8f);
        public Color pressColor = new Color(0.4f,0.4f,0.4f);
        
        [Space]
        public UnityEvent onClicked = new UnityEvent();
        
        [Space]
        public bool selected;
        public bool hovered;
        public bool pressed;

        public void Click()
        {
            onClicked.Invoke();
        }

        public void Select(bool value = true)
        {
            selected = value;
        }
        
        public void Deselect()
        {
            Select(false);
        }
        
        public void UpdateColor()
        {
            if (targetSprite)
            {
                targetSprite.color = normalColor;

                if (selected)
                {
                    targetSprite.color = selectColor;
                }
                
                if (hovered)
                {
                    targetSprite.color = hoverColor;
                }
                
                if (pressed)
                {
                    targetSprite.color = pressColor;
                }
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