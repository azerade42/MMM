using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NoteManager : MonoBehaviour
{
    private ObjectPool<Note> _pool;
    [SerializeField] private Note _notePrefab;
    [SerializeField] private int _spawnAmount;

    private void Start()
    {
        _pool = new ObjectPool<Note>(() => {
            return Instantiate(_notePrefab);
        }, note => {
            note.gameObject.SetActive(true);
        }, note => {
            note.gameObject.SetActive(false);
        }, note => {
            Destroy(note.gameObject);
        }, false,
        25
        );
        
        InitializePool();

    }

    private void InitializePool()
    {
        Vector2 screenBounds = GameManager.Instance.ScreenBounds;
        
        for (int i = 0; i < _spawnAmount; i++)
        {
            Note note = _pool.Get();
            note.transform.position = new Vector3(screenBounds.x, 0, 0);
        }
    }

    
}
