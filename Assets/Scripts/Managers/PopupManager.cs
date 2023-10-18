using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private RectTransform _desktop;
    // [SerializeField] private Dictionary<RenderTexture, PopupType> _popups;
    [SerializeField] private List<PopupSO> _popups;
    [SerializeField] private RectTransform _widescreenWindowPrefab;
    [SerializeField] private RectTransform _squareWindowPrefab;
    // [SerializeField] private float _spawnFrequency = 1f;

    [SerializeField] private bool _spawnInRandomPositions = true;
    [SerializeField] private Vector3 _popupSpawnPosition;

    private ObjectPool<RectTransform> _widescreenPopupPool;
    private ObjectPool<RectTransform> _squarePopupPool;

    [HideInInspector] public List<double> popupTimes = new List<double>();
    private int popupIndex = 0;


    protected override void Init()
    {
        _widescreenPopupPool = new ObjectPool<RectTransform>(() => {
            return Instantiate(_widescreenWindowPrefab, _desktop.transform, false);
            }, popup => {
                popup.gameObject.SetActive(true);
            }, popup => {
                popup.gameObject.SetActive(false);
            }, popup => {
                Destroy(popup);
            }, false, 10, 25);
        
        _squarePopupPool = new ObjectPool<RectTransform>(() => {
            return Instantiate(_squareWindowPrefab, _desktop.transform, false);
            }, popup => {
                popup.gameObject.SetActive(true);
            }, popup => {
                popup.gameObject.SetActive(false);
            }, popup => {
                Destroy(popup);
            }, false, 10, 25);
    }

    private void OnEnable()
    {
        LaneManager.ReadPopupNote += AddPopupToTimes;
    }

    private void OnDisable()
    {
        LaneManager.ReadPopupNote -= AddPopupToTimes;
    }

    private void AddPopupToTimes(double popupTime)
    {
        popupTimes.Add(popupTime);
    }

    private void SpawnPopup(Vector3 position)
    {
        RectTransform popup;
        PopupSO randomPopup = _popups[Random.Range(0, _popups.Count)];
        switch (randomPopup.type)
        {
            case PopupType.SquareVideo:
                popup = _squarePopupPool.Get();
                popup.GetComponentInChildren<RawImage>().texture = randomPopup.texture;
                popup.gameObject.transform.position = position;
                popup.localScale = Vector3.one * randomPopup.scale;
                break;
            case PopupType.WidescreenVideo:
                popup = _widescreenPopupPool.Get();
                popup.GetComponentInChildren<RawImage>().texture = randomPopup.texture;
                popup.gameObject.transform.position = position;
                popup.localScale = Vector3.one * randomPopup.scale;
                break;
        }
    }

    private void SpawnPopup()
    {
        RectTransform popup;
        PopupSO randomPopup = _popups[Random.Range(0, _popups.Count)];
        switch (randomPopup.type)
        {
            case PopupType.SquareVideo:
                popup = _squarePopupPool.Get();
                popup.GetComponentInChildren<RawImage>().texture = randomPopup.texture;
                popup.gameObject.transform.position = ChooseRandomPopupPosition(0f, 0f);
                popup.localPosition = new Vector3(popup.localPosition.x, popup.localPosition.y, 0);
                break;
            case PopupType.WidescreenVideo:
                popup = _widescreenPopupPool.Get();
                popup.GetComponentInChildren<RawImage>().texture = randomPopup.texture;
                popup.gameObject.transform.position = ChooseRandomPopupPosition(0f, 0f);
                popup.localPosition = new Vector3(popup.localPosition.x, popup.localPosition.y, 0);
                break;
        }
    }

    private Vector3 ChooseRandomPopupPosition(float length, float height)
    {
        Vector2 outsideBounds = ScreenManager.Instance.OutsideScreenBounds;
        float randomXPos = UnityEngine.Random.Range(-outsideBounds.x / 2 - length / 2, outsideBounds.x / 2 - length / 2);
        float randomYPos = UnityEngine.Random.Range(-outsideBounds.y / 2 - height / 2, outsideBounds.y / 2 - height / 2);
        
        return _desktop.transform.position + new Vector3(randomXPos, randomYPos, 0);
    }

    private void Start()
    {
        // InvokeRepeating(nameof(SpawnPopup), 2f, 1/_spawnFrequency);
    }

    private void Update()
    {
        if (popupIndex < popupTimes.Count && SongManager.GetAudioSourceTime() >= popupTimes[popupIndex])
        {
            if (_spawnInRandomPositions)
                SpawnPopup();
            else
                SpawnPopup(_popupSpawnPosition);
            
            popupIndex++;
        }
    }
}
