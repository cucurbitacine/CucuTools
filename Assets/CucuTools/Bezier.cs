using System;
using UnityEngine;

namespace CucuTools
{
    [Serializable]
    public class Bezier
    {
        #region SerializeField

        [SerializeField] private float _weightIn = 0.5f;
        public bool autoWeight = true;
        public bool autoTangent = true;

        [Space]
        [SerializeField] private Vector3 _origin = Vector3.zero;
        [SerializeField] private Vector3 _direction = Vector3.up;

        [Space]
        public bool use2D = false;
        public Transform transform = null;

        #endregion

        public const int DefaultResolution = 32;
        
        public Vector3 origin
        {
            get => transform ? (_origin = transform.position) : _origin;
            set
            {
                _origin = value;

                if (transform)
                {
                    transform.position = _origin;
                }
            }
        }

        public Vector3 direction
        {
            get
            {
                if (autoTangent)
                {
                    if (previousCurve != null)
                    {
                        return _direction = (pointOut - previousCurve.pointIn).normalized;
                    }

                    if (nextCurve != null)
                    {
                        return _direction = (nextCurve.pointIn - pointIn).normalized;
                    }
                }

                return transform ? (_direction = (use2D ? transform.up : transform.forward)) : _direction.normalized;
            }
            set
            {
                if (autoTangent) return;

                _direction = value.normalized;

                if (transform)
                {
                    if (use2D)
                    {
                        transform.up = _direction;
                    }
                    else
                    {
                        transform.forward = _direction;
                    }
                }
            }
        }

        public float weightIn
        {
            get
            {
                if (autoWeight)
                {
                    var distance = Vector3.Distance(pointIn, pointOut);
                    
                    if (previousCurve != null)
                    {
                        distance = Mathf.Min(Vector3.Distance(previousCurve.pointIn, previousCurve.pointOut), distance);
                    }

                    return _weightIn = distance * 0.5f;
                }
                
                return _weightIn;
            }
            set => _weightIn = value;
        }

        public Bezier previousCurve { get; private set; }
        public Bezier nextCurve { get; private set; }

        public Vector3 pointIn
        {
            get => origin;
            set => origin = value;
        }

        public Vector3 tangentIn
        {
            get => direction;
            set => direction = value;
        }

        public Vector3 pointTangentIn
        {
            get => pointIn + tangentIn * weightIn;
            set
            {
                var vector = value - pointIn;
                tangentIn = vector.normalized;
                weightIn = vector.magnitude;
            }
        }

        public float weightOut
        {
            get => nextCurve?.weightIn ?? weightIn;
            set
            {
                if (nextCurve != null)
                {
                    nextCurve.weightIn = value;
                }
            }
        }

        public Vector3 pointOut
        {
            get => nextCurve?.pointIn ?? pointIn;
            set
            {
                if (nextCurve != null)
                {
                    nextCurve.pointIn = value;
                }
            }
        }

        public Vector3 tangentOut
        {
            get => -nextCurve?.tangentIn ?? Vector3.zero;
            set
            {
                if (nextCurve != null)
                {
                    nextCurve.tangentIn = -value;
                }
            }
        }

        public Vector3 pointTangentOut
        {
            get => pointOut + tangentOut * weightOut;
            set
            {
                var vector = value - pointOut;
                tangentOut = vector.normalized;
                weightOut = vector.magnitude;
            }
        }

        public bool isBegin => previousCurve == null;
        public bool isEnd => nextCurve == null;

        #region Public API

        public float GetLength(int resolution = DefaultResolution)
        {
            if (isEnd) return 0f;

            return Length(pointIn, pointTangentIn, pointTangentOut, pointOut, resolution);
        }

        public Vector3 LocalProgress(float progress)
        {
            return Position(progress, pointIn, pointTangentIn, pointTangentOut, pointOut);
        }

        public Vector3 LocalEvaluate(float distance, int resolution = DefaultResolution)
        {
            var length = GetLength(resolution);

            return length > 0 ? LocalProgress(distance / length) : pointIn;
        }

        public Vector3 Evaluate(float distance, int resolution = DefaultResolution)
        {
            if (distance < 0)
            {
                if (previousCurve != null)
                {
                    var prevLength = previousCurve.GetLength(resolution);
                    return previousCurve.Evaluate(prevLength - Mathf.Abs(distance), resolution);
                }

                return pointIn;
            }

            var length = GetLength(resolution);

            if (length > 0)
            {
                if (length < distance)
                {
                    if (nextCurve != null)
                    {
                        return nextCurve.Evaluate(distance - length, resolution);
                    }
                }

                var progress = distance / length;

                return LocalProgress(progress);
            }

            return pointIn;
        }

