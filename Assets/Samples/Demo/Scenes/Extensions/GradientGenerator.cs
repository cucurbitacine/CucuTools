using CucuTools;
using UnityEngine;

namespace Samples.Demo.Scenes.Extensions
{
    public class GradientGenerator : MonoBehaviour
    {
        public ColorPaletteType paletteType;
        
        [Space]
        public LineGradient lineGradient;

        public Gradient Generate()
        {
            var palette = CucuColor.Palettes[paletteType];

            return palette.CreateGradient();
        }

        public void UpdateLine()
        {
            if (lineGradient)
            {
                lineGradient.SetGradient(Generate());
            }
        }

        public void SetPalette(ColorPaletteType palette)
        {
            paletteType = palette;
            
            UpdateLine();
        }
        
        private void OnValidate()
        {
            UpdateLine();
        }
    }
}