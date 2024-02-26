using UnityEngine;
using UnityEngine.Events;

namespace Samples.Demo.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class DraggableSprite : MonoBehaviour
    {
        public bool freezeX;
        public bool freezeY;

        [Space]
        public bool fixedArea;
        public Vector2 areaCenter = Vector2.zero;
        public Vector2 areaSize = Vector2.one;
        
        [Space]
        public Collider2D collider2d;
        
        private static Camera cameraMain => Camera.main;
        
        private Vector2 offset;

        public UnityEvent onPositionChanged = new UnityEvent();
        
        private void OnMouseDown()
        {
            if (collider2d & cameraMain)
            {
                var mousePosition = (Vector2)cameraMain.ScreenToWorldPoint(Input.mousePosition);
                offset = collider2d.transform.InverseTransformPoint(mousePosition);
            }
        }

        private void OnMouseDrag()
        {
            if (collider2d && cameraMain)
            {
                var mousePosition = (Vector2)cameraMain.ScreenToWorldPoint(Input.mousePosition);

                var position = mousePosition - offset;

                if (fixedArea)
                {
                    position.x = Mathf.Clamp(position.x, areaCenter.x - areaSize.x * 0.5f, areaCenter.x + areaSize.x * 0.5f);
                    position.y = Mathf.Clamp(position.y, areaCenter.y - areaSize.y * 0.5f, areaCenter.y + areaSize.y * 0.5f);
                }
                
                if (freezeX) position.x = collider2d.transform.position.x;
                if (freezeY) position.y = collider2d.transform.position.y;

                collider2d.transform.position = position;
                
                onPositionChanged.Invoke();
            }
        }

        private void Awake()
        {
            if (collider2d == null) collider2d = GetComponent<Collider2D>();
        }
        
        private void OnValidate()
        {
            if (collider2d == null) collider2d = GetComponent<Collider2D>();
        }

        private void OnDrawGizmos()
        {
            if (fixedArea && collider2d)
            {
                Gizmos.DrawWireCube(areaCenter, areaSize);
            }
        }
    }
}