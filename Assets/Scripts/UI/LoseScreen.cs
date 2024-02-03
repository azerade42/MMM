using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    private TransitionManager transitionLevel;
    // public MainMenu mainMenu;
    private void Awake()
    {
        transitionLevel = FindObjectOfType<TransitionManager>();
        // private MainMenu mainMenu;

    }
    public void SetScreenActive()
    {
        AudioManager.Instance.musicSource.Stop();
        gameObject.SetActive(true);
        AudioManager.Instance.worldSource.loop = true;
        AudioManager.Instance.PlayWorld("LoseSound");
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        transitionLevel.StartLoad(SceneManager.GetActiveScene().buildIndex);

        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevelSelect(int sceneID)
    {
        Time.timeScale = 1f;
        // mainMenu.levelSelectEnabled = true;
        AudioManager.Instance.worldSource.loop = false;
        transitionLevel.StartLoad(sceneID);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
