using UnityEngine;

namespace Samples.Demo.Scripts
{
    public class DraggableRigid : MonoBehaviour
    {
        [Min(0)] public float speedMax = 50;
        [Min(0)] public float dampPosition = 8; 
        
        [Space]
        public Rigidbody2D rigid;
        
        private static Camera cameraMain => Camera.main;
        
        private Vector2 localTouch;
        private Vector2 anchor;
        private Vector2 dragPoint;
        private Vector2 velocity;
        
        private void OnMouseDown()
        {
            var mousePosition = (Vector2)cameraMain.ScreenToWorldPoint(Input.mousePosition);
            localTouch = rigid.transform.InverseTransformPoint(mousePosition);
        }

        private void OnMouseDrag()
        {
            if (rigid && cameraMain)
            {
                var mousePosition = (Vector2)cameraMain.ScreenToWorldPoint(Input.mousePosition);
                
                anchor = rigid.transform.TransformPoint(localTouch);
                
                dragPoint = Vector2.Lerp(anchor, mousePosition, dampPosition * Time.deltaTime);
                
                velocity = Vector2.ClampMagnitude((dragPoint - anchor) / Time.deltaTime, speedMax);
                
                var force = rigid.mass * (velocity - rigid.GetPointVelocity(anchor));
                
                rigid.AddForceAtPosition(force, anchor, ForceMode2D.Force);
            }
        }

        private void Awake()
        {
            if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        }
        
        private void OnValidate()
        {
            if (rigid == null) rigid = GetComponent<Rigidbody2D>();
        }
    }
}