using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ConeMeshGenerator))]
public class ConeMeshGeneratorDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Rebuild"))
            {
                if (target is ConeMeshGenerator)
                {
                    ConeMeshGenerator _target = target as ConeMeshGenerator;
                    _target.RebuildMesh();
                }
            }
        }
    }
}
