using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Action GameOver;
    public static Action SongOver;
    public static Action Pause;
    public static Action UnPause;

    public static void TriggerGameOver()
    {
        GameOver?.Invoke();
        Time.timeScale = 0f;
        AudioManager.Instance.musicSource.Stop();
        PlayerInput.Instance.DisableInput();
    }

    public static void TriggerSongOver()
    {
        SongOver?.Invoke();
        Time.timeScale = 0f;
        AudioManager.Instance.musicSource.Stop();
        PlayerInput.Instance.DisableInput();
    }

    public static void TriggerPause()
    {
        Time.timeScale = 0f;
        Pause?.Invoke();
        AudioManager.Instance.musicSource.Pause();
    }

    public static void TriggerUnPause()
    {
        Time.timeScale = 1f;
        UnPause?.Invoke();
        AudioManager.Instance.musicSource.Play();
    }
}
