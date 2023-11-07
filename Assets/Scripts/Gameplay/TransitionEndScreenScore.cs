using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionEndScreenScore : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<TextMeshProUGUI>().text = TransitionScoreTracker.Instance.score.ToString("F0");
    }
}
