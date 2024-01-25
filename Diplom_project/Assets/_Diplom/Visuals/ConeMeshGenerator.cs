using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeMeshGenerator : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] public float length;
    [SerializeField] public float angle;
    [SerializeField] public int segments;


    [SerializeField] private bool drawImpactGizmos;
    private List<Vector3> raycastHits = new();

    void Start()
    {
        Mesh mesh = new();
        meshFilter.mesh = mesh;
    }

    private void Update()
    {
        RebuildMesh();
    }

    public void RebuildMesh()
    {
        if (meshFilter == null)
            return;

        var mesh = meshFilter.mesh;
        mesh.Clear();
        float radius = Mathf.Tan(angle * 0.5f * Mathf.Deg2Rad) * length;

        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        var triangles = new List<int>();

        var origin = new Vector3(0, 0, 0);
        var circleCenter = origin + new Vector3(0, 0, length);

        vertices.Add(origin);
        uv.Add(new Vector2(0.5f, 1.0f));
        int originIndex = 0;

        vertices.Add(circleCenter);
        uv.Add(new Vector2(0.5f, 0.0f));
        int circleIndex = 1;

        void addTriangle(int p1Index, Vector3 p2, Vector3 p3)
        {
            triangles.Add(p1Index);

            vertices.Add(p2);
            triangles.Add(vertices.Count - 1);
            uv.Add(new Vector2(1.0f, 0.5f));

            vertices.Add(p3);
            triangles.Add(vertices.Count - 1);
            uv.Add(new Vector2(0.0f, 0.5f));
        }

        if (drawImpactGizmos)
            raycastHits.Clear();


        for (int i = 0; i < segments; i++)
        {
            var p1 = GetCircleVertexXY(circleCenter, radius, i + 1, segments);
            {
                var p1Direction = transform.TransformDirection(p1.normalized);
                if (Physics.Raycast(transform.position, p1Direction, out var hit, length))
                {
                    if (hit.distance < length)
                    {
                        p1 = transform.InverseTransformPoint(hit.point);
                        if (drawImpactGizmos)
                            raycastHits.Add(hit.point);
                    }
                }
            }
            var p2 = GetCircleVertexXY(circleCenter, radius, i, segments);
            {
                var p2Direction = transform.TransformDirection(p2.normalized);
                if (Physics.Raycast(transform.position, p2Direction, out var hit, length))
                {
                    if (hit.distance < length)
                    {
                        p2 = transform.InverseTransformPoint(hit.point);
                        if (drawImpactGizmos)
                            raycastHits.Add(hit.point);
                    }
                }
            }
            addTriangle(originIndex, p1, p2);
            //addTriangle(circleIndex, p2, p1);
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    private static Vector3 GetCircleVertexXY(Vector3 center, float radius, int index, int sides)
    {
        return new Vector3(center.x + radius * MathF.Cos(2 * MathF.PI * index / sides),
                            center.y + radius * MathF.Sin(2 * MathF.PI * index / sides),
                            center.z);
    }

    private void OnDrawGizmos()
    {
        if (drawImpactGizmos && raycastHits != null)
        {
            Gizmos.color = Color.green;
            foreach (var point in raycastHits)
            {
                Gizmos.DrawSphere(point, 0.03f);
            }
        }
    }

}