        public Vector3 Progress(float progress, int resolution = DefaultResolution)
        {
            var totalLength = GetLengthToEnd(resolution);

            var distance = progress * totalLength;

            return Evaluate(distance, resolution);
        }

        public bool TryGetBegin(out Bezier begin)
        {
            if (previousCurve != null)
            {
                begin = previousCurve;

                while (begin != this && begin.previousCurve != null)
                {
                    begin = begin.previousCurve;
                }

                return begin != this;
            }

            begin = this;
            return true;
        }

        public bool TryGetEnd(out Bezier end)
        {
            if (nextCurve != null)
            {
                end = nextCurve;

                while (end != this && end.nextCurve != null)
                {
                    end = end.nextCurve;
                }

                return end != this;
            }

            end = this;
            return true;
        }

        public float GetLengthToEnd(int resolution = DefaultResolution)
        {
            var length = GetLength(resolution);

            if (nextCurve != null)
            {
                var curve = nextCurve;

                while (curve != null && curve != this)
                {
                    length += curve.GetLength(resolution);

                    curve = curve.nextCurve;
                }
            }

            return length;
        }

        public float GetLengthFull(int resolution = DefaultResolution)
        {
            TryGetBegin(out var begin);

            return begin.GetLengthToEnd(resolution);
        }

        #endregion

        #region Link

        public void UnlinkNext()
        {
            if (nextCurve != null)
            {
                if (nextCurve.previousCurve == this)
                {
                    nextCurve.previousCurve = null;
                }

                nextCurve = null;
            }
        }

        public void UnlinkPrevious()
        {
            if (previousCurve != null)
            {
                if (previousCurve.nextCurve == this)
                {
                    previousCurve.UnlinkNext();
                }
            }
        }

        public bool LinkNext(Bezier curve)
        {
            UnlinkNext();

            if (curve == null) return false;
            if (curve == this) return false;
            if (curve.previousCurve != null) return false;

            nextCurve = curve;
            nextCurve.previousCurve = this;
            return true;
        }

        public bool LinkPrevious(Bezier curve)
        {
            UnlinkPrevious();

            if (curve == null) return false;
            if (curve == this) return false;
            if (curve.nextCurve != null) return false;

            return curve.LinkNext(this);
        }

        public bool Link(Bezier prev, Bezier next)
        {
            return LinkPrevious(prev) && LinkNext(next);
        }

        public void Unlink()
        {
            UnlinkPrevious();
            UnlinkNext();
        }

        #endregion

        #region Static

        public static Vector3 Position(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var t3 = t * t * t;
            var t2 = t * t;

            return p0 * (-t3 + 3 * t2 - 3 * t + 1) +
                   p1 * (3 * t3 - 6 * t2 + 3 * t) +
                   p2 * (-3 * t3 + 3 * t2) +
                   p3 * t3;
        }

        public static Vector3 Velocity(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var t2 = t * t;

            return p0 * (-3 * t2 + 6 * t - 3) +
                   p1 * (9 * t2 - 12 * t + 3) +
                   p2 * (-9 * t2 + 6 * t) +
                   p3 * 3 * t2;
        }

        public static Vector3 Acceleration(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return p0 * (-6 * t + 6) +
                   p1 * (18 * t - 12) +
                   p2 * (-18 * t + 6) +
                   p3 * 6 * t;
        }

        public static float Length(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int resolution = 32)
        {
            var length = 0f;
            Vector3 previous = default;
            for (var i = 0; i < resolution; i++)
            {
                var t = (float)i / (resolution - 1);

                var current = Position(t, p0, p1, p2, p3);
                if (i > 0) length += Vector3.Distance(previous, current);
                previous = current;
            }

            return length;
        }

        public static Vector3 Position(float t, Vector3[] p)
        {
            return Position(t, p[0], p[1], p[2], p[3]);
        }

        public static Vector3 Velocity(float t, Vector3[] p)
        {
            return Velocity(t, p[0], p[1], p[2], p[3]);
        }

        public static Vector3 Acceleration(float t, Vector3[] p)
        {
            return Acceleration(t, p[0], p[1], p[2], p[3]);
        }

        public static float Length(Vector3[] p, int resolution = 32)
        {
            return Length(p[0], p[1], p[2], p[3], resolution);
        }

        public static Vector2 Position(float t, Vector2[] p)
        {
            return Position(t, p[0], p[1], p[2], p[3]);
        }

        public static Vector2 Velocity(float t, Vector2[] p)
        {
            return Velocity(t, p[0], p[1], p[2], p[3]);
        }

        public static Vector2 Acceleration(float t, Vector2[] p)
        {
            return Acceleration(t, p[0], p[1], p[2], p[3]);
        }

        public static float Length(Vector2[] p, int resolution = 32)
        {
            return Length(p[0], p[1], p[2], p[3], resolution);
        }

        #endregion
    }
}