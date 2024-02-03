using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMoveLeft : MonoBehaviour
{
    public float speed = 10f;   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
