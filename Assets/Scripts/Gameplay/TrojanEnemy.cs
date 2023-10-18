using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrojanEnemy : MonoBehaviour
{
    private float newPosition;

    [SerializeField] private Vector3 trojanEnemyOffset;
    //[SerializeField] private Vector3 spiderEnemyOffset;

    [SerializeField] private float moveTimeToNextNote = 0.05f;
    [SerializeField] private GameObject _spiderman;
    [SerializeField] private List<GameObject> _horseObjects;

    private Animator _animator;

    // bool spiderUnleashed = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        LaneManager.NoteSpawned += MoveToNoteSpawn;
        LaneManager.BeatDrop += TrojanExplosion;
    }

    private void OnDisable()
    {
        LaneManager.NoteSpawned -= MoveToNoteSpawn;
        LaneManager.BeatDrop -= TrojanExplosion;
    }

    private void MoveToNoteSpawn(Vector3 position)
    {
        StartCoroutine(MoveEnemyToPosition(position, moveTimeToNextNote));
        _animator.SetTrigger("Attack");
    }

    private IEnumerator MoveEnemyToPosition(Vector3 position, float moveTime)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.localPosition;
        //Vector3 endPos = spiderUnleashed ? position + spiderEnemyOffset : position + trojanEnemyOffset;
        Vector3 endPos = position + trojanEnemyOffset;
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, endPos, (elapsedTime/moveTime));
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

    private void TrojanExplosion()
    {
        // spiderUnleashed = true;
        _spiderman.SetActive(true);

        foreach (GameObject horseObject in _horseObjects)
            horseObject.SetActive(false);

        _animator = _spiderman.GetComponent<Animator>();
    }
}
