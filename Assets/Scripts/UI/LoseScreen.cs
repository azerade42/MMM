using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    public void SetScreenActive()
    {
        AudioManager.Instance.musicSource.Stop();
        gameObject.SetActive(true);
        AudioManager.Instance.PlayWorld("LoseSound");
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevelSelect()
    {
        Time.timeScale = 1f;
        MainMenu.levelSelectEnabled = true;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
