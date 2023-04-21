using System;
using System.Collections.Generic;
using CucuTools.PlayerSystem.Visions.Hovering;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class HoverableColor : HoverableBehaviour
    {
        [Space]
        public HoverColorSettings settings = new HoverColorSettings();
        
        [Space]
        public Transform target = null;
        
        private readonly Dictionary<Material, Color> _colorCache = new Dictionary<Material, Color>();

        public override void Hover(bool value)
        {
            isHovering = value;

            foreach (var color in _colorCache)
            {
                var colorTarget = color.Value;
                
                if (isHovering)
                {
                    switch (settings.hoverType)
                    {
                        case HoverType.Color:
                            colorTarget = GetColor();
                            break;
                        case HoverType.OffsetHSV:
                            colorTarget = GetColorOffsetHSV(colorTarget);
                            break;
                    }
                }
                
                color.Key.color = Color.Lerp(color.Value, colorTarget, settings.powerColorHover);
            }
            
            onHoverChange.Invoke(isHovering);
        }

        public Color GetColor()
        {
            return settings.colorHovering;
        }
        
        public Color GetColorOffsetHSV(Color color)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
                
            h = Mathf.Repeat(h + settings.colorShift * 1f, 1f);

            if (s < 0.1f) s = 0.5f;
            if (v < 0.1f) v = 0.5f;

            return Color.HSVToRGB(h, s, v);
        }
        
        private void Awake()
        {
            if (target == null) target = transform;
            
            var renders = target.GetComponentsInChildren<Renderer>();
            foreach (var render in renders)
            {
                var material = render.material;
                _colorCache.TryAdd(material, material.color);
            }
        }
    }
    
    public enum HoverType
    {
        Color,
        OffsetHSV,
    }
    
    [Serializable]
    public class HoverColorSettings
    {
        public HoverType hoverType = HoverType.Color;
        [Range(0f, 1f)]
        public float powerColorHover = 0.618f;
        
        [Space]
        public Color colorHovering = Color.yellow;
        
        [Space]
        [Range(0f, 1f)]
        public float colorShift = 0.618f;
    }
}