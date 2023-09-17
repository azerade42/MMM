using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : Singleton<ScreenManager>
{
    [HideInInspector] public bool EndSession; // ????

    [Header("Screen")]
    [SerializeField] private Vector2 _referenceResolution;
    [SerializeField] private RectMask2D _gameplayWindowBorder;
    [SerializeField] public Transform _bottomLeftScreen;
    [SerializeField] private Transform _bottomRightScreen;
    [SerializeField] public Transform _topLeftScreen;

    [Header("Camera")]
    [SerializeField] private Transform _outsideCamera;

    public GameObject testcube;

    private Vector2 _screenBounds;
    public Vector2 ScreenBounds
    {
        get { return _screenBounds; }
    }

    private Vector2 _insideScreenSize;
    public Vector2 InsideScreenSize
    {
        get { return _insideScreenSize; }
    }

    private Vector3 _bottomLeftScreenPos;
    public Vector3 BottomLeftScreenPos
    {
        get { return _bottomLeftScreenPos; }
    }

    
    public override void Init()
    {
        float xScreenBounds = _referenceResolution.x;
        float yScreenBounds = _referenceResolution.y;
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(xScreenBounds, yScreenBounds, Camera.main.transform.position.z));
        Instantiate(testcube, _screenBounds, Quaternion.identity);

        _bottomLeftScreenPos = _outsideCamera.GetComponent<Camera>().WorldToScreenPoint(_bottomLeftScreen.position);
        Vector3 bottomRightScreenPos = _outsideCamera.GetComponent<Camera>().WorldToScreenPoint(_bottomRightScreen.position);
        Vector3 topLeftScreenPos = _outsideCamera.GetComponent<Camera>().WorldToScreenPoint(_topLeftScreen.position);

        _insideScreenSize.x = bottomRightScreenPos.x - _bottomLeftScreenPos.x;
        _insideScreenSize.y = topLeftScreenPos.y - _bottomLeftScreenPos.y;
    }
}
