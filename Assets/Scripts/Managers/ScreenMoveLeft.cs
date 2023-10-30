using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMoveLeft : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10f;
    Vector3 startPos;
    float repeatWidth;
    public GameObject bg1;
    public GameObject bg2;
    Camera whatever;
    void Start()
    {
        whatever = Camera.main;
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider2D>().size.x/2;
    }

    // Update is called once per frame
    void Update()
    {
        whatever.transform.position += Vector3.right * Time.deltaTime * speed;
        if(transform.position.x < startPos.x - repeatWidth ){
            transform.position = startPos;
        }
    }
}
