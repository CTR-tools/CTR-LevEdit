using System;
using System.Collections;
using System.Collections.Generic;
using CTRFramework;
using UnityEditor;
using UnityEngine;

public class VisInstance : MonoBehaviour
{
    public VisData Visi;
    private SceneHandler sceneHandler;
    [SerializeField] private int leftChild;
    [SerializeField] private int rightChild;
    [SerializeField] private uint numQuadBlock;
    [SerializeField] private uint ptrQuadBlock;
    [SerializeField] private uint u1;
    [SerializeField] private uint ptrUnkData;
    public VisInstance()
    {
    }
    public VisInstance(VisData visi)
    {
        Visi = visi;
        leftChild = visi.leftChild;
        rightChild = visi.rightChild;
        u1 = visi.u1;
        ptrUnkData = visi.ptrUnkData;
        numQuadBlock = Visi.numQuadBlock;
        ptrQuadBlock = Visi.ptrQuadBlock;
    }

    public void SetSceneHandler(SceneHandler sh)
    {
        sceneHandler = sh;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!Visi.IsLeaf)
        {
            leftChild = Visi.leftChild;
            rightChild = Visi.rightChild;
            if (Selection.Contains(transform.gameObject)) {
                Gizmos.color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                Gizmos.color = new Color(1, 1, 1, 0.1f);
            }
            Vector3 bmax = new Vector3(Visi.bbox.Max.X / 255.0f, Visi.bbox.Max.Y / 255.0f, -Visi.bbox.Max.Z / 255.0f);
            Vector3 bmin = new Vector3(Visi.bbox.Min.X / 255.0f, Visi.bbox.Min.Y / 255.0f, -Visi.bbox.Min.Z / 255.0f);
            Vector3 bsize = new Vector3(Mathf.Abs(bmax.x - bmin.x), Mathf.Abs(bmax.y - bmin.y), Mathf.Abs(bmax.z - bmin.z));
            Vector3 bpos = new Vector3((bmax.x + bmin.x) / 2.0f, (bmax.y + bmin.y) / 2.0f,
                (bmax.z + bmin.z) / 2.0f);
            transform.position = sceneHandler.transform.position + bpos;

            Gizmos.DrawWireCube(transform.position, bsize);
            if (Selection.Contains(transform.gameObject)) {
                Handles.Label(transform.position, Visi.ToString());
            }
        }
        else
        {
            numQuadBlock = Visi.numQuadBlock;
            ptrQuadBlock = Visi.ptrQuadBlock;
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawWireCube(transform.parent.position, new Vector3(1, 1, 1));
            if (Selection.Contains(transform.gameObject)) {
                Handles.Label(transform.parent.position, Visi.ToString());
            }
            Gizmos.color = new Color(1, 0, 0, 0.333f);
            //if (Selection.Contains(transform.gameObject)) 
            {
                Gizmos.color = new Color(1, 1, 0, 0.333f);
                ptrQuadBlock = ptrQuadBlock;
                //ptrQuadBlock = ptrQuadBlock >> 16;
                ptrQuadBlock = (uint) (((ptrQuadBlock) / LevelShift.Div)  + LevelShift.Offset);
                QuadBlock[] quads = sceneHandler.quads.ToArray();
                for (int i = 0; i < numQuadBlock; i++)
                {
                    long pointer = ptrQuadBlock + i;
                    if (pointer < 0 || pointer > quads.Length - 1) continue;
                    QuadBlock quad = quads[pointer];
                    Vertex[] vertList = quad.GetVertexList(sceneHandler).ToArray();
                    for (int j = 0; j < vertList.Length-1; j++)
                    {
                        Vertex vertA = vertList[j];
                        Vertex vertB = vertList[j+1];
                        Vector3 a = vertA.coord.GetVector3();
                        Vector3 b = vertB.coord.GetVector3();
                        var position = sceneHandler.transform.position;
                        a += position;
                        b += position;
                        Gizmos.DrawLine(a,b);
                    }

                    LevelShift.PolyHits++;
                }
            }
        }
    }
}