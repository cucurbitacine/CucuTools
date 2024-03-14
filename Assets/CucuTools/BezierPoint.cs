using System.Collections.Generic;
using System.Linq;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools
{
    public class BezierPoint : CucuBehaviour
    {
        #region SerializeField

        public int index = 0;
        public bool drawGizmos = true;
        
        [Space] [SerializeField] private Bezier _bezier = new Bezier();

        [Space] public BezierPoint nextPoint;
        
        #endregion
        
        private const string GroupNavigation = "Navigation";
        private const string GroupEdit = "Edit";
        private const string GroupConnection = "Connection";
        private const string GroupOther = "Other";
        
        private const string DefaultName = "Point";

        public Transform origin => transform;

        public Bezier bezier
        {
            get
            {
                _bezier.transform = origin;
                return _bezier;
            }
        }

        #region Public API

        public void Link(BezierPoint point)
        {
            if (point)
            {
                point.bezier.UnlinkPrevious();

                if (bezier.LinkNext(point.bezier))
                {
                    nextPoint = point;
                }
                else
                {
                    nextPoint = null;

                    Debug.LogWarning($"{name} :: I did not was linked to {point.name}");
                }
            }
        }

        public void Unlink()
        {
            bezier.UnlinkNext();

            nextPoint = null;
        }

        public int GetPointsNonAlloc(List<BezierPoint> points)
        {
            points.Clear();

            bezier.TryGetBegin(out var begin);

            var first = begin.transform.GetComponent<BezierPoint>();

            if (first)
            {
                var current = first.nextPoint;

                points.Add(first);

                while (current != null && current != first)
                {
                    points.Add(current);
                    current = current.nextPoint;
                }
            }

            return points.Count;
        }

        public List<BezierPoint> GetPoints()
        {
            var points = new List<BezierPoint>();
            GetPointsNonAlloc(points);
            return points;
        }

        public void UpdateIndexes()
        {
            var points = GetPoints();
            for (var i = 0; i < points.Count; i++)
            {
                points[i].index = i;
            }
        }

        #endregion

        protected void Link()
        {
            if (nextPoint)
            {
                Link(nextPoint);
            }
        }

#if UNITY_EDITOR
        protected void Select(params GameObject[] objects)
        {
            UnityEditor.Selection.objects = objects.OfType<Object>().ToArray();
        }

        private void Select(params Component[] components)
        {
            Select(components.Select(c => c.gameObject).ToArray());
        }
        
        [DrawButton(group: GroupNavigation, order: 0)]
        protected void Next()
        {
            if (bezier.nextCurve != null && bezier.nextCurve.transform != null)
            {
                var point = bezier.nextCurve.transform.GetComponent<BezierPoint>();

                if (point)
                {
                    Select(point);
                }
            }
        }

        [DrawButton(group: GroupNavigation, order: 1)]
        protected void Previous()
        {
            if (bezier.previousCurve != null && bezier.previousCurve.transform != null)
            {
                var point = bezier.previousCurve.transform.GetComponent<BezierPoint>();

                if (point)
                {
                    Select(point);
                }
            }
        }

        [DrawButton(name: "Select All", group: GroupNavigation, order: 2)]
        protected void SelectAll()
        {
            var points = GetPoints();
            Select(points.OfType<Component>().ToArray());
        }
        
        [DrawButton(group: GroupEdit, name: "Add point", order: 3)]
        protected void AddPoint()
        {
            var pointPosition = bezier.tangentIn + bezier.pointIn;

            var lastPoint = nextPoint;

            if (lastPoint)
            {
                pointPosition = bezier.LocalProgress(0.5f);
            }

            Unlink();

            var siblingIndex = gameObject.transform.GetSiblingIndex();

            var newPoint = new GameObject($"{DefaultName} {index + 1}").AddComponent<BezierPoint>();
            if (transform.parent)
            {
                newPoint.transform.SetParent(transform.parent, true);
            }
            newPoint.transform.SetSiblingIndex(siblingIndex + 1);
            newPoint.transform.position = pointPosition;
            
            newPoint.bezier.use2D = bezier.use2D;
            newPoint.index = index + 1;

            Link(newPoint);

            if (lastPoint)
            {
                nextPoint.Link(lastPoint);
            }

            UpdateNames();

            Next();
        }

        [DrawButton(group: GroupEdit, name: "Remove point", order: 4)]
        protected void RemovePoint()
        {
            BezierPoint prevPoint = null;
            BezierPoint savedNextPoint = nextPoint;

            if (bezier.previousCurve != null && bezier.previousCurve.transform != null)
            {
                prevPoint = bezier.previousCurve.transform.GetComponent<BezierPoint>();
            }

            if (nextPoint)
            {
                Unlink();
            }

            if (prevPoint)
            {
                prevPoint.Unlink();

                if (savedNextPoint)
                {
                    prevPoint.Link(savedNextPoint);
                }

                Select(prevPoint);
            }
            else
            {
                if (savedNextPoint)
                {
                    Select(savedNextPoint);
                }
            }

            if (prevPoint) prevPoint.UpdateNames();
            if (savedNextPoint) savedNextPoint.UpdateNames();

            DestroyImmediate(gameObject);
        }

        [DrawButton(name: "Loop", group: GroupConnection, order: 5)]
        protected void Loop()
        {
            if (bezier.TryGetBegin(out var begin) && bezier.TryGetEnd(out var end))
            {
                if (begin.transform && end.transform)
                {
                    var beginPoint = begin.transform.GetComponent<BezierPoint>();
                    var endPoint = end.transform.GetComponent<BezierPoint>();

                    if (beginPoint && endPoint)
                    {
                        endPoint.Link(beginPoint);
                    }
                }
            }
        }

        [DrawButton(name: "Unlink", group: GroupConnection, order: 6)]
        protected void UnlinkButton()
        {
            var savedNextPoint = nextPoint;

            Unlink();

            if (savedNextPoint)
            {
                savedNextPoint.UpdateNames();
            }

            UpdateNames();
        }

        [DrawButton(name: "Update Indexes", group: GroupOther, order: 7)]
        protected void UpdateNames()
        {
            UpdateIndexes();
            var points = GetPoints();
            foreach (var point in points)
            {
                point.UpdateName();
            }
        }
        
        protected void UpdateName()
        {
            if (name.StartsWith($"{DefaultName}"))
            {
                name = $"{DefaultName} #{index}";
            }
        }
#endif

        #region MonoBehaviour

        private void Awake()
        {
            Link();
        }

        private void OnValidate()
        {
            Link();
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                if (nextPoint == null) return;

                CucuGizmos.DrawBezier(bezier);
            }
        }

        #endregion
    }
}