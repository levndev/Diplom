using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ProceduralGenerator))]
public class ProceduralGeneratorDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Regenerate"))
            {
                if (target is ProceduralGenerator)
                {
                    ProceduralGenerator _target = target as ProceduralGenerator;
                    _target.Regenerate();
                }
            }
            if (GUILayout.Button("Change seed and regenerate"))
            {
                if (target is ProceduralGenerator)
                {
                    ProceduralGenerator _target = target as ProceduralGenerator;
                    _target.ChangeSeed();
                    _target.Regenerate();
                }
            }
        }
    }
}
