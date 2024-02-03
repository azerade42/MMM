using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UINoteTiming : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _perfectText;
    [SerializeField] private TextMeshProUGUI _goodText;
    [SerializeField] private TextMeshProUGUI _missText;

    [SerializeField] private TextMeshProUGUI _perfectPopupText;
    [SerializeField] private TextMeshProUGUI _goodPopupText;

    private void OnEnable()
    {
        LaneManager.HitPerfect += EnablePerfect;
        LaneManager.HitGood += EnableGood;
        LaneManager.HitMiss += EnableMiss;
    }
    private void OnDisable()
    {
        LaneManager.HitPerfect -= EnablePerfect;
        LaneManager.HitGood -= EnableGood;
        LaneManager.HitMiss -= EnableMiss;
    }
    private void EnablePerfect()
    {
        _perfectText.gameObject.SetActive(true);
        _goodText.gameObject.SetActive(false);
        _missText.gameObject.SetActive(false);

        _perfectPopupText.gameObject.SetActive(true);
        _goodPopupText.gameObject.SetActive(false);
        StartCoroutine(PopupDisplayTime(_perfectPopupText.gameObject));
    }

    private void EnableGood()
    {
        _perfectText.gameObject.SetActive(false);
        _goodText.gameObject.SetActive(true);
        _missText.gameObject.SetActive(false);

        _goodPopupText.gameObject.SetActive(true);
        _perfectPopupText.gameObject.SetActive(false);
        StartCoroutine(PopupDisplayTime(_goodPopupText.gameObject));
    }

    private void EnableMiss()
    {
        _perfectText.gameObject.SetActive(false);
        _goodText.gameObject.SetActive(false);
        _missText.gameObject.SetActive(true);
    }

    private IEnumerator PopupDisplayTime(GameObject popup)
    {
        yield return new WaitForSeconds(0.7f);
        popup.SetActive(false);
        yield return null;
    }
}
