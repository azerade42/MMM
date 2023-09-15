using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeNote : Note
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
        gameObject.GetComponent<RectTransform>().anchoredPosition -= Vector2.right * NoteManager.Instance.LastNoteWidth;

        if (transform.position.x < PlayerInput.Instance.transform.position.x)
        {
            ActionManager.Instance.DeleteSafeNote(this);
        }

    }
}
