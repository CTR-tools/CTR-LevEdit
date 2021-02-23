
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelShift : MonoBehaviour
{
    public static int Offset = -124;
    public static int Div = 92;
    public static int Overreach = 0;
    public static int PolyHits = 0;

    [SerializeField] public int offset = 0;
    [SerializeField] public int div = 91;
    [SerializeField] public int overreach ;
    private int max;

    void Update()
    {/*
        int t = (int)(Time.time * 80f);
        Offset = (t & 255) - 127;
        Div = (t >> 8) + 64;*/
        Offset = offset;
        Div = Mathf.Max(div,1);
        Overreach = Mathf.Max(overreach,0);
    }

    private void LateUpdate()
    {
        if (max < PolyHits)
        {
            max = PolyHits;
            offset = Offset;
            div = Div;
        }
       // Debug.Log(Offset + "; " + Div + "; " + PolyHits + "   max  " + offset + "; " + div + "; " + max);
        PolyHits=0;
    }
}
