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
        private Camera _cam;

        public Vector2 position
        {
            get
            {
                return _cam.transform.position;
            }
            set
            {
                _cam.transform.position = new Vector3(value.x, value.y, -depth);
            }
        }

        private void UpdateCamara(bool force = false)
        {
            if (target)
            {
                if (force)
                {
                    position = (Vector2)target.transform.position + offset;
                }
                else
                {
                    position = Vector2.Lerp(position, (Vector2)target.transform.position + offset, positionChangeRate * Time.deltaTime);
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
