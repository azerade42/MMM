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
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
