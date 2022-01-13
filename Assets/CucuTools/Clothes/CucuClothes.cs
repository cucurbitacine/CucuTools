using System;
using System.Collections.Generic;
using CucuTools.Surfaces;
using UnityEngine;

namespace CucuTools.Clothes
{
    public class CucuClothes : MonoBehaviour
    {
        [Header("Simulation")]
        public int depthSimulation = 8;
        
        [Space]
        [Range(0f, 1f)]
        public float fading = 0.01f;
        
        [Space]
        public bool useKinematic = true;
        [Range(0f, 1f)]
        public float weightKinematic = 0.05f; 
        
        [Space]
        public bool useGravity = true;
        [Range(0f, 1f)]
        public float weightGravity = 1f;
        
        [Space]
        public bool useWind = true;
        [Range(0f, 1f)]
        public float weightWind = 1f;
        public WindZone windZone;
        
        [Header("Surface")]
        public bool useCrossing = true;
        public SurfaceBehaviour surface;
        
        [Header("Gizmos")]
        public bool drawGizmos = true;
        
        private MeshFilter _meshFilter;
        private Mesh _mesh;
        private Vector3 _previous;
        private Point[] _points;
        private List<Connection> _connections;

        private int GetPointIndex(int i, int j)
        {
            return i * surface.gizmos.SizeV + j;
        }
        
        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
            
            if (_meshFilter != null)
            {
                if (_meshFilter.sharedMesh != null) _mesh = _meshFilter.sharedMesh;
                else
                {
                    _mesh = new Mesh();
                    _mesh.name = _meshFilter.gameObject.name;
                }
            }
            
            var quadCount = (surface.gizmos.SizeU - 1) * (surface.gizmos.SizeV - 1);
            vertices = new Vector3[surface.gizmos.SizeU * surface.gizmos.SizeV];
            triangles = new int[quadCount * 2 * 3];
            uv = new Vector2[vertices.Length];
            
            _points = new Point[surface.gizmos.SizeU * surface.gizmos.SizeV];
            
            for (var i = 0; i < surface.gizmos.SizeU; i++)
            {
                var u = surface.gizmos.GridU[i];

                for (var j = 0; j < surface.gizmos.SizeV; j++)
                {
                    var v = surface.gizmos.GridV[j];

                    var point = surface.GetLocalPoint(u, v);

                    var index = GetPointIndex(i, j);
                    _points[index] = new Point(point, j == surface.gizmos.SizeV - 1);
                }
            }

            _connections = new List<Connection>();
            
            for (var i = 0; i < surface.gizmos.SizeU; i++)
            {
                for (var j = 0; j < surface.gizmos.SizeV; j++)
                {
                    var index = GetPointIndex(i, j);

                    if (j + 1 < surface.gizmos.SizeV)
                    {
                        var up = GetPointIndex(i, j + 1);
                        var connection = new Connection(_points[index], _points[up]);
                        _connections.Add(connection);
                    }
                    
                    if (i + 1 < surface.gizmos.SizeU)
                    {
                        var right = GetPointIndex(i + 1, j);
                        var connection = new Connection(_points[index], _points[right]);
                        _connections.Add(connection);
                    }

                    if (useCrossing)
                    {
                        if (i + 1 < surface.gizmos.SizeU && j + 1 < surface.gizmos.SizeV)
                        {
                            var rightCorner = GetPointIndex(i + 1, j + 1);
                            var connection = new Connection(_points[index], _points[rightCorner]);
                            _connections.Add(connection);
                        }

                        if (0 <= i - 1 && j + 1 < surface.gizmos.SizeV)
                        {
                            var leftCorner = GetPointIndex(i - 1, j + 1);
                            var connection = new Connection(_points[index], _points[leftCorner]);
                            _connections.Add(connection);
                        }
                    }
                }
            }
            
            _previous = transform.position;
        }

