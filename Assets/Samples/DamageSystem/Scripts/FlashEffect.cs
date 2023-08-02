using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samples.DamageSystem.Scripts
{
    public class FlashEffect : MonoBehaviour
    {
        public bool flashing;
        
        [Space]
        [Min(0f)]
        public float duration = 0.2f;
        public Color flashColor = Color.white;

        [Space]
        public SpriteRenderer[] sprites;
        
        private readonly Dictionary<SpriteRenderer, Color> colors = new Dictionary<SpriteRenderer, Color>();

        private Coroutine _flash;

        private void Awake()
        {
            foreach (var sprite in sprites)
            {
                colors.Add(sprite, sprite.color);
            }
        }

        private IEnumerator _Flash()
        {
            flashing = true;
            
            foreach (var sprite in sprites)
            {
                sprite.color = flashColor;
            }

            yield return new WaitForSeconds(duration);

            foreach (var sprite in sprites)
            {
                sprite.color = colors[sprite];
            }
            
            flashing = false;
        }

        public void Flash()
        {
            if (_flash != null) StopCoroutine(_flash);
            _flash = StartCoroutine(_Flash());
        }

        public void SetColor(Color color)
        {
            flashColor = color;
            
            if (flashing)
            {
                foreach (var sprite in sprites)
                {
                    sprite.color = flashColor;
                }
            }
        }
    }
}