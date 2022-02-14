using System;
using System.Collections;
using CucuTools.Colors;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Scripts.DamageSystem
{
    public class AvatarHitEffect : MonoBehaviour
    {
        private Coroutine _playing;

        [Range(0f, 1f)]
        public float alphaModifier = 1f;
        
        [Range(0f, 1f)]
        public float progress = 0f;
        [Range(0f, 1f)]
        public float alpha = 0f;

        [Space]
        public float durationIn = 1f;
        public AnimationCurve curveIn = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public float durationOut = 1f;
        public AnimationCurve curveOut = AnimationCurve.Linear(0f, 1f, 1f, 0f);

        [Space]
        public Image Image; 
        
        public void Play()
        {
            if (_playing != null) StopCoroutine(_playing);
            _playing = StartCoroutine(Playing());
        }

        private IEnumerator Playing()
        {
            var timer = progress * durationIn;
            while (timer < durationIn)
            {
                progress = timer / durationIn;
                alpha = curveIn.Evaluate(progress) * alphaModifier;
                Image.color = Image.color.AlphaTo(alpha);
                    
                timer += Time.deltaTime;
                yield return null;
            }

            progress = 1f;
            alpha = alphaModifier;
            Image.color = Image.color.AlphaTo(alpha);
            
            timer = durationOut;
            while (timer > 0)
            {
                progress = timer / durationOut;
                alpha = curveOut.Evaluate(1f - progress) * alphaModifier;
                Image.color = Image.color.AlphaTo(alpha);
                
                timer -= Time.deltaTime;
                yield return null;
            }
            
            progress = 0f;
            alpha = 0f;
            Image.color = Image.color.AlphaTo(alpha);
        }

        private void Awake()
        {
            progress = 0f;
            alpha = 0f;
            Image.color = Image.color.AlphaTo(alpha);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Play();
            }
        }
    }
}
