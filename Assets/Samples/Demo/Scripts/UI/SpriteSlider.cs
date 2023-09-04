using System;
using UnityEngine;
using UnityEngine.Events;

namespace Samples.Demo.Scripts.UI
{
    public class SpriteSlider : MonoBehaviour
    {
        public float value = 0f;

        [Space]
        public float minValue = 0f;
        public float maxValue = 1f;

        [Space]
        public bool wholeNumbers = false;
        
        [Space]
        public SpriteRenderer background = null;
        public SpriteRenderer fill = null;
        public DraggableSprite handle = null;

        [Space]
        public UnityEvent<float> onValueChanged = new UnityEvent<float>();
        
        private float percent => (value - minValue) / (maxValue - minValue);

        public static int GetInt(float value)
        {
            var floor = Mathf.FloorToInt(value);
            var ceil =  Mathf.CeilToInt(value);


            var left = value - floor;
            var right = ceil - value;

            return left < right ? floor : ceil;
        }
        
        private void UpdateLimits()
        {
            if (wholeNumbers)
            {
                minValue = GetInt(minValue);
                maxValue = GetInt(maxValue);
            }
            
            if (maxValue <= minValue)
            {
                maxValue = wholeNumbers ? minValue + 1 : minValue + 0.01f;
            }
            
            value = Mathf.Clamp(value, minValue, maxValue);

            if (wholeNumbers)
            {
                value = GetInt(value);
            }
        }
        
        private void UpdateHandle()
        {
            if (handle && background)
            {
                handle.areaCenter = background.transform.position;
                handle.areaSize = background.size;
            }
            
            if (handle)
            {
                handle.fixedArea = true;
                handle.freezeY = true;

                handle.transform.position = handle.areaCenter + Vector2.right * handle.areaSize.x * (percent - 0.5f);
            }
        }

        private void UpdateFill()
        {
            if (fill && background)
            {
                var fillSize = fill.size;
                var backgroundSize = background.size;
                
                fillSize.x = backgroundSize.x * percent;
                
                var sprites = fill.GetComponentsInChildren<SpriteRenderer>();
                foreach (var sprite in sprites)
                {
                    sprite.transform.position = background.transform.position + Vector3.right *
                        (fillSize.x - backgroundSize.x) * 0.5f;
                    
                    sprite.size = fillSize;
                }
            }
        }

        public void UpdateSlider()
        {
            UpdateLimits();
            
            UpdateHandle();

            UpdateFill();
        }
        
        private void UpdateValue()
        {
            if (handle)
            {
                var handleValue = (handle.transform.position.x - handle.areaCenter.x) / handle.areaSize.x + 0.5f;

                value = Mathf.Lerp(minValue, maxValue, handleValue);
            }

            UpdateSlider();

            onValueChanged.Invoke(value);
        }
        
        private void OnEnable()
        {
            if (handle)
            {
                handle.onPositionChanged.AddListener(UpdateValue);
            }
        }

        private void OnValidate()
        {
            UpdateSlider();
        }

        private void OnDrawGizmos()
        {
            if (background)
            {
                Gizmos.DrawWireCube(background.transform.position, background.size);
            }
        }
    }
}