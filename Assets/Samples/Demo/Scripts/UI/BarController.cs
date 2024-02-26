using UnityEngine;

namespace Samples.Demo.Scripts.UI
{
    public class BarController : MonoBehaviour
    {
        [Range(0, 1)] public float value = 0.75f;

        [Space]
        [Min(0)] public float width = 2f;
        [Min(0)] public float height = 0.5f;
        
        [Space]
        public SpriteRenderer spriteValue;
        public SpriteRenderer spriteBackground;
        public SpriteRenderer spriteOutline;

        private Vector2 sizeBar => new Vector2(width, height);
        private Vector2 sizeValue => new Vector2(Mathf.Lerp(0f, width, value), height);
        
        public void UpdateBar()
        {
            if (spriteValue)
            {
                spriteValue.size = sizeValue;
            }
            
            if (spriteBackground)
            {
                spriteBackground.size = sizeBar;
            }
            
            if (spriteOutline)
            {
                spriteOutline.size = sizeBar;
            }
        }
        
        public void SetValue(float newValue)
        {
            value = newValue;
            
            UpdateBar();
        }

        public void SetValue(int newValue, int maxValue)
        {
            if (maxValue != 0)
            {
                SetValue((float)newValue / maxValue);
            }
        }
        
        private void Start()
        {
            UpdateBar();
        }
    }
}
