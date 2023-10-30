using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 startOffset;

    private void Start()
    {
        startOffset = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 newCameraPos = _target.position + startOffset;
        transform.position = new Vector3(newCameraPos.x + 40f, 0, startOffset.z);
    }

}
