using Samples.Demo.Scripts.UI;
using UnityEngine;

namespace Samples.Demo.Extensions
{
    public class HandleColorChange : MonoBehaviour
    {
        public Color color;

        [Space]
        public SpriteRenderer handle;
        public SpriteRenderer sprite;
        
        [Space]
        public SpriteSlider sliderColor;
        public SpriteSlider sliderGradient;
        
        [Space]
        public LineGradient line;

        public void UpdateSpriteColor()
        {
            if (handle)
            {
                handle.color = color;
            }

            if (sprite)
            {
                sprite.color = color;
            }
        }

        public void UpdateColor(Color newColor)
        {
            color = newColor;

            UpdateSpriteColor();
        }

        public void UpdateGradient(float value)
        {
            if (line)
            {
                var newColor = line.gradient.Evaluate(value);

                UpdateColor(newColor);
            }
        }

        public void UpdateGradient()
        {
            if (sliderColor)
            {
                UpdateGradient(sliderColor.value);
            }
        }

        private void OnEnable()
        {
            if (sliderColor)
            {
                sliderColor.onValueChanged.AddListener(UpdateGradient);
            }

            if (sliderGradient)
            {
                sliderGradient.onValueChanged.AddListener(_ => UpdateGradient());
            }
        }

        private void OnDisable()
        {
            if (sliderColor)
            {
                sliderColor.onValueChanged.RemoveListener(UpdateGradient);
            }

            if (sliderGradient)
            {
                sliderGradient.onValueChanged.RemoveListener(_ => UpdateGradient());
            }
        }

        private void Start()
        {
            UpdateGradient();
        }
    }
}