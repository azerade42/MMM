using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEnemySpawner : MonoBehaviour
{
    [SerializeField] private TransitionEnemyController transitionEnemyPrefab;

    [SerializeField] private RailPath topRail;
    [SerializeField] private RailPath middleRail;
    [SerializeField] private RailPath bottomRail;

    TransitionPlayerController pc;

    private void Start()
    {
        pc = TransitionPlayerController.Instance;

        Invoke(nameof(SpawnEnemy), 2f);
    }
    private void SpawnEnemy()
    {
        TransitionEnemyController enemy = Instantiate(transitionEnemyPrefab, pc.railPositions[pc.railPositions.Count - pc.lastChildIndex], Quaternion.identity);
        enemy.topRail = topRail;
        enemy.middleRail = middleRail;
        enemy.bottomRail = bottomRail;
    }
}
