using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BezierPoint), true, isFallback = false)]
    public class BezierPointEditor : CucuBehaviourEditor
    {
        private Vector3 newPosition;
        private Quaternion newRotation;
        private float newWeight;

        private Vector3 Position(Transform transform)
        {
            var handlePosition = transform.position;
            var handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? transform.rotation
                : Quaternion.identity;

            return Handles.PositionHandle(handlePosition, handleRotation);
        }

        private Quaternion Rotation(Transform transform)
        {
            var handlePosition = transform.position;
            var handleRotation = newRotation;

            return Handles.RotationHandle(handleRotation, handlePosition);
        }

        private float Weight(BezierPoint point)
        {
            var handlePosition = point.bezier.pointIn;
            var handleRotation = Quaternion.identity;
            
            var size = HandleUtility.GetHandleSize(handlePosition) * 0.5f;
            var cross = Tools.pivotRotation == PivotRotation.Local ? -point.transform.up : Vector3.down;
            var offset = cross.normalized * size;
            var snap = 0.1f;

            Handles.DrawLine(handlePosition, handlePosition + offset, 2f);

            return Handles.ScaleValueHandle(
                point.bezier.weightIn,
                handlePosition + offset, handleRotation,
                size, Handles.DotHandleCap, snap);
        }

        private void Arrow(Vector3 origin, Vector3 point)
        {
            var radius = 0.05f;
            var angle = 30f;

            Handles.DrawLine(origin, point, 1f);
            Handles.DrawSolidArc(point, Vector3.forward, origin - point, angle * 0.5f, radius);
            Handles.DrawSolidArc(point, Vector3.forward, origin - point, -angle * 0.5f, radius);
        }
        
        private void Arrow(Bezier bezier)
        {
            Arrow(bezier.pointIn, bezier.pointTangentIn);
        }
        
        private void Arrow(BezierPoint point)
        {
            Arrow(point.bezier);
        }
        
        protected virtual void OnSceneGUI()
        {
            var point = (BezierPoint)target;

            if (!point.drawGizmos) return;
            
            Arrow(point);
            
            var transform = point.transform;

            newPosition = transform.position;
            newRotation = transform.rotation;
            newWeight = point.bezier.weightIn;

            EditorGUI.BeginChangeCheck();
            
            var tool = Tools.current;
            if (tool == Tool.Move)
            {
                newPosition = Position(transform);

                if (!point.bezier.autoWeight)
                {
                    newWeight = Weight(point);
                }
            }
            else if (tool == Tool.Rotate)
            {
                if (!point.bezier.autoTangent)
                {
                    Tools.pivotRotation = PivotRotation.Local;
                    
                    newRotation = Rotation(transform);
                }
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(transform, "Point Was Moved");
                transform.position = newPosition;

                Undo.RecordObject(transform, "Point Was Rotated");
                transform.rotation = newRotation;

                Undo.RecordObject(transform, "Weight Was Changed");
                point.bezier.weightIn = newWeight;
            }
        }
    }
}