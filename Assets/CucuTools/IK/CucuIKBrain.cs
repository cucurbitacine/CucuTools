using System;
using System.Drawing;
using Codice.Utils;
using CucuTools.Attributes;
using CucuTools.Colors;
using UnityEngine;
using Color = UnityEngine.Color;

namespace CucuTools.IK
{
    [DisallowMultipleComponent]
    public sealed class CucuIKBrain : CucuBehaviour
    {
        #region SerializeField

        [CucuReadOnly]
        [SerializeField] private bool solved = false;
        
        [Header("IK")]
        [SerializeField] private CucuIK cucuIK = default;
        [SerializeField] private bool useInitialPoints = true;
        
        [Header("Target")]
        [SerializeField] private Vector3 target = default;
        
        [Header("Points")]
        [SerializeField] private Vector3[] points = default;

        #endregion
        
        #region Properties & Fields
        
        private Vector3[] _initialPoints = default;
        private float[] _lengths = default;
        private Vector3[] _gizmosPoints = default;
        
        public bool Solved
        {
            get => solved;
            private set => solved = value;
        }

        public CucuIK CucuIK
        {
            get => cucuIK != null ? cucuIK : (cucuIK = new CucuIK());
            set => cucuIK = value;
        }

        public bool UseInitialPoints
        {
            get => useInitialPoints;
            set => useInitialPoints = value;
        }

        private Vector3 Target
        {
            get => target;
            set => target = value;
        }

        private Vector3[] Points
        {
            get => points != null ? points : (points = Array.Empty<Vector3>());
            set => points = value;
        }
        
        private Vector3[] InitialPoints
        {
            get => _initialPoints;
            set => _initialPoints = value;
        }

        private float[] Lengths
        {
            get => _lengths;
            set => _lengths = value;
        }
        
        private Vector3 TargetWorld => transform.TransformPoint(Target);
        
        public int PointCount => Points.Length;
        
        #endregion

        #region IK

        public void SetTarget(Vector3 target, bool useWorldSpace = true)
        {
            Target = useWorldSpace ? transform.InverseTransformPoint(target) : target;
        }
        
        public Vector3 GetTarget(bool useWorldSpace = true)
        {
            return useWorldSpace ? TargetWorld : Target;
        }
        
        public void SetPoints(Vector3[] points, bool useWorldSpace = true)
        {
            InitialPoints = new Vector3[points.Length];
            Points = new Vector3[points.Length];

            for (var i = 0; i < points.Length; i++)
            {
                InitialPoints[i] = useWorldSpace ? transform.InverseTransformPoint(points[i]) : points[i];
            }
            
            if (Application.isEditor && !Application.isPlaying)
            {
                Array.Copy(InitialPoints, Points, Points.Length);
            }
            
            Lengths = CucuIK.GetLengths(InitialPoints);
        }

        public Vector3 GetPoint(int index, bool useWorldSpace = true)
        {
            return useWorldSpace ? transform.TransformPoint(Points[index]) : Points[index];
        }
        
        public bool Solve()
        {
            if (PointCount < 2) return false;
            
            if (UseInitialPoints)
            {
                for (var i = 0; i < InitialPoints.Length; i++)
                {
                    Points[i] = InitialPoints[i];
                }
            }

            return CucuIK.Solve(Target, Points, Lengths);
        }

        #endregion

        #region MonoBehaviour

        public bool Pause { get; set; }

        private void Awake()
        {
            SetPoints(Points, true);
        }

        private void Update()
        {
            if (Pause) return;

            Solved = Solve();
        }

        private void OnDrawGizmos()
        {
            if (Points.Length < 1) return;
            
            if (_gizmosPoints == null) _gizmosPoints = new Vector3[Points.Length];
            if (_gizmosPoints.Length != Points.Length) Array.Resize(ref _gizmosPoints, Points.Length);
            Array.Copy(Points, _gizmosPoints, Points.Length);
            
            for (var i = 0; i < _gizmosPoints.Length; i++)
            {
                _gizmosPoints[i] = transform.TransformPoint(_gizmosPoints[i]);
            }
            
            Gizmos.color = Color.green;
            CucuGizmos.DrawLines(_gizmosPoints);
            CucuGizmos.DrawWireSpheres(_gizmosPoints, 0.1f);

            if (!Application.isPlaying)
            {
                CucuIK.Solve(TargetWorld, _gizmosPoints, CucuIK.GetLengths(_gizmosPoints));

                Gizmos.color = Color.yellow;
                CucuGizmos.DrawLines(_gizmosPoints);
                CucuGizmos.DrawWireSpheres(_gizmosPoints, 0.1f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(TargetWorld, 0.1f);
        }
        
        #endregion
    }
}
