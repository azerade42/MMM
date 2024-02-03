using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEnemyController : MonoBehaviour
{
    public static Action EnemyDied;

    private List<RailPath> railPaths;
    public RailPath topRail;
    public RailPath middleRail;
    public RailPath bottomRail;
    private RailPath currentRail;
    public List<Transform> railTransforms;

    private int lastChildIndex;
    private int railPointsCompleted;
    private float railDistance;
    private float currentLerpPos;
    private float backgroundSpeed;
    private TransitionPlayerController pc;
    bool paused;

    private int frameCount;

    void Start()
    {
        pc = TransitionPlayerController.Instance;
        lastChildIndex = pc.lastChildIndex + 50;
        backgroundSpeed = pc.backgroundSpeed;       
    }

    void OnEnable()
    {
        if (pc != null)
            lastChildIndex = pc.lastChildIndex + 10;

        GameManager.Pause += () => paused = true;
        GameManager.UnPause += () => paused = false;
        TransitionPlayerController.OnUpdateSpeed += UpdateSpeed;

        railPaths = new List<RailPath>();
        railPaths.Add(TransitionEnemySpawner.Instance.topRail);
        railPaths.Add(TransitionEnemySpawner.Instance.middleRail);
        railPaths.Add(TransitionEnemySpawner.Instance.bottomRail);

        SwitchRail();
    }

    void OnDisable()
    {
        GameManager.Pause -= () => paused = true;
        GameManager.UnPause -= () => paused = false;
        TransitionPlayerController.OnUpdateSpeed += UpdateSpeed;
    }

    void Update()
    {
        if (paused) return;
        if (railTransforms.Count <= 1) return;
        if (railPointsCompleted >= 8 || lastChildIndex <= 0)
        {
            railPointsCompleted = 0;
            TransitionEnemySpawner.Instance.objPool.Release(this);
            EnemyDied?.Invoke();
            return;
        }
        
        Vector3 lastRailPos = railTransforms[lastChildIndex].position;

        railDistance = Vector3.Distance(lastRailPos, railTransforms[lastChildIndex - 1].position);

        //if (++frameCount > 3) { railPositions = currentRail.GetRailPath(); frameCount = 0; return; }
        //railPositions[lastChildIndex - 1] = currentRail.GetNextRailPos();

        currentLerpPos += Time.deltaTime * backgroundSpeed;

        if (currentLerpPos < railDistance && lastChildIndex > 0)
        {
            Vector3 nextRailPos = railTransforms[lastChildIndex - 1].position;
        
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
        railTransforms = currentRail.GetRailPathTransforms();
    }
    public void SwitchRail(int railIndex)
    {
        currentRail = railPaths[railIndex];
        railTransforms = currentRail.GetRailPathTransforms();
    }

    private void UpdateSpeed() => backgroundSpeed = pc.backgroundSpeed;

}
