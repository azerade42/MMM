using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongLengthSlider : MonoBehaviour
{
    [SerializeField] private Slider songLengthSlider;

    // [SerializeField]
    // private TextMeshProUGUI songLengthText;

    
    // private float songTime;
    // private bool stopTimer;
    // private bool startTimer;

    void OnEnable()
    {
        SongManager.SongStarted += StartSongTimer;
    }

    void OnDisable()
    {
        SongManager.SongStarted -= StartSongTimer;
    }

    // void Start()
    // {
    //     startTimer = false;
    //     stopTimer = false;

        
    // }

    // void Update()
    // {
    //     if (startTimer) 
    //     {
    //         float time = 0 + Time.time;

    //         int minutes = Mathf.FloorToInt(time / 60);
    //         int seconds = Mathf.FloorToInt(time - minutes * 60f);

    //         string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

    //         if (time <= songTime)
    //             stopTimer = true;

    //         if (stopTimer == false)
    //         {
    //             songLengthText.text = textTime;
    //             songLengthSlider.value = time;
    //         }
    //     }
    // }

    public void StartSongTimer()
    {
        StartCoroutine(StartStopWatch(AudioManager.Instance.musicSource.clip.length));
        // startTimer = true;
        // songTime = AudioManager.Instance.musicSource.clip.length;
        // songLengthSlider.maxValue = songTime;
    }

    private IEnumerator StartStopWatch(float endOfSongInSeconds)
    {
        float curTime = 0;

        while (curTime < endOfSongInSeconds)
        {
            curTime += Time.deltaTime;
            songLengthSlider.value = curTime / endOfSongInSeconds;
            yield return null;
        }
    }

}
