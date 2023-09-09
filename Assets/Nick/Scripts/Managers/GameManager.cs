using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public bool EndSession; // ????

    [Header("Screen")]
    [SerializeField] private Vector2 _referenceResolution;
    [SerializeField] private Transform _gameplayScreen;
    [SerializeField] public Transform _bottomLeftScreen;
    [SerializeField] private Transform _bottomRightScreen;
    [SerializeField] public Transform _topLeftScreen;

    [Header("Camera")]
    [SerializeField] private Transform _outsideCamera;

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
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(_referenceResolution.x, _referenceResolution.y, Camera.main.transform.position.z));

        _bottomLeftScreenPos = _outsideCamera.GetComponent<Camera>().WorldToScreenPoint(_bottomLeftScreen.position);
        Vector3 bottomRightScreenPos = _outsideCamera.GetComponent<Camera>().WorldToScreenPoint(_bottomRightScreen.position);
        Vector3 topLeftScreenPos = _outsideCamera.GetComponent<Camera>().WorldToScreenPoint(_topLeftScreen.position);

        _insideScreenSize.x = bottomRightScreenPos.x - _bottomLeftScreenPos.x;
        _insideScreenSize.y = topLeftScreenPos.y - _bottomLeftScreenPos.y;
    }
}
