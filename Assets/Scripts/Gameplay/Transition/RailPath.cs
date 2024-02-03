using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailPath : MonoBehaviour
{
    public List<Vector3> GetRailPath()
    {
        List<Vector3> childPositions = new List<Vector3>();
        foreach(Transform child in transform)
        {
            childPositions.Add(child.transform.position);
        }

        return childPositions;
    }

    public List<Transform> GetRailPathTransforms()
    {
        List<Transform> childTransforms = new List<Transform>();

        foreach (Transform child in transform)
            childTransforms.Add(child.transform);
        
        return childTransforms;
    }
}
