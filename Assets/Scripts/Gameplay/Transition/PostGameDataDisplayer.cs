using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PostGameDataDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeSurvivedText;
    [SerializeField] private TextMeshProUGUI malwareDodgedText;
    [SerializeField] private TextMeshProUGUI longestDodgeStreakText;
    //[SerializeField] private TextMeshProUGUI healthCollectedText;

    private void OnEnable()
    {
        float timeSurvived = TransitionEnemySpawner.Instance.TimeSurvived;
        float minutes = Mathf.FloorToInt(timeSurvived / 60);
        float seconds = Mathf.FloorToInt(timeSurvived % 60);

        scoreText.text = TransitionScoreTracker.Instance.score.ToString("F0");
        timeSurvivedText.text = string.Format("{0:00}m {1:00}s", minutes, seconds);
        //timeSurvivedText.text = TransitionEnemySpawner.Instance.TimeSurvived.ToString("F2");
        malwareDodgedText.text = TransitionScoreTracker.Instance.numEnemiesDodged.ToString("F0");
        longestDodgeStreakText.text = TransitionPlayerController.Instance.HighestDodgeStreak.ToString("F0");
    }
}
