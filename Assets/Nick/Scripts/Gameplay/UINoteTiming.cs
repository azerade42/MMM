using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UINoteTiming : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _perfectText;
    [SerializeField] private TextMeshProUGUI _goodText;
    [SerializeField] private TextMeshProUGUI _missText;

    private void OnEnable()
    {
        LaneManager.HitPerfect += EnablePerfect;
        LaneManager.HitGood += EnableGood;
        LaneManager.HitMiss += EnableMiss;
    }
    private void EnablePerfect()
    {
        _perfectText.gameObject.SetActive(true);
        _goodText.gameObject.SetActive(false);
        _missText.gameObject.SetActive(false);
    }

    private void EnableGood()
    {
        _perfectText.gameObject.SetActive(false);
        _goodText.gameObject.SetActive(true);
        _missText.gameObject.SetActive(false);
    }

    private void EnableMiss()
    {
        _perfectText.gameObject.SetActive(false);
        _goodText.gameObject.SetActive(false);
        _missText.gameObject.SetActive(true);
    }

}
