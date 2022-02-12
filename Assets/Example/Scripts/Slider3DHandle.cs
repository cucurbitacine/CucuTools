using System;
using CucuTools;
using CucuTools.Colors;
using CucuTools.Interactables;
using UnityEngine;

namespace Example.Scripts
{
    public class Slider3DHandle : InteractableBehaviour
    {
        private float _distance;
        private Vector3 _offset;
        
        public bool Handled;

        public float Damp = 32f;
        public float MaxRadiusHandle = 0.5f;
        
        [Space] public CucuAvatarInteractHandler Context;

        [Space] public Transform HandleHead;
        public Transform HandleRoot;
        public CucuSlider Slider;

        public Transform Head => Context.CucuAvatar.Head;
        public Vector3 Target => _offset + Head.position + Head.forward * _distance;
        
        private void StartHandle(ICucuContext context)
        {
            if (Handled) return;

            if (context is CucuAvatarInteractHandler avatar)
            {
                Context = avatar;
                
                _distance = Vector3.Distance(HandleHead.position, Head.position);
                _offset = HandleHead.position - (Head.position + Head.forward * _distance);
                
                Handled = true;
                
                IsEnabled = false;
            }
        }

        private void UpdateHandle()
        {
            if (!Context.Pressed)
            {
                StopHandle();
                return;
            }

            if (Vector3.Distance(Target, HandleHead.position) > MaxRadiusHandle)
            {
                StopHandle();
                return;
            }
            
            Slider.Value =  Slider.GetValueByPoint(Target);
        }
        
        private void StopHandle()
        {
            if (!Handled) return;

            IsEnabled = true;
            
            Idle();
            
            Handled = false;
            Context = null;
            _offset = Vector3.zero;
        }
        
        private void OnValueSliderChanged(float value)
        {
            if (Damp <= 0)
            {
                HandleRoot.position = Slider.HandlePosition;
            }
            else
            {
                HandleRoot.position = Vector3.Lerp(HandleRoot.position, Slider.HandlePosition, Damp * Time.deltaTime);
            }
        }
        
        private void Awake()
        {
            InteractEvents.OnPressStart.AddListener(StartHandle);
            //InteractEvents.OnPressCancel.AddListener(StopHandle);

            HandleRoot.position = Slider.HandlePosition;
            
            Slider.OnValueChanged.AddListener(OnValueSliderChanged);
        }

        private void Update()
        {
            if (Handled) UpdateHandle();
        }

        private void OnDrawGizmos()
        {
            if (Context != null && Head != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(Target, Slider.HandlePosition);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(Target, 0.1f);

            }
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(Slider.HandlePosition, 0.1f);
            
            Gizmos.color = Color.white.AlphaTo(0.5f);
            Gizmos.DrawSphere(HandleHead.position, MaxRadiusHandle);
        }
    }
}