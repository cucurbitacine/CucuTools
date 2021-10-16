using System.Linq;
using CucuTools;
using CucuTools.Interactables;
using UnityEngine;

namespace Example.Interactables
{
    public class ExampleInteractables : ExampleBlock, ICucuContext
    {
        public float SpeedFollow = 1000f;

        public CucuInteractable GrabInteractable;
        public CucuInteractable CurrentInteractable;

        private RaycastHit[] RaycastHits;
        private int CountHits;
        private bool wasPressing;
        
        public bool IsEnabled { get; set; }
        
        public InteractHandler InteractHandler { get; set; }
        
        public override void ShowGUI()
        {
            
        }

        public override void Show()
        {
            base.Show();
            
            if (InteractHandler == null) InteractHandler = new InteractHandler();
            if (RaycastHits == null) RaycastHits = new RaycastHit[32];
            
            IsEnabled = true;
        }

        public override void Hide()
        {
            base.Hide();
            
            IsEnabled = false;
        }

        private void FixedUpdate()
        {
            if (IsEnabled)
            {
                var inputMousePosition = Input.mousePosition;
                var cameraMain = Camera.main; 
                
                var ray = cameraMain.ScreenPointToRay(inputMousePosition);
                CountHits = Physics.RaycastNonAlloc(ray, RaycastHits);

                var firstHit = RaycastHits.Take(CountHits).OrderBy(rh=>rh.distance).FirstOrDefault();
                CurrentInteractable = firstHit.transform?.GetComponent<CucuInteractable>();

                var pressing = Input.GetKey(KeyCode.Mouse0);

                InteractHandler.Update(this, pressing, CurrentInteractable);
                
                if (wasPressing != pressing)
                {
                    if (pressing)
                    {
                        if (CurrentInteractable != null)
                        {
                            GrabInteractable = CurrentInteractable;
                            //GrabInteractable.Press(this);
                            GrabInteractable.IsEnabled = false;
                        }
                    }
                    else
                    {
                        if (GrabInteractable != null)
                        {
                            GrabInteractable.IsEnabled = true;
                            GrabInteractable.Idle();
                            GrabInteractable = null;
                        }
                    }
                }

                if (GrabInteractable != null)
                {
                    var angleX = Vector3.SignedAngle(Vector3.forward,
                        Vector3.ProjectOnPlane(ray.direction, Vector3.up), Vector3.up);
                    var angleY = Vector3.SignedAngle(Vector3.forward,
                        Vector3.ProjectOnPlane(ray.direction, Vector3.right), -Vector3.right);

                    var pos = cameraMain.transform.position;
                    var z = Mathf.Abs(0 - pos.z);

                    var x = z * Mathf.Tan(angleX * Mathf.Deg2Rad);
                    var y = z * Mathf.Tan(angleY * Mathf.Deg2Rad);

                    pos.x = Mathf.Clamp(cameraMain.transform.position.x + x, -5, 5);
                    pos.y = Mathf.Clamp(cameraMain.transform.position.y + y, 0, 10);
                    pos.z = 0;
                    
                    pos.z = 0f;
                    
                    if (pressing)
                    {
                        GrabInteractable.GetComponent<Rigidbody>().velocity =
                            (pos - GrabInteractable.transform.position) * SpeedFollow * Time.fixedDeltaTime;
                    }
                    
                    Debug.DrawLine(cameraMain.transform.position, pos);
                }

                wasPressing = pressing;
            }
        }
    }
}
