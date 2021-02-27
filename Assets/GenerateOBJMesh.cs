using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOBJMesh : MonoBehaviour
{
    private OBJLoader objLoader;
    private Mesh mesh;
    private Material material;
    [SerializeField] private String objPath;
    [SerializeField] private BspGenerator bspGenerator;
     
   public Vector3 GetPolygonNormal( Vector3[] vertices )
   {
     Vector3 normal = Vector3.zero;
     Vector3 currVert, nextVert;
     
     for ( int i = 0; i < vertices.Length; i ++ )
     {
       currVert = vertices[ i ];
       nextVert = vertices[ ( (i + 1) % vertices.Length ) ];
 
       normal.x += ( currVert.y - nextVert.y ) * ( currVert.z + nextVert.z );
       normal.y += ( currVert.z - nextVert.z ) * ( currVert.x + nextVert.x );
       normal.z += ( currVert.x - nextVert.x ) * ( currVert.y + nextVert.y );
     }
     
     return normal.normalized;
   }
   
    void Start()
    {
        objLoader = new OBJLoader(objPath);
        material = new Material(Shader.Find("PS1"));
        if (objLoader != null)
        {
            int shapeCounter = 0;
            foreach (OBJShape shape in objLoader.shapeList)
            {
                GameObject go = new GameObject("Shape" + shapeCounter);
                MeshFilter filter = go.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
                meshRenderer.material = material;
                List<Vector3> vertexList = new List<Vector3>();
                List<Vector3> normalsList = new List<Vector3>();
                List<Vector2> uvList = new List<Vector2>();
                List<Color> colorList = new List<Color>();
                Vector3 center = Vector3.zero;
                if (shape.IndeciesVertex.Count == 3) //triangles
                {
                    Gizmos.color = new Color(1.0f,1.0f,0.0f,0.2f);
                    
                    for (int i = 0; i < shape.IndeciesVertex.Count; i++)
                    {
                        int vertexIndex = shape.IndeciesVertex[i];
                        int normalIndex = -65536;
                        int uvIndex = -65536;
                        
                        if (vertexIndex < 0)
                        {
                            vertexIndex = objLoader.vertexList.Count + vertexIndex;
                        }
                        if (shape.IndeciesNormal.Count > i && objLoader.normalsList.Count>0)
                        {
                            normalIndex = shape.IndeciesNormal[i];
                            if (normalIndex != -65536 && normalIndex < 0)
                            {
                                normalIndex = objLoader.normalsList.Count + normalIndex;
                            }
                        }
                        if (shape.IndeciesUV.Count > i && objLoader.uvList.Count>0)
                        {
                            uvIndex = shape.IndeciesUV[i];
                            if (uvIndex != -65536 && uvIndex < 0)
                            {
                                uvIndex = objLoader.uvList.Count + uvIndex;
                            }
                        }

                        center += objLoader.vertexList[vertexIndex];
                        vertexList.Add(objLoader.vertexList[vertexIndex]);
                        if(normalIndex != -65536) {
                            normalsList.Add(objLoader.normalsList[normalIndex]);
                        }
                        if(uvIndex != -65536) {
                            uvList.Add(objLoader.uvList[uvIndex]);
                        }
                        
                        colorList.Add(Color.yellow);
                    }

                    center.x /= 3f;
                    center.y /= 3f;
                    center.z /= 3f;
                    if (normalsList.Count == 0)
                    {
                        Vector3 normal = GetPolygonNormal(vertexList.ToArray());
                        for (int i = 0; i < shape.IndeciesVertex.Count; i++)
                        {
                            normalsList.Add(normal);
                        }
                    }
                    if (uvList.Count == 0)
                    {
                        uvList.Add(new Vector2(0f,0f));
                        uvList.Add(new Vector2(0f,1f));
                        uvList.Add(new Vector2(1f,0f));
                    }
                    
                    filter.mesh = new Mesh
                    {
                        vertices = vertexList.ToArray(),
                        triangles = new int[] {0,1,2},
                        colors = colorList.ToArray(),
                        normals = normalsList.ToArray(),
                        uv = uvList.ToArray()
                    };
                }
                else // quad
                {
                    Gizmos.color = new Color(0.0f,1.0f,1.0f,0.2f);
                    for (int i = 0; i < shape.IndeciesVertex.Count; i++)
                    {
                        int vertexIndex = shape.IndeciesVertex[i];
                        int normalIndex = -65536;
                        int uvIndex = -65536;
                        
                        if (vertexIndex < 0)
                        {
                            vertexIndex = objLoader.vertexList.Count + vertexIndex;
                        }
                        if (shape.IndeciesNormal.Count > i && objLoader.normalsList.Count>0)
                        {
                            normalIndex = shape.IndeciesNormal[i];
                            if (normalIndex != -65536 && normalIndex < 0)
                            {
                                normalIndex = objLoader.normalsList.Count + normalIndex;
                            }
                        }
                        if (shape.IndeciesUV.Count > i && objLoader.uvList.Count>0)
                        {
                            uvIndex = shape.IndeciesUV[i];
                            if (uvIndex != -65536 && uvIndex < 0)
                            {
                                uvIndex = shape.IndeciesUV.Count + uvIndex;
                            }
                        }

                        center += objLoader.vertexList[vertexIndex];
                        vertexList.Add(objLoader.vertexList[vertexIndex]);
                        if(normalIndex != -65536) {
                            normalsList.Add(objLoader.normalsList[normalIndex]);
                        }
                        if(uvIndex != -65536) {
                            uvList.Add(objLoader.uvList[uvIndex]);
                        }
                        
                        colorList.Add(Color.green);
                    }
                    center.x /= 4f;
                    center.y /= 4f;
                    center.z /= 4f;
                    if (normalsList.Count == 0)
                    {
                        Vector3 normal = GetPolygonNormal(vertexList.ToArray());
                        for (int i = 0; i < vertexList.Count; i++)
                        {
                            normalsList.Add(normal);
                        }
                    }
                    if (uvList.Count == 0)
                    {
                        if (vertexList.Count == 4)
                        {
                            uvList.Add(new Vector2(0f, 0f));
                            uvList.Add(new Vector2(0f, 1f));
                            uvList.Add(new Vector2(1f, 0f));
                            uvList.Add(new Vector2(1f, 1f));
                        }
                        else
                        {
                            Debug.Log("huh");
                        }
                    }

                    filter.mesh = new Mesh
                    {
                        vertices = vertexList.ToArray(),
                        triangles = new int[] {0,1,2,0,2,3},
                        colors = colorList.ToArray(),
                        normals = normalsList.ToArray(),
                        uv = uvList.ToArray()
                    };
                }
                go.transform.parent = transform;

                ModelQuadVisualiser quadVisualiser = go.AddComponent<ModelQuadVisualiser>();
                quadVisualiser.mesh = filter.mesh;
                quadVisualiser.Renderer = meshRenderer;
                quadVisualiser.Center = center;
                if(normalsList.Count>0) {
                    quadVisualiser.Normal = normalsList[0]; 
                }
                else
                {
                    quadVisualiser.Normal = GetPolygonNormal(vertexList.ToArray()); 
                }
                MeshCollider collider = go.AddComponent<MeshCollider>();
                collider.sharedMesh = filter.mesh;
                quadVisualiser.Collider = collider;
                if (shape.IndeciesNormal.Count > 0)
                {
                    for (int i = 0; i < normalsList.Count; i++)
                    {
                        normalsList[i] = -normalsList[i];
                    }

                    Mesh reverseMesh = new Mesh
                    {
                        vertices = vertexList.ToArray(),
                        normals = normalsList.ToArray(),
                        triangles = filter.mesh.triangles
                    };
                    MeshCollider collider2 = go.AddComponent<MeshCollider>();
                    collider2.sharedMesh = reverseMesh;
                }

                shapeCounter++;
            }
        }
    }
    
    void OnDrawGizmos()
    {
        if (objLoader != null)
        {
            foreach (OBJShape shape in objLoader.shapeList)
            {
                if (shape.IndeciesVertex.Count == 3) //triangles
                {
                    Gizmos.color = new Color(1.0f,1.0f,0.0f,0.2f);
                    
                    for (int i = 0; i < shape.IndeciesVertex.Count; i++)
                    {
                        Gizmos.DrawLine(
                            objLoader.vertexList[shape.IndeciesVertex[i]],
                            objLoader.vertexList[shape.IndeciesVertex[(i+1) % shape.IndeciesVertex.Count]]);
                    }
                }
                else // quad
                {
                    Gizmos.color = new Color(0.0f,1.0f,1.0f,0.2f);
                    for (int i = 0; i < shape.IndeciesVertex.Count; i++)
                    {
                        Gizmos.DrawLine(
                            objLoader.vertexList[shape.IndeciesVertex[i]],
                            objLoader.vertexList[shape.IndeciesVertex[(i+1) % shape.IndeciesVertex.Count]]);
                    }
                }
            }
        }
    }
}
