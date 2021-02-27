using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJShape : MonoBehaviour
{
    public List<int> IndeciesVertex;
    public List<int> IndeciesUV;
    public List<int> IndeciesNormal;

    public OBJShape()
    {
        IndeciesVertex = new List<int>();
        IndeciesUV = new List<int>();
        IndeciesNormal = new List<int>();
    }
}
