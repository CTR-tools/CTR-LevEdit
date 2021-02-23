using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadHitBox : MonoBehaviour
{
    [SerializeField] private Material material;
    private void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        triangles = mesh.triangles;
        //var neighbours = MeshQuadNeighbors.GetNeighbors(mesh).ToArray();

        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, transform.rotation, transform.lossyScale);
        for (int i = 0; i < (int) Mathf.Floor((triangles.Length / 6.0f)) * 6.0f; i += 6)
        {
            int i1 = triangles[i + 0];
            int i2 = triangles[i + 1];
            int i3 = triangles[i + 2];
            int i4 = -4;
            for (int j = 3; j < 6; j++)
            {
                if (triangles[i + j] != i1 && triangles[i + j] != i2 && triangles[i + j] != i3)
                {
                    i4 = triangles[i + j];
                }
            }

            if (i1 < 0) continue;
            if (i2 < 0) continue;
            if (i3 < 0) continue;
            if (i4 < 0) continue;
            Vector3 p1 = matrix.MultiplyPoint(vertices[i1]) + transform.position;
            Vector3 p2 = matrix.MultiplyPoint(vertices[i2]) + transform.position;
            Vector3 p3 = matrix.MultiplyPoint(vertices[i3]) + transform.position;
            Vector3 p4 = matrix.MultiplyPoint(vertices[i4]) + transform.position;
            Vector3 n1 = normals[i1];
            Vector3 n2 = normals[i2];
            Vector3 n3 = normals[i3];
            Vector3 n4 = normals[i4];

            Mesh quadMesh = new Mesh();

            Vector3[] meshVertices = new Vector3[4]
            {
                p1, p2, p3, p4
            };
            Vector3[] meshNormals = new Vector3[4]
            {
                n1, n2, n3, n4
            };
            quadMesh.vertices = meshVertices;
            quadMesh.normals = meshNormals;

            int[] meshTris = new int[6]
            {
                0, 1, 2,
                0, 2, 3
            };
            quadMesh.triangles = meshTris;

            Vector3 c = new Vector3((p1.x + p2.x + p3.x + p4.x) / 4f, (p1.y + p2.y + p3.y + p4.y) / 4f,
                (p1.z + p2.z + p3.z + p4.z) / 4f);

            GameObject meshGo = new GameObject("Quad" + i / 4);
            meshGo.transform.parent = transform;
            //meshGo.transform.position = c;
            ModelQuadVisualiser quadVisualiser = meshGo.AddComponent<ModelQuadVisualiser>();
            quadVisualiser.mesh = quadMesh;
            MeshCollider collider = meshGo.AddComponent<MeshCollider>();
            collider.sharedMesh = quadMesh;
            MeshFilter meshFilter = meshGo.AddComponent<MeshFilter>();
            meshFilter.mesh = quadMesh;
            MeshRenderer meshRenderer = meshGo.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
            quadVisualiser.Renderer = meshRenderer;
            quadVisualiser.Center = c;
            quadVisualiser.Collider = collider;


        }
    }
}

