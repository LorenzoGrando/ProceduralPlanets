using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeshGenerator generator = (MeshGenerator)target;
        
        if (GUILayout.Button("Generate Mesh"))
        {
            generator.GenerateBaseMesh();
        }
    }
}
