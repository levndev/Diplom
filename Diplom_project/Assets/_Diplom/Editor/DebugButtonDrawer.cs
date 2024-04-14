using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugButton))]
public class DebugButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Do action"))
            {
                if (target is DebugButton)
                {
                    DebugButton _target = target as DebugButton;
                    _target.action?.Invoke();
                }
            }
        }
    }
}
