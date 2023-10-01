using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float newPosition;

    [SerializeField] private Vector3 enemyOffset;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        LaneManager.NoteSpawned += MoveToNoteSpawn;
    }

    private void OnDisable()
    {
        LaneManager.NoteSpawned -= MoveToNoteSpawn;
    }

    private void MoveToNoteSpawn(Vector3 position)
    {
        StartCoroutine(MoveEnemyToPosition(position, 0.05f));
        _animator.SetTrigger("Attack");
    }

    private IEnumerator MoveEnemyToPosition(Vector3 position, float moveTime)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = position + enemyOffset;
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime/moveTime));
            //print(Vector3.Lerp(startPos, endPos, elapsedTime));
            yield return null;
        }
    }

    public void AnimFinishedAttacking()
    {
        _animator.ResetTrigger("Attack");
    }

    public void Die()
    {
        _animator.SetTrigger("Die");
    }
}
