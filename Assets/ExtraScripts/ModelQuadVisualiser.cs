using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelQuadVisualiser : MonoBehaviour
{
    public Mesh mesh;
    private bool visible = false;
    public MeshCollider Collider;
    public MeshRenderer Renderer;
    public Vector3 Center;
    private void OnDrawGizmosSelected()
    {
        /*if (Selection.Contains(transform.gameObject))*/
        {
            //Gizmos.DrawWireCube(boxCollider.center,boxCollider.size);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = visible ?  Color.white : Color.red;
        Gizmos.DrawWireMesh(mesh);
    }

    private void Update()
    {
        if (CountQuads.MainCamera == null) return;
        if (GeometryUtility.TestPlanesAABB(CountQuads.Planes, Collider.bounds))
        {
            Vector3 offset = (Center - CountQuads.MainCamera.transform.position).normalized;
            if (Physics.Linecast(Center - offset * 0.005f, CountQuads.MainCamera.transform.position ))
            {
                if (visible)
                {
                    CountQuads.QuadCount--;
                }
                visible = false;
            }
            else
            {
                if (!visible)
                {
                    CountQuads.QuadCount++;
                }
                visible = true;
                if (Vector3.Distance(Center, CountQuads.MainCamera.transform.position) < 32f)
                {
                    CountQuads.CloseQuads++;
                }
            }
        }
        else
        {
            if (visible)
            {
                CountQuads.QuadCount--;
            }
            visible = false;
        }
    }

}
