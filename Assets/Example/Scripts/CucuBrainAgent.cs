using System;
using System.Linq;
using CucuTools;
using CucuTools.Avatar;
using UnityEngine;
using UnityEngine.AI;

namespace Example.Scripts
{
    public class CucuBrainAgent : CucuBrain
    {
        private CucuAvatar _self = default;
        private NavMeshPath _path;
        
        [SerializeField] private bool following;
        [SerializeField] private Vector2 view;
        [Header("Settings")]
        [SerializeField] private CucuAvatar target = default;
        [SerializeField] private float minDistance = 0.5f;
        [SerializeField] private float maxDistance = 25f;
        [SerializeField] private Color colorBody = Color.red;
        [SerializeField] private float viewDamp = 4f;
        
        public CucuAvatar Self => _self != null ? _self : (_self = GetComponentInParent<CucuAvatar>());
        public Transform Body => Self.Body;
        public Transform Head => Self.Head;

        public override InputInfo GetInput()
        {
            if (target == null) return default;
            
            var input = new InputInfo();

            var direction = target.Head.position - Head.position;
            
            var ditance = direction.magnitude;

            if (ditance > maxDistance) input.sprint = true;
            
            if (following)
            {
                following = ditance > minDistance * 0.9f;
            }
            else
            {
                following = ditance > minDistance * 1.1f;
            }

            var success = following && NavMesh.CalculatePath(Head.position, target.position, 1, _path);
            
            if (following && success &&  _path.status == NavMeshPathStatus.PathComplete)
            {
                var from = _path.corners[0];
                var to = _path.corners[1];
                var dir = (to - from).normalized;
                        
                var bodyLook = Body.InverseTransformDirection(dir).normalized;
                bodyLook = Vector3.ProjectOnPlane(bodyLook, Vector3.up).normalized;
                view.x = Vector3.SignedAngle(Vector3.forward, bodyLook, Vector3.up);

                var headLook = Head.InverseTransformDirection(dir).normalized;
                headLook = Vector3.ProjectOnPlane(headLook, Vector3.right).normalized;
                view.y = Vector3.SignedAngle(Vector3.forward, headLook, Vector3.right);
                        
                var move = Vector3.ProjectOnPlane(Body.InverseTransformDirection(dir), Vector3.up).normalized;
                input.move = move;
            }
            else
            {
                var bodyLook = Body.InverseTransformDirection(direction).normalized;
                bodyLook = Vector3.ProjectOnPlane(bodyLook, Vector3.up).normalized;
                view.x = Vector3.SignedAngle(Vector3.forward, bodyLook, Vector3.up);

                var headLook = Head.InverseTransformDirection(direction).normalized;
                headLook = Vector3.ProjectOnPlane(headLook, Vector3.right).normalized;
                view.y = Vector3.SignedAngle(Vector3.forward, headLook, Vector3.right);
            }

            input.view = Vector2.Lerp(Vector2.zero, view, viewDamp * Time.deltaTime);
            
            input.jump = target.InputInfo.jump;
            input.crouch = target.InputInfo.crouch;
            
            return input;
        }
        
        private void Awake()
        {
            _path = new NavMeshPath();

            transform.parent.GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.material.color = colorBody);
        }
        
        private void OnDrawGizmos()
        {
            if (_path != null && _path.corners.Length > 1)
            {
                CucuGizmos.DrawLines(_path.corners);
            }
        }
    }
}
