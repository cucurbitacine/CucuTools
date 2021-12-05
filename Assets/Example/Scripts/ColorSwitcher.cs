using CucuTools;
using CucuTools.Interactables;
using UnityEngine;

namespace Example.Scripts
{
    public class ColorSwitcher : CucuBehaviour
    {
        public Color IdleColor = Color.white;
        public Color HoverColor = Color.red;
        public Color PressColor = Color.green;

        public Renderer[] Renderers => GetComponentsInChildren<Renderer>();
        public InteractableBehaviour Interactable => GetComponentInChildren<InteractableBehaviour>();

        private void Awake()
        {
            Interactable.InteractEvents.OnIdleStart.AddListener(Idle);
            Interactable.InteractEvents.OnHoverStart.AddListener(Hover);
            Interactable.InteractEvents.OnPressStart.AddListener(Press);
        }

        public void SetColor(Color color)
        {
            foreach (var renderer in Renderers)
            {
                renderer.material.color = color;
            }
        }

        public void Idle()
        {
            SetColor(IdleColor);
        }

        public void Hover(ICucuContext context)
        {
            SetColor(HoverColor);
        }

        public void Press(ICucuContext context)
        {
            SetColor(PressColor);
        }
    }
}