using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSwapper : MonoBehaviour
{
    [SerializeField] private Texture2D cursorArrow;

    private void Start()
    {
        SwapCursor();
    }
    private void SwapCursor()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }
}
