using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class NoteManager : Singleton<NoteManager>
{
    private ObjectPool<SafeNote> _safeTextPool;
    private ObjectPool<MalwareNote> _malwareTextPool;
    [SerializeField] private SafeNote _safePrefab;
    [SerializeField] private MalwareNote _malwarePrefab;
    [SerializeField] private RectTransform malwareCanvas;

    private float _initalXPos;

    private List<float> _lanesList;

    public float LastNoteWidth;

    private int _poolInitCount;

    private void Start()
    {
        _safeTextPool = new ObjectPool<SafeNote>(() => {
            return Instantiate(_safePrefab, malwareCanvas.transform);
        }, note => {
            note.gameObject.SetActive(true);
        }, note => {
            note.gameObject.SetActive(false);
        }, note => {
            Destroy(note.gameObject);
        }, false,
        50
        );

        _malwareTextPool = new ObjectPool<MalwareNote>(() => {
            return Instantiate(_malwarePrefab, malwareCanvas.transform);
        }, note => {
            note.gameObject.SetActive(true);
        }, note => {
            note.gameObject.SetActive(false);
        }, note => {
            Destroy(note.gameObject);
        }, false,
        50
        );

        _lanesList = CalculateYPosLanes(3);
        InvokeRepeating(nameof(SpawnNote), 60f/SongManager.Instance.CurrentSongBPM, 60f/SongManager.Instance.CurrentSongBPM);

    }

    private void OnEnable()
    {
        ActionManager.Instance.DeleteSafeNote += ReleaseSafeNote;
        ActionManager.Instance.DeleteMalwareNote += ReleaseMalwareNote;
    }

    private void OnDisable()
    {
        ActionManager.Instance.DeleteSafeNote -= ReleaseSafeNote;
        ActionManager.Instance.DeleteMalwareNote -= ReleaseMalwareNote;
    }

    private void ReleaseSafeNote(SafeNote note)
    {
        _safeTextPool.Release(note);
    }

    private void ReleaseMalwareNote(MalwareNote note)
    {
        _malwareTextPool.Release(note);
    }


    private List<float> CalculateYPosLanes(int numLanes)
    {
        List<float> lanes = new List<float>();

        float spacing = malwareCanvas.rect.height / numLanes;
        
        for (int i = 0; i < numLanes; i++)
        {
            float pos = spacing * 0.5f + spacing * i - malwareCanvas.rect.height * 0.5f;
            lanes.Add(pos);
        }

        return lanes;
    }

    private void SpawnNote()
    {
        int randomLane = Random.Range(0,_lanesList.Count);
        for (int i = 2; i < _lanesList.Count; i++)
        {
            Note note;
            
            if (i == randomLane)
            {
                note = _malwareTextPool.Get();
                TextMeshProUGUI noteTMP = note.gameObject.GetComponent<TextMeshProUGUI>();
                // noteTMP.text = "EVIL";
                
                LastNoteWidth = noteTMP.preferredWidth;
            }
            else
            {
                note = _safeTextPool.Get();
                
                TextMeshProUGUI noteTMP = note.gameObject.GetComponent<TextMeshProUGUI>();
                // while (noteTMP.preferredWidth < LastNoteWidth)
                //     noteTMP.text += Random.Range(0,2);
                
                LastNoteWidth = noteTMP.preferredWidth;
                
            }
            //Note note = _safeTextPool.Get();
            Vector2 screenBounds = ScreenManager.Instance.ScreenBounds;
            
            float xPos = Mathf.Abs(screenBounds.x) * 2 + PlayerInput.Instance.transform.position.x + 0.175f;
            float YPos = Mathf.Abs(screenBounds.y) * 2 * (_lanesList[i] / malwareCanvas.rect.height);
            note.transform.position = new Vector3(xPos, YPos, 0);
            
            //print(LastNoteWidth);
        }
    }
}
