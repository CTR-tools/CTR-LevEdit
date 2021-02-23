using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshQuadNeighbors
 {
     public class Vertex
     {
         public int Index;
         public Vector3 Position;
         public List<int> Connections;

         public Vertex(int index, Vector3 position)
         {
             Index = index;
             Position = position;
             Connections = new List<int>();
         }

         public bool AddConnection(int connection)
         {
             if (connection == Index)
             {
                 return false;
             }
             else if (Connections.Contains(connection))
             {
                 return false;
             }
             else
             {
                 Connections.Add(connection);
                 return true;
             }
         }
     }

     public static List<int> GetNeighbors(Mesh aMesh)
     {
         var quadVertexList = new List<int>();
         var tris = aMesh.triangles;
         var verts = aMesh.vertices;
         var vertexs = new List<Vertex>();
         int i = 0;
         foreach (var vert in verts)
         {
             vertexs.Add(new Vertex(i++, vert));
         }

         foreach (var vert in vertexs)
         {
             for(int v=1; v<tris.Length-1; v+=3)
             {
                 if (tris[v+0] == vert.Index)
                 {
                     vert.AddConnection(tris[v+1]);
                     vert.AddConnection(tris[v+2]);
                 }
                 if (tris[v+1] == vert.Index)
                 {
                     vert.AddConnection(tris[v+0]);
                     vert.AddConnection(tris[v+2]);
                 }
                 if (tris[v+2] == vert.Index)
                 {
                     vert.AddConnection(tris[v+0]);
                     vert.AddConnection(tris[v+1]);
                 }
             }
         }
         
         //ok they are now connected
         //do quads
         
         foreach (var vert in vertexs)
         {
             foreach (var neighbour in vert.Connections)
             {
                 if (vert.Index != vertexs[neighbour].Index)
                 {
                     foreach (var neighbour2 in vertexs[neighbour].Connections)
                     {
                         if (vert.Index != vertexs[neighbour2].Index &&
                             vertexs[neighbour].Index != vertexs[neighbour2].Index)
                         {
                             foreach (var neighbour3 in vertexs[neighbour2].Connections)
                             {
                                 if (vert.Index != vertexs[neighbour3].Index &&
                                     vertexs[neighbour].Index != vertexs[neighbour3].Index &&
                                     vertexs[neighbour2].Index != vertexs[neighbour3].Index)
                                 {
                                     foreach (var neighbour4 in vertexs[neighbour3].Connections)
                                     {
                                         if (vert.Index == vertexs[neighbour4].Index)
                                         {
                                             // is quad
                                             //quadVertexList.Add(vert.Index);
                                             quadVertexList.Add(vertexs[neighbour].Index);
                                             quadVertexList.Add(vertexs[neighbour2].Index);
                                             quadVertexList.Add(vertexs[neighbour3].Index);
                                             quadVertexList.Add(vertexs[neighbour4].Index);
                                         }
                                     }
                                 }
                             }
                         }
                     }
                 }
             }
         }

         return quadVertexList;
     }
 }