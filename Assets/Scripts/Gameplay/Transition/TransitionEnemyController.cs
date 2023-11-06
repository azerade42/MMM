using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEnemyController : MonoBehaviour
{
    private List<RailPath> railPaths;
    public RailPath topRail;
    public RailPath middleRail;
    public RailPath bottomRail;
    private RailPath currentRail;
    public List<Vector3> railPositions;

    private int lastChildIndex;
    private int railPointsCompleted;
    private float railDistance;
    private float currentLerpPos;
    private float backgroundSpeed;
    private TransitionPlayerController pc;
    bool paused;

    void Start()
    {
        pc = TransitionPlayerController.Instance;
        lastChildIndex = pc.lastChildIndex + 50;
        backgroundSpeed = pc.backgroundSpeed;

        railPaths = new List<RailPath>();
        railPaths.Add(topRail);
        railPaths.Add(middleRail);
        railPaths.Add(bottomRail);
        SwitchRail();
    }

    void OnEnable()
    {
        if (pc != null)
            lastChildIndex = pc.lastChildIndex + 50;

        GameManager.Pause += () => paused = true;
        GameManager.UnPause += () => paused = false;
    }

    void OnDisable()
    {
        GameManager.Pause -= () => paused = true;
        GameManager.UnPause -= () => paused = false;
    }

    void Update()
    {
        if (paused) return;
        if (railPositions.Count <= 1) return;
        if (railPointsCompleted >= 100 || lastChildIndex <= 0) { railPointsCompleted = 0; TransitionEnemySpawner.Instance.objPool.Release(this); return; }
        
        Vector3 lastRailPos = railPositions[lastChildIndex];

        railDistance = Vector3.Distance(lastRailPos, railPositions[lastChildIndex - 1]);
        railPositions = currentRail.GetRailPath();

        currentLerpPos += Time.deltaTime * backgroundSpeed;

        if (currentLerpPos < railDistance && lastChildIndex > 0)
        {
            Vector3 nextRailPos = railPositions[lastChildIndex - 1];
        
            Vector3 currentRailPos = Vector3.Lerp(lastRailPos, nextRailPos, currentLerpPos / railDistance);
            transform.position = currentRailPos;
        }
        else if (currentLerpPos >= 1)
        {
            lastChildIndex--;
            railPointsCompleted++;
            currentLerpPos = 0;
        }
    }

    public void SwitchRail()
    {
        currentRail = railPaths[UnityEngine.Random.Range(0, railPaths.Count)];
        railPositions = currentRail.GetRailPath();
    }
    public void SwitchRail(int railIndex)
    {
        currentRail = railPaths[railIndex];
        railPositions = currentRail.GetRailPath();
    }
}
