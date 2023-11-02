using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEnemySpawner : MonoBehaviour
{
    [SerializeField] private TransitionEnemyController transitionEnemyPrefab;

    [SerializeField] private RailPath topRail;
    [SerializeField] private RailPath middleRail;
    [SerializeField] private RailPath bottomRail;

    // [SerializeField] private ParticleSystem sparkEffect;
    TransitionPlayerController pc;

    private void Start()
    {
        pc = TransitionPlayerController.Instance;

        InvokeRepeating(nameof(SpawnEnemy), 0f, 1f);
    }
    private void SpawnEnemy()
    {
        TransitionEnemyController enemy = Instantiate(transitionEnemyPrefab, Vector3.zero, Quaternion.identity);
        // Instantiate(sparkEffect, enemy.transform.position, enemy.transform.rotation);
        enemy.topRail = topRail;
        enemy.middleRail = middleRail;
        enemy.bottomRail = bottomRail;
    }
}
