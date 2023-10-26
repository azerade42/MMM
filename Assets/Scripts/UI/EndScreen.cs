using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // private void Awake()
    // {
    //     GameManager.SongOver += SetScreenActive;
    //     gameObject.SetActive(false);
    // }
    // private void OnDisable()
    // {
    //     GameManager.SongOver -= SetScreenActive;
    // }

    private TransitionManager transitionLevel;

    private void Awake()
    {
        transitionLevel = FindObjectOfType<TransitionManager>();
    }
    public void SetScreenActive()
    {
        AudioManager.Instance.musicSource.Stop();
        gameObject.SetActive(true);
        AudioManager.Instance.PlayWorld("WinSound");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        transitionLevel.StartLoad(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel(int sceneID)
    {
        Time.timeScale = 1f;
        transitionLevel.StartLoad(SceneManager.GetActiveScene().buildIndex + 1);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadLevelSelect(int sceneID)
    {
        Time.timeScale = 1f;
        transitionLevel.StartLoad(sceneID);
        // SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
