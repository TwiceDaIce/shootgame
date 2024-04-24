using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConeVisualizer))]
public class ConeVisualizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ConeVisualizer coneVisualizer = (ConeVisualizer)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Recalculate Mesh"))
        {
            coneVisualizer.CreateConeMesh();
        }
    }
}
