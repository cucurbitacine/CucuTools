using System;
using System.Collections;
using CucuTools.Colors;
using CucuTools.Interactables;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Interactables
{
    public class OutlineButton : CucuInteractableEntity
    {
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        private static readonly int MainColor = Shader.PropertyToID("_Color");
        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
        
        private Coroutine c_changeColor;

        public float SpeedChangeColor = 1f;
        public float WidthOutline = 0.16f;
        public Color _color;
        
        
        [Header("Colors")]
        public Color ColorIdle;
        public Color ColorHover;
        public Color ColorPress;

        [Header("References")] public Renderer Renderer;

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                Material.SetColor(OutlineColor, _color);
                Material.SetColor(MainColor, Color.Lerp(_color, Color.white, 0.95f));
                Material.SetFloat(OutlineWidth, _color.a * WidthOutline);
            }
        }

        public Material Material => Renderer.material;

        private void SetColor(Color color)
        {
            SetColor(color, Vector3.Distance(Color.ToVector3(), color.ToVector3()) / SpeedChangeColor);
        }

        private void SetColor(Color color, float duration)
        {
            if (c_changeColor != null) StopCoroutine(c_changeColor);
            c_changeColor = StartCoroutine(_ChangeColor(color, duration));
        }

        private IEnumerator _ChangeColor(Color targetColor, float duration)
        {
            var startColor = Color;

            var timer = 0f;
            while (timer < duration)
            {
                var t = timer / duration;

                Color = Color.Lerp(startColor, targetColor, t);
                
                yield return null;
                timer += Time.deltaTime;
            }

            Color = targetColor;
        }

        private void Awake()
        {
            InteractEvents.OnIdleStart.AddListener(() => SetColor(ColorIdle));
            InteractEvents.OnHoverStart.AddListener(ctx => SetColor(ColorHover));
            InteractEvents.OnPressStart.AddListener(ctx => SetColor(ColorPress));

            SetColor(ColorIdle, 0f);
        }
    }
}