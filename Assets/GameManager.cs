using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool EndSession;

    private Vector2 _screenBounds;
    public Vector2 ScreenBounds
    {
        get { return _screenBounds; }
    }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Cannot have multiple instances of singleton.");
            return;
        }

        Instance = this;

        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    private void Start()
    {
    }
}
