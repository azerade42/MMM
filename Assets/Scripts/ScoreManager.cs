using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TextMeshPro scoreText;
    
    public GameObject endScreen;
    public TextMeshProUGUI menuScore;
    static int comboScore;
    static float scoreTrack;
    static int meterScore;
    public float rateOfChange;
   
    void Start()
    {
        Instance = this;
        comboScore = 0;
        scoreTrack = 0;
        meterScore = 0;
    }
    public static void PerfectHit()
    {
        comboScore += 250;
        meterScore += 1;
        Instance.hitSFX.Play();
    }
    public static void GoodHit()
    {
        comboScore += 100;
        meterScore += 1;
        Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        if (comboScore > 70)
            comboScore -= 70;
        else
            comboScore = 0;
        meterScore = 0;
        Instance.missSFX.Play();    
    }
    private void Update()
    {
        scoreText.text = meterScore.ToString();
        if (endScreen.activeSelf && scoreTrack < comboScore)
        {
            scoreTrack += rateOfChange * Time.deltaTime;
            menuScore.text = scoreTrack.ToString("F0");
        }
    }
}