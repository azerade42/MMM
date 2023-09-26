using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Action GameOver;

    public void TriggerGameOver()
    {
        GameOver?.Invoke();
    }
}
