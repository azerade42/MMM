using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    // public AudioSource hitSFX;
    // public AudioSource missSFX;
    public TextMeshProUGUI scoreText;
    // public Slider healthSlider;
    public GameObject endScreen;
    public GameObject loseScreen;
    public TextMeshProUGUI menuScore;
    public float rateOfChange;
    public int healthLoss;
    

    static int comboScore;
    static float scoreTrack;
    static int meterScore;
    // static int health;
   
    void Start()
    {
        Instance = this;
        comboScore = 0;
        scoreTrack = 0;
        meterScore = 0;
        // health = 50;
        // healthSlider.value = health;
    }
    public static void PerfectHit()
    {
        comboScore += 250;
        meterScore += 1;
        // health += meterScore / 2;
        // Instance.hitSFX.Play();
    }
    public static void GoodHit()
    {
        comboScore += 100;
        meterScore += 1;
        // health += meterScore / 2;
        // Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        if (comboScore > 70)
            comboScore -= 70;
        else
            comboScore = 0;
        meterScore = 0;
        // health -= 5;
        // Instance.missSFX.Play();    
    }
    private void Update()
    {
        // if (health >= 100)
        //     health = 100;
        // else if (health <= 0)
        //     health = 0;
        // healthSlider.value = health;
        scoreText.text = meterScore.ToString();
        if (endScreen.activeSelf && scoreTrack < comboScore)
        {
            scoreTrack += rateOfChange * Time.deltaTime;
            menuScore.text = scoreTrack.ToString("F0");
            MainMenu.levelSelectEnabled = true;
        }
        // if(healthSlider.value <= 0)
        // {
        //     loseScreen.SetActive(true);
        //     health = 50;
        //     healthSlider.value = health;
        //     GameManager.TriggerPause();
        // }
    }
}