using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    void OnEnable()
    {
        ActionManager.Instance.MoveNotesHalf += MoveObject;
    }

    void OnDisable()
    {
        ActionManager.Instance.MoveNotesHalf -= MoveObject;
    }
    void MoveObject()
    {
        transform.position -= Vector3.right * Mathf.Abs(GameManager.Instance.ScreenBounds.x) * 0.2f;

        if (transform.position.x < PlayerInput.Instance.transform.position.x)
        {
            ActionManager.Instance.DeleteNote(this);
        }

    }

    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime;
    }
}
