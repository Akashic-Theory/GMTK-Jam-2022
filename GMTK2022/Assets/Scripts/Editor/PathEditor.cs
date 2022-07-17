using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    SerializedProperty points;
    public float size = 0.5f;
    Path path;

    private Vector3 spawn = Vector3.zero;

    private void OnEnable()
    {
        path = (target as Path);
        points = serializedObject.FindProperty("points");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();

        size = EditorGUILayout.FloatField("Handle Size", size);

        if (GUILayout.Button("Add Point"))
        {
            points.InsertArrayElementAtIndex(points.arraySize);
            points.GetArrayElementAtIndex(points.arraySize - 1).vector3Value = spawn;
        }

        if (GUILayout.Button("Remove Point") && points.arraySize != 0)
        {
            points.DeleteArrayElementAtIndex(points.arraySize - 1);
        }

        if (GUILayout.Button("Clear Points"))
        {
            points.ClearArray();
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        if (path.points.Length == 0)
            return;

        List<Vector3> handles = new List<Vector3>();
        Vector3 pos = path.transform.position;
        Vector3? lastPos = null;

        EditorGUI.BeginChangeCheck();

        foreach (Vector3 p in path.points)
        {
            Vector3 v = Handles.Slider2D(p + pos, Vector3.up, Vector3.right, Vector3.forward, size, Handles.CircleHandleCap, 0f) - pos;

            if(lastPos != null)
                Handles.DrawLine((Vector3)lastPos, v + pos);

            lastPos = v + pos;

            v.x = Mathf.RoundToInt(v.x);
            v.z = Mathf.RoundToInt(v.z);
            handles.Add(new Vector3(v.x, 0f, v.z));
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Modified Bounds");

            path.points = handles.ToArray();
        }
    }
}