        private void FixedUpdate()
        {
            if (depthSimulation > 0)
            {
                var position = transform.position;

                var kinematic = -(position - _previous);//* Time.fixedDeltaTime;
                kinematic = transform.InverseTransformDirection(kinematic.normalized) * kinematic.magnitude;
                
                var gravity = Physics.gravity * (Time.fixedDeltaTime * Time.fixedDeltaTime);
                gravity = transform.InverseTransformDirection(gravity.normalized) * gravity.magnitude;

                var wind = Vector3.zero;
                if (windZone != null)
                {
                    if (windZone.mode == WindZoneMode.Directional)
                    {
                        wind = windZone.transform.forward * (windZone.windMain * Time.fixedDeltaTime);
                    }
                    else
                    {
                        var dir = transform.position - windZone.transform.position;
                        var pow = Mathf.Max(windZone.radius - dir.magnitude, 0f) / windZone.radius;
                        if (pow > 0) wind = dir.normalized * (pow * windZone.windMain * Time.fixedDeltaTime);
                    }
                    
                    wind = transform.InverseTransformDirection(wind.normalized) * wind.magnitude;
                }
                
                foreach (var point in _points)
                {
                    if (point.locked) continue;

                    var step = (point.position - point.previousPosition) * (1f - fading);

                    if (useKinematic)
                    {
                        step += weightKinematic * kinematic;
                    }

                    if (useGravity)
                    {
                        step += weightGravity * gravity;
                    }
                    
                    if (useWind && windZone != null)
                    {
                        step += weightWind * wind;
                    }

                    point.previousPosition = point.position;
                    point.position += step;
                }

                _previous = position;
            }

            for (var i = 0; i < depthSimulation; i++)
            {
                foreach (var connection in _connections)
                {
                    connection.Simulate();
                }
            }
        }

        private Vector3[] vertices = default;
        private int[] triangles = default;
        private Vector2[] uv = default;
        
        private void Update()
        {
            if (_meshFilter == null) return;
            
            var tri = 0;
            
            for (var i = 0; i < surface.gizmos.SizeU; i++)
            {
                var u = surface.gizmos.GridU[i];
                
                for (var j = 0; j < surface.gizmos.SizeV; j++)
                {
                    var v = surface.gizmos.GridV[j];
                    
                    var index = GetPointIndex(i, j);

                    vertices[index] = _points[index].position;
                    uv[index] = new Vector2(u, v);

                    if (i < surface.gizmos.SizeU - 1 && j < surface.gizmos.SizeV - 1)
                    {
                        var up = GetPointIndex(i, j + 1);
                        var corner = GetPointIndex(i + 1, j + 1);
                        var right = GetPointIndex(i + 1, j);

                        triangles[tri++] = index;
                        triangles[tri++] = up;
                        triangles[tri++] = corner;

                        triangles[tri++] = index;
                        triangles[tri++] = corner;
                        triangles[tri++] = right;
                    }
                }
            }

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.uv = uv;
            _mesh.RecalculateNormals();
            _mesh.Optimize();

            _meshFilter.sharedMesh = _mesh;
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;
            
            Vector3 F(Vector3 pos)
            {
                return transform.TransformPoint(pos);
            }

            if (_connections != null)
            {
                foreach (var conn in _connections)
                {
                    if (conn == null) continue;

                    var a = conn.a;
                    var b = conn.b;

                    if (a == null || b == null) continue;

                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(F(a.position), F(b.position));
                }
            }

            if (_points != null)
            {
                foreach (var point in _points)
                {
                    Gizmos.color = point.locked ? Color.red : Color.white;
                    Gizmos.DrawWireSphere(F(point.position), 0.02f);
                }
            }
            
        }
    }

    [Serializable]
    public class Point
    {
        public bool locked;
        public Vector3 position;
        public Vector3 previousPosition;

        public Point(Vector3 position, bool locked = false)
        {
            this.locked = locked;
            this.position = position;
            this.previousPosition = position;
        }
    }

    [Serializable]
    public class Connection
    {
        public Point a;
        public Point b;
        public float length;

        public Connection(Point a, Point b)
        {
            this.a = a;
            this.b = b;

            length = Vector3.Distance(a.position, b.position);
        }

        public void Simulate()
        {
            var center = (a.position + b.position) * 0.5f;
            var dir = (a.position - b.position).normalized;

            if (!a.locked) a.position = center + dir * (length * 0.5f);
            if (!b.locked) b.position = center - dir * (length * 0.5f);
        }
    }
}