using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class PathTracker : MonoBehaviour
    {
        public bool trackOnStart = true;
        public float timeStep = 0.1f;
        public Vector3 offset = Vector3.zero;
        
        [Space] public Transform target = null;
        [Space] public LineRenderer line = null;

        private readonly List<Vector3> _points = new List<Vector3>();

        private Coroutine _tracking = null;
        private bool _trackPositions = true;

        public void StartTracking()
        {
            ResetTrack();
            _trackPositions = true;

            if (_tracking != null) StopCoroutine(_tracking);
            _tracking = StartCoroutine(_Tracking());
        }

        public void StopTracking()
        {
            _trackPositions = false;
            if (_tracking != null) StopCoroutine(_tracking);
        }

        public void DrawTrack()
        {
            line.positionCount = _points.Count;
            for (var i = 0; i < _points.Count; i++)
            {
                line.SetPosition(i, _points[i] + offset);
            }
        }

        public void ResetTrack()
        {
            _points.Clear();
        }
        
        private IEnumerator _Tracking()
        {
            while (_trackPositions)
            {
                _points.Add(target.position);
                yield return new WaitForSeconds(timeStep);
            }
        }

        private void Start()
        {
            if (trackOnStart) StartTracking();
        }
    }
}