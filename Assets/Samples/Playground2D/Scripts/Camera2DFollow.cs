using UnityEngine;

namespace Samples.Playground2D.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float depth = 10f;
        public Vector2 offset;

        [Space] public float positionChangeRate = 16;
        [Space] public float rotationChangeRate = 16;
        private Camera _cam;

        public Vector2 position
        {
            get => _cam.transform.position;
            set => _cam.transform.position = new Vector3(value.x, value.y, -depth);
        }

        public Quaternion rotation
        {
            get => _cam.transform.rotation;
            set => _cam.transform.rotation = value;
        }

        private void UpdateCamara(bool force = false)
        {
            if (target)
            {
                var realOffset = (Vector2)target.TransformVector(offset);
                
                var targetPosition = (Vector2)target.transform.position + realOffset;
                var targetRotation = Quaternion.LookRotation(Vector3.forward, target.up);
                
                if (force)
                {
                    rotation = targetRotation;
                    position = targetPosition;
                }
                else
                {
                    rotation = Quaternion.Lerp(rotation, targetRotation, rotationChangeRate * Time.deltaTime);
                    position = Vector2.Lerp(position, targetPosition, positionChangeRate * Time.deltaTime);
                }
            }
        }
        
        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void Update()
        {
            UpdateCamara(false);
        }

        private void OnValidate()
        {
            _cam = GetComponent<Camera>();

            UpdateCamara(true);
        }
    }
}
