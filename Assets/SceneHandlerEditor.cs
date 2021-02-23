using System.Collections;
using System.Collections.Generic;
using CTRFramework;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneHandler))]
public class SceneHandlerEditor : Editor
{
    SceneHandler script;
    GameObject scriptObject;
 
    void OnEnable() {
        script = (SceneHandler) target;
        scriptObject = script.gameObject;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Save"))
        {
            script.Write();
        }
    }
}