using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public Vector3 shrinkScale;
    public Vector3 moveScale;
    bool shrinkTrigger = false;
    bool moveTrigger = false;
    void Update()
    {
        if(shrinkTrigger)
            transform.localScale += shrinkScale;
        if(moveTrigger)
            transform.position += moveScale;
    }
    public void Shrink()
    {
        shrinkTrigger = !shrinkTrigger;
    }
    public void Move()
    {
        moveTrigger = !moveTrigger;
    }

}
