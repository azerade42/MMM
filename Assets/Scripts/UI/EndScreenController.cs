using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenController : MonoBehaviour
{
    public EndScreen endScreen;
    public LoseScreen loseScreen;

    private void OnEnable()
    {
        GameManager.SongOver += endScreen.SetScreenActive;
        GameManager.GameOver += loseScreen.SetScreenActive;
    }

    private void OnDisable()
    {
        GameManager.SongOver -= endScreen.SetScreenActive;
        GameManager.GameOver -= loseScreen.SetScreenActive;
    }
}
