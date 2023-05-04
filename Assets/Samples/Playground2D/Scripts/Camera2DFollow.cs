using System;
using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Samples.Playground2D.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class Camera2DFollow : MonoBehaviour
    {
        public PlayerPlatformController player;
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
            if (player)
            {
                if (force)
                {
                    position = player.playerPoint + offset;
                }
                else
                {
                    position = Vector2.Lerp(position, player.playerPoint + offset, positionChangeRate * Time.deltaTime);
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
