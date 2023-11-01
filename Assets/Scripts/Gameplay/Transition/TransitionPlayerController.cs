using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;

public class TransitionPlayerController : MonoBehaviour
{
    public static TransitionPlayerController Instance;
    [SerializeField] private ScreenMoveLeft screenMoveLeft;
    private List<RailPath> railPaths;
    [SerializeField] private RailPath topRail;
    [SerializeField] private RailPath middleRail;
    [SerializeField] private RailPath bottomRail;

    [SerializeField] private int startIndex = 4;

    private RailPath currentRail;
    public List<Vector3> railPositions;

    public int lastChildIndex { get; private set; }

    Vector3 currentRailPos;
    Vector3 startPos;
    float currentLerpPos;

    public float backgroundSpeed;

    private float railDistance;
    
    private bool inputDisabled;

    private Vector2 _screenBounds;
    public Vector2 ScreenBounds
    {
        get { return _screenBounds; }
    }

    
    [SerializeField] private Vector2 _referenceResolution;

    void Awake()
    {
        Instance = this;

        
    }

    void Start()
    {
        lastChildIndex = startIndex;
        currentRail = topRail;
        railPositions = currentRail.GetRailPath();
        startPos = railPositions[lastChildIndex];
        transform.position = startPos;

        railPaths = new List<RailPath>();
        railPaths.Add(topRail);
        railPaths.Add(middleRail);
        railPaths.Add(bottomRail);

        //InvokeRepeating(nameof(SwitchRail), 0f, 1f);
    }

    void Update()
    {
        if (railPositions.Count <= 0) return;

        railPositions = currentRail.GetRailPath();

        if (currentLerpPos == 0)
            railDistance = Vector3.Distance(railPositions[lastChildIndex], railPositions[lastChildIndex + 1]);

        currentLerpPos += Time.deltaTime * backgroundSpeed;
        Vector3 lastRailPos = railPositions[lastChildIndex];

        if (currentLerpPos < railDistance && lastChildIndex < railPositions.Count - 1)
        {
            Vector3 nextRailPos = railPositions[lastChildIndex + 1];
        
            currentRailPos = Vector3.Lerp(lastRailPos, nextRailPos, currentLerpPos / railDistance);
            screenMoveLeft.transform.position = new Vector3(-currentRailPos.x, 0, 0);
            transform.position = new Vector3(0, currentRailPos.y, currentRailPos.z);
        }
        else if (currentLerpPos >= 1)
        {
            lastChildIndex++;
            currentLerpPos = 0;
        }

        if (lastChildIndex >= railPositions.Count - 1)
        {
            lastChildIndex = 0;
            transform.position = startPos;
            screenMoveLeft.transform.position = screenMoveLeft.startPos;
            // swap to next rail
        }
    }

    private void SwitchRail()
    {
        currentRail = railPaths[UnityEngine.Random.Range(0, railPaths.Count)];
        railPositions = currentRail.GetRailPath();
    }
    private void SwitchRail(int railIndex)
    {
        currentRail = railPaths[railIndex];
        railPositions = currentRail.GetRailPath();
    }

    public void OnMoveMouse(InputAction.CallbackContext context)
    {
        if (!inputDisabled)
            Move(context.ReadValue<Vector2>());
    }

    private void Move(Vector2 mousePos)
    {
        float curHeight = Screen.height;

        if (mousePos.y > curHeight*2/3f && currentRail != topRail)
        {
            // print("top!");
            SwitchRail(0);
        }
        else if (mousePos.y > curHeight*5/12f && mousePos.y < curHeight*7/12f && currentRail != middleRail)
        {
            // print("middle!");
            SwitchRail(1);
        }
        else if (mousePos.y < curHeight/3f && currentRail != bottomRail)
        {
            // print("bottom!");
            SwitchRail(2);
        }
        else
        {
            // print("WRONG");
        }
    }
}
