using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LineViewer))]
public class LineViewerEDITOR : Editor
{
    private void OnEnable()
    {
        SceneView.duringSceneGui += CustomOnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= CustomOnSceneGUI;
    }

    public void CustomOnSceneGUI(SceneView scene)
    {
        var lineViewer = target as LineViewer;
        
        var color = new Color(1, 0.8f, 0.4f, 1);
        Handles.color = color;

        lineViewer.SetLineSegments();

        if (lineViewer.lineSegments.Length > 1)
            Handles.DrawLines(lineViewer.lineSegments);
    }
}
#endif

