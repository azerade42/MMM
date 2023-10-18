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
    public void SetScreenActive()
    {
        AudioManager.Instance.musicSource.Stop();
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        //TO DO BROKEN
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        //TO DO BROKEN
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
