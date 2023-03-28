using System;
using CucuTools;
using CucuTools.Attributes;
using CucuTools.PlayerSystem;
using UnityEngine;

namespace Examples.Playground
{
    public class CameraFollow : CucuBehaviour
    {
        public Vector3 localOffset = Vector3.zero;
        
        [Space]
        public Camera mainCamera;
        public PersonInput input;

        private Camera GetCamera()
        {
            if (mainCamera != null) return mainCamera;
            if (mainCamera == null) mainCamera = GetComponent<Camera>();
            if (mainCamera == null) mainCamera = Camera.main;
            return mainCamera;
        }

        private void Follow()
        {
            if (input == null) return;

            mainCamera.transform.rotation = input.Person.eyes.rotation;
            mainCamera.transform.position = input.Person.eyes.position + localOffset.ToWorldDirection(input.Person.eyes);
        }

        [Button("Follow")]
        private void Validate()
        {
            mainCamera = GetCamera();
            Follow();
        }

        private void Awake()
        {
            Validate();
        }

        private void Update()
        {
            Follow();
        }

        private void OnValidate()
        {
            Validate();
        }
    }
}
