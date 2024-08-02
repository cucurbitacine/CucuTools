using System;
using UnityEngine;
using UnityEngine.Events;

namespace Samples.Demo.Scripts.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class SpriteButton : MonoBehaviour
    {
        public SpriteRenderer targetSprite;

        [Space]
        public Color normalColor = new Color(1.00f, 1.00f, 1.00f);
        public Color hoverColor  = new Color(0.80f, 0.80f, 0.80f);
        public Color pressColor  = new Color(0.70f, 0.70f, 0.70f);
        
        [Space]
        public UnityEvent onClicked = new UnityEvent();
        
        [Space]
        public bool hovered;
        public bool pressed;

        public void Click()
        {
            onClicked.Invoke();
        }
        
        public void UpdateColor()
        {
            if (targetSprite)
            {
                var color = normalColor;
                
                if (hovered)
                {
                    color = hoverColor;
                }
                
                if (pressed)
                {
                    color = pressColor;
                }
                
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

        private void OnEnable()
        {
            UpdateColor();
        }

        private void OnDisable()
        {
            hovered = false;
            pressed = false;
        }

        private void OnValidate()
        {
            UpdateColor();
        }
    }
}