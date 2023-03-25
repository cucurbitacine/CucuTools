using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public class PlayerRigidInput : PlayerInput<PlayerRigidController>
    {
        [Space]
        public bool isEnabled = true;

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
        
        private void UpdateInput()
        {
            move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            move = Vector3.ClampMagnitude(move, 1);

            view = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            mouseScrollDelta = Input.mouseScrollDelta;

            run = Input.GetKey(KeyCode.LeftShift);
            sneak = Input.GetKey(KeyCode.LeftControl);
            jump = Input.GetKeyDown(KeyCode.Space);

            shoot = Input.GetKeyDown(KeyCode.Mouse0);
            dragging =  Input.GetAxisRaw("Fire2") > 0f;
        }

        private void UpdatePlayer()
        {
            player.MoveLocal(move);

            angles = Vector2.Scale(view, sensitivity);
            
            player.Rotate(angles);
        
            if (jump) player.Jump();
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