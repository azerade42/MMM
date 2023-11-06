using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TransitionScoreTracker : MonoBehaviour
{
    private int score;

    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        TransitionEnemySpawner.AddScore += AddScore;
    }

    private void OnDisable()
    {
        TransitionEnemySpawner.AddScore -= AddScore;
    }

    private void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = $"SCORE: {score}";
    }
}
