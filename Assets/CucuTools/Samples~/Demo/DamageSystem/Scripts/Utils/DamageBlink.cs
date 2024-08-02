using System;
using System.Collections;
using CucuTools.DamageSystem;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.Utils
{
    public class DamageBlink : MonoBehaviour
    {
        [SerializeField] protected Color blinkColor = Color.red;
        [SerializeField] protected float blinkDuration = 0.1f;
        [Space]
        [SerializeField] private DamageReceiver damageReceiver;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Coroutine _blinking;
        private Color _baseColor;
        
        public void Blink(Color color, float duration)
        {
            if (_blinking != null) StopCoroutine(_blinking);
            _blinking = StartCoroutine(Blinking(color, duration));
        }
        
        private IEnumerator Blinking(Color color, float duration)
        {
            SetColor(color);
            yield return new WaitForSeconds(duration);
            SetColor(_baseColor);
        }

        private void SetColor(Color color)
        {
            if (spriteRenderer)
            {
                spriteRenderer.color = color;
            }
        }
        
        private Color GetColor()
        {
            return spriteRenderer ? spriteRenderer.color : default;
        }
        
        protected virtual void HandleDamageEvent(DamageEvent damageEvent)
        {
            if (damageEvent.damage.amount > 0)
            {
                Blink(blinkColor, blinkDuration);
            }
        }

        private void Awake()
        {
            if (damageReceiver == null) damageReceiver = GetComponent<DamageReceiver>();
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (damageReceiver)
            {
                damageReceiver.OnDamageReceived += HandleDamageEvent;
            }

            _baseColor = GetColor();
        }
        
        private void OnDisable()
        {
            if (damageReceiver)
            {
                damageReceiver.OnDamageReceived -= HandleDamageEvent;
            }
            
            if (_blinking != null) StopCoroutine(_blinking);
            
            SetColor(_baseColor);
        }
    }
}