using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public bool EndSession;

    private Vector2 _screenBounds;
    public Vector2 ScreenBounds
    {
        get { return _screenBounds; }
    }
    
    public override void Init()
    {
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    private void Start()
    {
    }
}
