using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Window : MonoBehaviour
{
    private RectTransform _window;

    void Awake()
    {
        _window = GetComponent<RectTransform>();
    }

    public void Close()
    {
        gameObject.SetActive(false);

    }

    public void DeactivateInput()
    {
        PlayerInput.Instance.DisableInput();
    }
}
