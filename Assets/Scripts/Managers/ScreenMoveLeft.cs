using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMoveLeft : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float backgroundSpeed;
    public Vector3 startPos;
    public float repeatWidth;

    void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider2D>().size.x/2;
    }
}
