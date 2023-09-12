using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class NoteManager : Singleton<NoteManager>
{
    private ObjectPool<Note> _pool;
    [SerializeField] private Note _notePrefab;

    private float _initalXPos;

    private void Start()
    {
        _pool = new ObjectPool<Note>(() => {
            return Instantiate(_notePrefab, transform);
        }, note => {
            note.gameObject.SetActive(true);
        }, note => {
            note.gameObject.SetActive(false);
        }, note => {
            Destroy(note.gameObject);
        }, false,
        25
        );
        
        InvokeRepeating(nameof(SpawnNote), 0f, 60f/SongManager.Instance.CurrentSongBPM);

    }

    private void OnEnable()
    {
        ActionManager.Instance.DeleteNote += ReleaseNote;
    }

    private void OnDisable()
    {
        ActionManager.Instance.DeleteNote -= ReleaseNote;
    }

    private void ReleaseNote(Note note)
    {
        _pool.Release(note);
    }

    private void SpawnNote()
    {
        Vector2 screenBounds = GameManager.Instance.ScreenBounds;
        Note note = _pool.Get();
        TextMeshProUGUI noteTMP = note.gameObject.GetComponent<TextMeshProUGUI>();
        float noteWidth = noteTMP.GetRenderedValues().x;
        float xPos = Mathf.Abs(screenBounds.x) * 2 + PlayerInput.Instance.transform.position.x;
        float YPos = -screenBounds.y * 2 * 0.33f * Random.Range(1, 4) + screenBounds.y;
        note.transform.position = new Vector3(xPos, YPos, 0);
    }
}
