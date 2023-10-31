using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineViewer : MonoBehaviour
{
    #if UNITY_EDITOR
    [HideInInspector] public Vector3 [] lineSegments;

    [SerializeField] private bool isLooping = true;

    public void SetLineSegments()
    {
        Transform [] children = new Transform[transform.childCount];
        int index = 0;
        foreach(Transform child in transform)
        {
            children[index++] = child;
        }

        
        lineSegments = new Vector3[children.Length * 2];

        for (int i = 0; i < children.Length; i++)
        {
            if (i == children.Length - 1)
            {
                if (!isLooping) continue;
                lineSegments[i*2] = children[children.Length - 1].position;
                lineSegments[i*2+1] = children[0].position;
            }
            else
            {
                lineSegments[i*2] = children[i].position;
                lineSegments[i*2+1] = children[i+1].position;
            }
        }
    }
    #endif
}
