using System;
using CucuTools;
using Samples.Demo.Scripts.UI;
using UnityEngine;

namespace Samples.Demo.Extensions
{
    public class GradientController : MonoBehaviour
    {
        public GradientGenerator generator;
        public SpriteSlider slider;

        private void UpdateGradient(float value)
        {
            var intValue = SpriteSlider.GetInt(value);

            var palettes = (ColorPaletteType[])Enum.GetValues(typeof(ColorPaletteType));

            generator.SetPalette(palettes[intValue % palettes.Length]);
        }
        
        private void Start()
        {
            if (slider)
            {
                var palettes = (ColorPaletteType[])Enum.GetValues(typeof(ColorPaletteType));
                
                slider.minValue = 0;
                slider.maxValue = palettes.Length - 1;
                slider.wholeNumbers = true;
                slider.value = 0;

                if (generator)
                {
                    generator.SetPalette(palettes[0]);
                }

                slider.UpdateSlider();
                
                slider.onValueChanged.AddListener(UpdateGradient);   
            }
        }
    }
}