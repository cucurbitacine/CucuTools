using CucuTools.PlayerSystem;
using CucuTools.PlayerSystem.Visions;
using CucuTools.PlayerSystem.Visions.Dragging;
using CucuTools.PlayerSystem.Visions.Hovering;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerManager : MonoBehaviour
    {
        public PlayerController person = null;
        
        [Space]
        public VisionController vision = null;
        public TouchController touch = null;
        public HoverController hover = null;
        public DragController drag = null;
        

        private void Awake()
        {
            person = GetComponent<PlayerController>();

            vision = person.GetComponentInChildren<VisionController>();
            touch = person.GetComponentInChildren<TouchController>();
            hover = person.GetComponentInChildren<HoverController>();
            drag = person.GetComponentInChildren<DragController>();
        }
    }
}