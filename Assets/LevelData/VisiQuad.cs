using System.Collections;
using System.Collections.Generic;
using CTRFramework;
using UnityEditor;
using UnityEngine;

public class VisiQuad : MonoBehaviour
{
    public Vector3 Position { get; set; }
    public int LeafBspID;
    public QuadBlock Qb;

    private void OnDrawGizmosSelected()
    {
        if (Selection.Contains(transform.gameObject)) {
            Handles.Label(Position, $"bitvalue = {Qb.bitvalue} | midunk = {Qb.midunk} | offset2 = {Qb.offset2} | trackPos = {Qb.trackPos}\r\n | extradata = {Qb.extradata} | quadFlags = {Qb.quadFlags} | trackPos = {Qb.WeatherIntensity} | WeatherType = {Qb.WeatherType}");
        }
    }
}
