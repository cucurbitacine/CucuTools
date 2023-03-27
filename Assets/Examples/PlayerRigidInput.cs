using CucuTools.PlayerSystem;
using CucuTools.PlayerSystem.Visions.Dragging;
using UnityEngine;

namespace Examples
{
    public class PlayerRigidInput : PlayerInput
    {
        [Space]
        public bool isEnabled = true;
        [SerializeField] private PlayerController _player = null;
        
        [Space]
        public DragController drag = null;
        
        [Space]
        public Vector2 sensitivity = Vector2.one * 4;
        
        [Space]
        public Vector2 move = Vector2.zero;
        public Vector2 view = Vector2.zero;
        public Vector2 angles = Vector2.zero;

        public bool run = false;
        public bool sneak = false;
        public bool jump = false;
        
        public bool shoot = false;
        public bool dragging = false;
        
        public Vector2 mouseScrollDelta = Vector2.zero;

        public override PlayerController player => _player;
        
        private void UpdateInput()
        {
            move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            move = Vector3.ClampMagnitude(move, 1);

            view = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            mouseScrollDelta = Input.mouseScrollDelta;

            run = Input.GetKey(KeyCode.LeftShift);
            sneak = Input.GetKey(KeyCode.LeftControl);
            jump = Input.GetKeyDown(KeyCode.Space);

            shoot = Input.GetKeyDown(KeyCode.Mouse0);
            dragging = Input.GetKey(KeyCode.Mouse1);
        }

        private void UpdatePlayer()
        {
            player.Move(move);

            angles = Vector2.Scale(view, sensitivity);
            
            player.View(angles);
        
            if (jump) player.Jump();

            if (drag) drag.inputDragging = dragging;
        }

        private void Update()
        {
            if (isEnabled)
            {
                UpdateInput();

                if (player != null) UpdatePlayer();
            }

            if (Input.GetKeyDown(KeyCode.P)) isEnabled = !isEnabled;
        }
        
        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}