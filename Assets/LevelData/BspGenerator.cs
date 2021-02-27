using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BspGenerator : MonoBehaviour
{
    private Vector3 middlePoint;
    public void Generate(List<VisiQuad> quads)
    {
        middlePoint = Vector3.zero;
        foreach (VisiQuad quad in quads)
        {
            middlePoint += quad.Position;
        }
        middlePoint /= quads.Count;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(middlePoint,Vector3.up * 100f);
    }
}
