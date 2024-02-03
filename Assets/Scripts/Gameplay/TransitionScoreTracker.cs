using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TransitionScoreTracker : Singleton<TransitionScoreTracker>
{
    public int score;

    [SerializeField] private TextMeshProUGUI scoreText;

    [HideInInspector] public int numEnemiesDodged = 0;

    private void OnEnable()
    {
        TransitionEnemySpawner.AddScore += AddScore;
        TransitionEnemyController.EnemyDied += AddNumEnemiesDodged;
        TransitionPlayerController.OnHit += RemoveNumEnemiesDodged;
    }

    private void OnDisable()
    {
        TransitionEnemySpawner.AddScore -= AddScore;
        TransitionEnemyController.EnemyDied -= AddNumEnemiesDodged;
        TransitionPlayerController.OnHit -= RemoveNumEnemiesDodged;
    }

    private void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = $"SCORE:\n {score}";
    }

    private void AddNumEnemiesDodged() => numEnemiesDodged++;

    private void RemoveNumEnemiesDodged() => numEnemiesDodged = Mathf.Clamp(--numEnemiesDodged, 0, Int32.MaxValue);
}
