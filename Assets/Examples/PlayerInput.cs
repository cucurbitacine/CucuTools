using CucuTools.PlayerSystem;
using CucuTools.PlayerSystem.Visions.Dragging;
using UnityEngine;

namespace Examples
{
    public class PlayerInput : PersonInput
    {
        [Space]
        public bool isEnabled = true;
        [SerializeField] private PersonController player = null;
        
        [Space]
        public DragController drag = null;
        
        [Space]
        public Vector2 sensitivity = Vector2.one * 4;

        [Space]
        public Vector3 move = Vector3.zero;
        public Vector2 view = Vector2.zero;
        public Vector2 angles = Vector2.zero;

        public bool run = false;
        public bool sneak = false;
        public bool jump = false;
        
        public bool shoot = false;
        public bool dragging = false;
        
        public Vector2 mouseScrollDelta = Vector2.zero;

        public override PersonController person => player;
        
        private void UpdateInput()
        {
            var yMove = Input.GetKey(KeyCode.R) ? 1f : (Input.GetKey(KeyCode.F) ? -1f : 0f);
            move = new Vector3(Input.GetAxisRaw("Horizontal"), yMove, Input.GetAxisRaw("Vertical"));
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
            angles = Vector2.Scale(view, sensitivity);
            
            person.Move(move);

            person.Rotate(angles);
        
            if (jump) person.Jump();

            if (drag) drag.inputDragging = dragging;
        }

        private void Update()
        {
            if (isEnabled)
            {
                UpdateInput();

                if (person != null) UpdatePlayer();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                isEnabled = !isEnabled;
                if (!isEnabled) person?.Stop();

                if (isEnabled)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }

            if (isEnabled && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Cursor.visible = false;
            }
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