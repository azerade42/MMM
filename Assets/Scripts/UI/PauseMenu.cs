using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private TransitionManager transitionLevel;

    private void Awake()
    {
        transitionLevel = FindObjectOfType<TransitionManager>();
    }
    public void PauseGame()
    {
        GameManager.TriggerPause();
    }

    public void ResumeGame()
    {
        GameManager.TriggerUnPause();
    }
    public void ToMainMenu(int sceneID)
    {
        Time.timeScale = 1f;
        transitionLevel.StartLoad(sceneID);
        AudioManager.Instance.musicSource.Stop();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
