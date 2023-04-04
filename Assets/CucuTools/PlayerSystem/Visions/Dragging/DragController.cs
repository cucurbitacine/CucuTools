using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.PlayerSystem.Visions.Dragging
{
    public class DragController : MonoBehaviour
    {
        [Space]
        public bool isOn = true;
        public bool inputDragging = false;
        
        [Space]
        public bool isDragging = false;
        public DraggableBase current = null;

        [Space]
        public bool fastDrag = false;
        public bool isPhysGun = false;
        [Min(0f)]
        public float draggingSpeedMax = 16f;

        [Space]
        public bool useDropDistance = false;
        [Min(0f)]
        public float dropDistanceMax = 3f;
        
        [Space]
        public bool useDragSmooth = false;
        [Range(0f, 1f)]
        public float dragSmooth = 0.618f;

        [Space]
        public bool usePowerPerMass = true;
        [Min(0.001f)]
        public float maxMass = 50f;
        public AnimationCurve powerPerMass = AnimationCurve.Linear(0, 1, 1, 0);
        
        [Space]
        public Vector3 dragPositionOffset = Vector3.zero;
        public Quaternion dragRotationOffset = Quaternion.identity;
        
        [Space]
        public UnityEvent<DragInfo> onDragChanged = new UnityEvent<DragInfo>();

        [Space]
        public PhysicMaterial dragPhysicMaterial = null;
        
        [Header("References")] 
        public TouchController touch = null;

        private Coroutine _dragging = null;
        private DragInfo _dragInfo = default;
        
        private Vector3 _prevEyeDirection = Vector3.zero;
        private Vector3 _eyeDirection = Vector3.zero;
        private Quaternion _dragStartLocalRotation = Quaternion.identity;
        
        private bool _nowDragging = false;
        private bool _wasDragging = false;

        private readonly Dictionary<Rigidbody, DraggableBase> _draggableCache = new Dictionary<Rigidbody, DraggableBase>();
        private readonly Dictionary<DraggableBase, Collider[]> _colliderCache = new Dictionary<DraggableBase, Collider[]>();
        private readonly Dictionary<Collider, PhysicMaterial> _physicMaterialCache = new Dictionary<Collider, PhysicMaterial>();
        
        public Transform eyes => touch != null ? touch.vision.eyes : transform;
        
        public Vector3 dragLocalPosition { get; set; }
        
        public Vector3 dragPosition
        {
            get => eyes.TransformPoint(dragLocalPosition);
            set => dragLocalPosition = eyes.InverseTransformPoint(value);
        }

        public Quaternion dragRotation { get; set; }

        public void Pick(DraggableBase draggable)
        {
            isDragging = true;
            current = draggable;

            dragPosition = current.rigid.position;
            dragRotation = current.rigid.rotation;
            _dragStartLocalRotation = Quaternion.Inverse(eyes.rotation) * dragRotation;
            _eyeDirection = Vector3.ProjectOnPlane(eyes.forward, Vector3.up);
                
            ApplyDragPhysics(current);
            
            if (_dragging != null) StopCoroutine(_dragging);
            _dragging = StartCoroutine(Dragging());

            _dragInfo.dragging = true;
            _dragInfo.draggable = current;
            
            current.Pick(this);
            
            dragPositionOffset = Vector3.zero;
            dragRotationOffset = Quaternion.identity;
            
            onDragChanged.Invoke(_dragInfo);
        }

        public void Drop()
        {
            if (_dragging != null) StopCoroutine(_dragging);

            ResetDragPhysics(current);

            _dragInfo.dragging = false;
            _dragInfo.draggable = current;
            
            current.Drop();
            
            current = null;
            isDragging = false;

            dragPositionOffset = Vector3.zero;
            dragRotationOffset = Quaternion.identity;
            
            onDragChanged.Invoke(_dragInfo);
        }

        public bool TryGetDraggable(Rigidbody rgb, out DraggableBase draggable)
        {
            if (rgb == null)
            {
                draggable = null;
                return false;
            }
            
            if (!_draggableCache.TryGetValue(rgb, out draggable))
            {
                draggable = rgb.GetComponent<DraggableBase>();
                _draggableCache.TryAdd(rgb, draggable);
            }
            
            return draggable != null;
        }
        
        private bool TryGetColliders(DraggableBase draggable, out Collider[] cld)
        {
            if (!_colliderCache.TryGetValue(draggable, out cld))
            {
                cld = draggable.rigid.GetComponentsInChildren<Collider>();
                _colliderCache.TryAdd(draggable, cld);
            }
            
            return cld.Length > 0;
        }

        private PhysicMaterial GetOrAddPhysicMaterial(Collider cld)
        {
            if (!_physicMaterialCache.TryGetValue(cld, out var pm))
            {
                pm = cld.sharedMaterial;
                _physicMaterialCache.TryAdd(cld, pm);
            }

            return pm;
        }
        
        private void ChangeDragMaterial(DraggableBase draggable, bool useDragMaterial)
        {
            if (dragPhysicMaterial == null) return;
            
            if (TryGetColliders(draggable, out var cld))
            {
                for (var i = 0; i < cld.Length; i++)
                {
                    var mat = GetOrAddPhysicMaterial(cld[i]);
                    cld[i].sharedMaterial = useDragMaterial ? dragPhysicMaterial : mat;
                }
            }
        }

        private void ApplyDragPhysics(DraggableBase draggable)
        {
            ChangeDragMaterial(draggable, true);
        }
        
        private void ResetDragPhysics(DraggableBase draggable)
        {
            ChangeDragMaterial(draggable, false);

            var velocity = Vector3.ClampMagnitude(current.rigid.velocity, draggingSpeedMax);
            var angularVelocity = Vector3.ClampMagnitude(current.rigid.angularVelocity, draggingSpeedMax);

            current.rigid.velocity = velocity;
            current.rigid.angularVelocity = angularVelocity;
        }

        private float GetPowerByMass(float mass)
        {
            if (!usePowerPerMass) return 1f;

            var t = Mathf.Max(0, mass) / maxMass;
            return powerPerMass.Evaluate(t);
        }
        
        private void UpdateInput(out bool startDragging, out bool stopDragging)
        {
            _wasDragging = _nowDragging;
            _nowDragging = inputDragging;

            startDragging = !_wasDragging && _nowDragging;
            stopDragging = _wasDragging && !_nowDragging;
        }
        
        private void UpdateHandRotation()
        {
            if (isPhysGun)
            {
                dragRotation = eyes.rotation * _dragStartLocalRotation;
            }
            else
            {
                _prevEyeDirection = _eyeDirection;
                _eyeDirection = Vector3.ProjectOnPlane(eyes.forward, Vector3.up);

                var angle = Vector3.SignedAngle(_prevEyeDirection, _eyeDirection, Vector3.up);
                dragRotation = Quaternion.Euler(0, angle, 0) * dragRotation;
            }
        }

        private IEnumerator Dragging()
        {
            while (true)
            {
                var power = GetPowerByMass(current.rigid.mass);
                var checkDistance = power * dropDistanceMax;
                
                if (useDropDistance && checkDistance < Vector3.Distance(current.position, dragPosition + dragPositionOffset))
                {
                    Drop();
                }
                else
                {
                    var speed = power * draggingSpeedMax;

                    var position = Vector3.Lerp(current.position, dragPositionOffset + dragPosition, power);
                    var rotation = Quaternion.Lerp(current.rotation, dragRotationOffset * dragRotation, power);

                    if (useDragSmooth)
                    {
                        position = Vector3.Lerp(position, current.position, dragSmooth);
                        rotation = Quaternion.Lerp(rotation, current.rotation, dragSmooth);
                    }

                    Cucu.SyncPosition(current.rigid, position, speed);
                    Cucu.SyncRotation(current.rigid, rotation, speed);
                    
                    if (fastDrag)
                    {
                        current.rigid.MovePosition(position);
                        current.rigid.MoveRotation(rotation);
                    }
                }
                
                yield return new WaitForFixedUpdate();
            }
        }

        private void Update()
        {
            UpdateInput(out var startDragging, out var stopDragging);

            if (isOn)
            {
                if (isDragging) UpdateHandRotation();

                if (startDragging && !isDragging && touch.touchSomething)
                {
                    if (TryGetDraggable(touch.current.hit.rigidbody, out var drag))
                    {
                        if (drag.isOn && !drag.isDragging)
                        {
                            if (0 < GetPowerByMass(drag.rigid.mass))
                            {
                                Pick(drag);
                            }
                        }
                    }
                }
                
                if (stopDragging && isDragging)
                {
                    Drop();
                }
            }
            else
            {
                if (isDragging)
                {
                    Drop();
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (isDragging)
            {
                var bounds = Cucu.GetBounds(current.rigid.gameObject);
                Gizmos.color = Color.green;
                CucuGizmos.DrawWireCube(bounds.center, bounds.size);
                
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(dragPosition + dragPositionOffset, 0.1f);
                
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(current.position, GetPowerByMass(current.rigid.mass) * dropDistanceMax);
                
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(current.position, Vector3.Distance(dragPosition, current.position));
                Gizmos.DrawLine(dragPosition, current.position);
            }
        }
    }
    
    public struct DragInfo
    {
        public bool dragging;
        public DraggableBase draggable;
    }
}