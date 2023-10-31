using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;

public class TransitionPlayerController : MonoBehaviour
{
    private List<RailPath> railPaths;
    [SerializeField] private RailPath topRail;
    [SerializeField] private RailPath middleRail;
    [SerializeField] private RailPath bottomRail;

    [SerializeField] private int startIndex = 4;

    private RailPath currentRail;
    public List<Vector3> railPositions;

    private int lastChildIndex;

    Vector3 currentRailPos;
    Vector3 startPos;
    float currentLerpPos;

    public float backgroundSpeed;

    private float railDistance;

    
    private bool inputDisabled;

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

        if (currentLerpPos == 0)
            railDistance = Vector3.Distance(railPositions[lastChildIndex], railPositions[lastChildIndex + 1]);

        currentLerpPos += Time.deltaTime * backgroundSpeed;
        Vector3 lastRailPos = railPositions[lastChildIndex];

        if (currentLerpPos < railDistance && lastChildIndex < railPositions.Count - 1)
        {
            Vector3 nextRailPos = railPositions[lastChildIndex + 1];
        
            currentRailPos = Vector3.Lerp(lastRailPos, nextRailPos, currentLerpPos / railDistance);
            transform.position = currentRailPos;
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
        //print(mousePos.y);
        // float newMouseYPos = Mathf.Lerp(-1f, 1f, (mousePos.y - ScreenManager.Instance.BottomLeftScreenPos.y) / ScreenManager.Instance.InsideScreenSize.y);
        // playerYPos = newMouseYPos * -screenBounds.y;
        // playerYPos = Mathf.Clamp(playerYPos, screenBounds.y + playerHeight, -screenBounds.y - playerHeight);

        // if (lastYPos < newMouseYPos)
        //     _playerAnim.SetFloat("mouseYPos", 1);
        // else if (lastYPos > newMouseYPos)
        //     _playerAnim.SetFloat("mouseYPos", -1);
        // else
        //     _playerAnim.SetFloat("mouseYPos", 0);
        
        // if (++frameCount > 10)
        // {
        //     lastYPos = newMouseYPos;
        //     frameCount = 0;
        // }
        if (mousePos.y > 620 && currentRail != topRail)
        {
            // print("top!");
            SwitchRail(0);
        }
        else if (mousePos.y > 440 && mousePos.y < 600 && currentRail != middleRail)
        {
            // print("middle!");
            SwitchRail(1);
        }
        else if (mousePos.y < 400 && currentRail != bottomRail)
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
