using CucuTools.PlayerSystem;
using CucuTools.PlayerSystem.Visions.Dragging;
using UnityEngine;

namespace Examples.Scripts.Input
{
    public abstract class RigidPersonInput<T> : PersonInput<T> where T : RigidPersonController
    {
        public bool isEnabled = true;
        
        [Header("Camera Settings")]
        public Camera cam = null;
        public Vector2 viewSens = Vector2.one * 4;
        [Range(0, 180)]
        public float fovIdle = 60f;
        [Range(0, 90)]
        public float fovRunOffset = 20f;
        [Range(-90, 0)]
        public float fovAimOffset = -20f;
        
        [Header("Input")]
        public Vector3 move = Vector3.zero;
        public Vector2 view = Vector2.zero;
        [Space] 
        public bool run = false;
        public bool jump = false;
        public bool aim = false;
        public bool shoot = false;
        public bool dragging = false;
        
        [Header("Person Settings")]
        [Space] public float runScale = 2f;
        [Space] public DragController drag;
        
        protected virtual void UpdateInput()
        {
            move = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0f, UnityEngine.Input.GetAxisRaw("Vertical"));
            move = Vector3.ClampMagnitude(move, 1);

            view = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));

            run = UnityEngine.Input.GetKey(KeyCode.LeftShift);
            jump = UnityEngine.Input.GetKeyDown(KeyCode.Space);
            aim = UnityEngine.Input.GetKey(KeyCode.Mouse1);
            shoot = UnityEngine.Input.GetKeyDown(KeyCode.Mouse0);
            dragging = UnityEngine.Input.GetKey(KeyCode.Mouse0);
            
            if (drag) drag.inputDragging = dragging;
        }

        protected abstract void UpdatePerson(float deltaTime);
        protected abstract void UpdateCamera(float deltaTime);

        protected virtual void HandlePause()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.P))
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

            if (isEnabled && UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                Cursor.visible = false;
            }
        }
        
        protected virtual void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected virtual void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        protected virtual void Update()
        {
            HandlePause();
            
            if (isEnabled)
            {
                UpdateInput();
                
                UpdatePerson(Time.deltaTime);
            }
        }

        protected virtual void LateUpdate()
        {
            if (isEnabled) UpdateCamera(Time.deltaTime);
        }
    }
    
    public abstract class RigidPersonInput : RigidPersonInput<RigidPersonController>
    {
    }
}