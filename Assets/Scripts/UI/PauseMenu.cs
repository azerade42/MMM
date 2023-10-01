using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void PauseGame()
    {
        GameManager.TriggerPause();
    }

    public void ResumeGame()
    {
        GameManager.TriggerUnPause();
    }
    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        AudioManager.Instance.musicSource.Stop();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
