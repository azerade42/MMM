﻿using EZCameraShake;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class LaneManager : Singleton<LaneManager>
{
    // public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;

    // The prefab that will be used for the note
    public Note _notePrefab;
    
    public LongNote _longNotePrefab;
    // The list of all notes in the song
    List<Note> notes = new List<Note>();
    List<LongNote> longNotes = new List<LongNote>();
    // The times in the song where the notes are positioned
    [HideInInspector] public List<double> timeStamps = new List<double>();
    // The Y positions for each note that correlate with each time stamp
    [HideInInspector] public List<int> timeYIndex = new List<int>();
    [HideInInspector] public List<bool> heldNotes = new List<bool>();
    // The queue created for releasing notes from the note pool
    private Queue<int> noteDeletionIndexQueue = new Queue<int>();
    // The object pool created for preallocating note objects to replace instantiation/destroy calls at runtime
    public ObjectPool<Note> _notePool;
    public ObjectPool<LongNote> _longNotePool;

    // Indexes for tracking the current note in the song
    private int spawnIndex = 0;
    private int inputIndex = 0;
    private int longNoteIndex = 0;

    // The current time in the song
    private double audioTime;

    [SerializeField] float enemyTime;

    public static Action<Vector3> NoteSpawned;
    public static Action HitPerfect;
    public static Action HitGood;
    public static Action HitMiss;

    public static Action AnimateAllTopLanes;
    public static Action AnimateAllMiddleLanes;
    public static Action AnimateAllBottomLanes;

    private bool noteSpawnCooldownComplete = true;


    // Subscribe to static actions
    void OnEnable()
    {
        PlayerInput.OnSlashPeak += DetermineSlashAccuracy;
    }

    // Unsubscribe from static actions
    void OnDisable()
    {
        PlayerInput.OnSlashPeak -= DetermineSlashAccuracy;
    }

    // Initialize the object pool, overridden from singleton (replaces Monobehavior.Awake())
    protected override void Init()
    {
        _notePool = new ObjectPool<Note>(CreateNote,
        note => {
            note.gameObject.SetActive(true);
        }, note => {
            note.gameObject.SetActive(false);
        }, note => {
            Destroy(note.gameObject);
        }, false,
        20,
        60
        );

        _longNotePool = new ObjectPool<LongNote>(CreateLongNote,
        note => {
            note.gameObject.SetActive(true);
        }, note => {
            note.gameObject.SetActive(false);
        }, note => {
            Destroy(note.gameObject);
        }, false,
        10,
        20
        );

    }

    // Called before the first frame of Update()
    private void Start()
    {
        CreateNote(10);
        CreateLongNote(5);
    }

    // Overloaded method for instantiating each individual note
    private Note CreateNote()
    {
        Note note = Instantiate(_notePrefab, transform.position + Vector3.right * SongManager.Instance.noteSpawnX * 2, Quaternion.identity, transform);
        return note;
    }

    // Overloaded method for preallocating note objects in the pool
    private void CreateNote(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Note note = _notePool.Get();
            note.gameObject.SetActive(false);
        }
    }

    // Overloaded method for instantiating each individual long note
    private LongNote CreateLongNote()
    {
        LongNote note = Instantiate(_longNotePrefab, transform.position + Vector3.right * SongManager.Instance.noteSpawnX * 2, Quaternion.identity, transform);
        return note;
    }

    // Overloaded method for preallocating long note objects in the pool
    private void CreateLongNote(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var note = _longNotePool.Get();
            note.gameObject.SetActive(false);
        }
    }

    // Initialize the time stamps in the song
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
            var metricEndTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.EndTime, SongManager.midiFile.GetTempoMap());

            bool longNote = note.Length > 64;

            // if ((int)note.Length % 64 != 0)
            // {
            //     Debug.LogError("Note duration is not a multiple of 64");
            //     continue;
            // }

            float noteCount = 1; //= note.Length / 64;
            if (longNote)
                noteCount = 2;

            // Based on the note type in the MIDI file, determine which lane it should go in (0 = top, 1 = mid, 2 = bottom, 3 = some fourth lane)
            switch (note.NoteName)
            {
                // bottom lane
                case Melanchall.DryWetMidi.MusicTheory.NoteName.A:

                    for (int i = 0; i < noteCount; i++)
                    {
                        bool isHeldNote = i > 0;
                        heldNotes.Add(isHeldNote);
                        timeYIndex.Add(0);
                        if (!isHeldNote)
                            timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + ((double)metricTimeSpan.Milliseconds) / 1000f);
                        else
                            timeStamps.Add((double)metricEndTimeSpan.Minutes * 60f + metricEndTimeSpan.Seconds + ((double)metricEndTimeSpan.Milliseconds) / 1000f);
                    }
                    break;

                // middle lane
                case Melanchall.DryWetMidi.MusicTheory.NoteName.G:
                    for (int i = 0; i < noteCount; i++)
                    {
                        bool isHeldNote = i > 0;
                        heldNotes.Add(isHeldNote);
                        timeYIndex.Add(1);
                        if (!isHeldNote)
                            timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + ((double)metricTimeSpan.Milliseconds) / 1000f);
                        else
                            timeStamps.Add((double)metricEndTimeSpan.Minutes * 60f + metricEndTimeSpan.Seconds + ((double)metricEndTimeSpan.Milliseconds) / 1000f);
                    }
                    break;

                // top lane
                case Melanchall.DryWetMidi.MusicTheory.NoteName.F:
                    for (int i = 0; i < noteCount; i++)
                    {
                        bool isHeldNote = i > 0;
                        heldNotes.Add(isHeldNote);
                        timeYIndex.Add(2);
                        if (!isHeldNote)
                            timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + ((double)metricTimeSpan.Milliseconds) / 1000f);
                        else
                            timeStamps.Add((double)metricEndTimeSpan.Minutes * 60f + metricEndTimeSpan.Seconds + ((double)metricEndTimeSpan.Milliseconds) / 1000f);
                    }
                    break;
                case Melanchall.DryWetMidi.MusicTheory.NoteName.E:
                    // popup times
                    break;
                case Melanchall.DryWetMidi.MusicTheory.NoteName.D:
                    // camera angles
                    break;
                default:
                    Debug.LogError($"Note name {note.NoteName} not accounted for when setting timeStamps.");
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Add a new note to the scene when it's appropriate in the song
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                float yPos = ScreenManager.Instance.InsideLanesYPositions[timeYIndex[spawnIndex]];
                Vector3 spawnPos = Vector3.up * yPos;

                Note note = _notePool.Get();
                note.Init(yPos);
                notes.Add(note);
                
                note.assignedTime = (float)timeStamps[spawnIndex];

                // SPAWN LONG NOTE
                if (heldNotes[spawnIndex + 1])
                {
                    LongNote longNote = _longNotePool.Get();
                    // Instantiate(_longNotePrefab, new Vector3(SongManager.Instance.noteSpawnX, yPos, 0), Quaternion.identity) as LongNote;
                    longNote.Init(yPos, (float)timeStamps[spawnIndex + 1]);
                    longNote.assignedTime = (float)timeStamps[spawnIndex];

                    longNotes.Add(longNote);
                }

                spawnIndex++;
            }
            else if (noteSpawnCooldownComplete && SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime - enemyTime)
            {
                StartCoroutine(NoteSpawnCooldown(0.1f));
                float yPos = ScreenManager.Instance.InsideLanesYPositions[timeYIndex[spawnIndex]];
                NoteSpawned?.Invoke(new Vector3(SongManager.Instance.noteSpawnX, yPos, 0));

            }
        }

        // Remove a note from the scene when the note goes too far or is completely missed by the player
        if (inputIndex < timeStamps.Count)
        {
            audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);
            double timeStamp = timeStamps[inputIndex];
            double marginOfErrorX = SongManager.Instance.marginOfErrorX;
            
            if (timeStamp + marginOfErrorX <= audioTime)
            {
                // Release the note
                notes[inputIndex].ReleaseNote();
                int tempIndex = inputIndex;
                // Focus the next note
                inputIndex++;

                // Only truly miss the note and affect health if the note isn't a held note
                if (!heldNotes[tempIndex])
                {
                    Miss();
                    AudioManager.Instance.PlaySFX("MissNote");
                    print("Miss (did not attempt to hit)");

                    // Grey out the held note if the first one is missed
                    bool nextNoteIsHeldNote = heldNotes[tempIndex + 1];
                    if (nextNoteIsHeldNote)
                    {
                        notes[tempIndex + 1].GreyOutNote();
                        longNotes[longNoteIndex].GreyOutNote();
                        longNoteIndex++;
                    }
                }
                // else
                // {
                //     longNotes[longNoteIndex].ReleaseNote();
                //     longNoteIndex++;
                // }                
            }
        }       
    }

    IEnumerator NoteSpawnCooldown(float cooldownTime)
    {
        noteSpawnCooldownComplete = false;
        float currentTime = 0;

        while (currentTime < cooldownTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        noteSpawnCooldownComplete = true;
    }

    // Determine how accurate each note is when the player slashes their sword
    private void DetermineSlashAccuracy()
    {
        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfErrorX = SongManager.Instance.marginOfErrorX;
            float marginOfErrorY = SongManager.Instance.marginOfErrorY;
            audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);
            Vector2 playerPosition = PlayerInput.Instance.PlayerPosition;

            // Calculate the difference between the player and the note
            double xPosDifference = Math.Abs(audioTime - timeStamp);
            float yPosDifference = Math.Abs(ScreenManager.Instance.InsideLanesYPositions[timeYIndex[inputIndex]] - playerPosition.y - 1.07f); // -1 to account for the sword's vertical offset
            bool isHeldNote = heldNotes[inputIndex];
            // Debug that helps with determine how off the player's swing was compared to the margins of error
            // print($"lane: {gameObject.name} xPosDiff: {xPosDifference} yPosDiff: {yPosDifference}");

            // If the player's swing is perfect (within 1/4 margin of error)
            if (xPosDifference < marginOfErrorX / 4f && yPosDifference < marginOfErrorY && !PlayerInput.Instance.IsSpinSlashing && !isHeldNote)
            {
                PerfectHit();
                // print($"Perfect Hit on {inputIndex} note");
                print("Perfect");
                noteDeletionIndexQueue.Enqueue(inputIndex);
                
                AudioManager.Instance.PlaySFX("HitNotePerfect");

                if (heldNotes[inputIndex + 1])
                {
                    StartCoroutine(WaitForSpinSlash(0.5f));
                    longNotes[longNoteIndex].StartDecrease();
                }

                AnimateObjects(timeYIndex[inputIndex]);
                
            }
            // If the player's swing is good (within the margin of error)
            else if (xPosDifference < marginOfErrorX && yPosDifference < marginOfErrorY && !PlayerInput.Instance.IsSpinSlashing && !isHeldNote)
            {
                GoodHit();
                // print($"Good Hit on {inputIndex} note");
                print("Good");
                
                noteDeletionIndexQueue.Enqueue(inputIndex);
                
                AudioManager.Instance.PlaySFX("HitNoteGood");

                if (heldNotes[inputIndex + 1])
                {
                    StartCoroutine(WaitForSpinSlash(0.5f));
                    longNotes[longNoteIndex].StartDecrease();
                }

                AnimateObjects(timeYIndex[inputIndex]);
            }
            // If the player's swing missed (could be used to release the note)
            else
            {
                
                //print($"Miss on {gameObject.name} lane with {Math.Abs(audioTime - timeStamp)} delay");
                Miss();
                // notes[inputIndex].GreyOutNote();
                // noteDeletionIndexQueue.Enqueue(inputIndex);
                print("Miss (bad timing)");
                AudioManager.Instance.PlaySFX("MissNote");
                
            }

            // Release all the notes in the deletion queue
            while (noteDeletionIndexQueue.Count > 0)
            {
                Note noteToRelease = notes[noteDeletionIndexQueue.Dequeue()];
                noteToRelease.ReleaseNote();
                inputIndex++;
            }
        }       
    }

    IEnumerator WaitForSpinSlash(float waitTime)
    {
        float curTime = 0;

        while (curTime < waitTime)
        {
            curTime += Time.deltaTime;
            if (PlayerInput.Instance.IsSpinSlashing) break;
            yield return null;
        }
        //print("got to here!");
        if (PlayerInput.Instance.IsSpinSlashing)
            StartCoroutine(WaitForHeldNoteOnBeat());
        else
        {
            print("failed to press spinslash");
            Miss();
            AudioManager.Instance.PlaySFX("MissNote");
            notes[inputIndex].GreyOutNote();
            longNotes[longNoteIndex].GreyOutNote();
            longNotes[longNoteIndex].EndDecrease();
            longNoteIndex++;
        }
    }

    IEnumerator WaitForHeldNoteOnBeat()
    {
        audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);
        double timeStamp = timeStamps[inputIndex];
        bool fullSpinSlash = false;
        while (audioTime <= timeStamp)
        {
            //print($"{audioTime} + {timeStamp}");
            audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);
            timeStamp = timeStamps[inputIndex];
            
            float marginOfErrorY = SongManager.Instance.marginOfErrorY;
            Vector2 playerPosition = PlayerInput.Instance.PlayerPosition;
            float yPosDifference = Math.Abs(ScreenManager.Instance.InsideLanesYPositions[timeYIndex[inputIndex]] - playerPosition.y - 1.07f); // -1 to account for the sword's vertical offset

            fullSpinSlash = PlayerInput.Instance.IsSpinSlashing && yPosDifference < marginOfErrorY;
            if (!fullSpinSlash) break;
            yield return null;
        }
        if (fullSpinSlash)
            RemoveHeldNoteOnBeat();
        else
        {
            print("failed to finish spin slash");
            Miss();
            AudioManager.Instance.PlaySFX("MissNote");
            notes[inputIndex].GreyOutNote();
            longNotes[longNoteIndex].GreyOutNote();
            longNotes[longNoteIndex].EndDecrease();
            longNoteIndex++;
        }
    }

    private void RemoveHeldNoteOnBeat()
    {
        if (heldNotes[inputIndex])
        {
            PerfectHit();
            AudioManager.Instance.PlaySFX("HitNotePerfect");
            print("Perfect");
            print("succeeded spin slash");
            AnimateObjects(timeYIndex[inputIndex]);
            notes[inputIndex].ReleaseNote();
            //notes[inputIndex].GreyOutNote();
            longNotes[longNoteIndex].ReleaseNote();
            longNoteIndex++;
            inputIndex++;
        }
    }

    // Functions that communicate with other scripts for each note accuracy
    private void PerfectHit()
    {
        ScoreManager.PerfectHit();
        HitPerfect?.Invoke();
    }
    private void GoodHit()
    {
        ScoreManager.GoodHit();
        HitGood?.Invoke();
    }
    private void Miss()
    {
        //CameraShaker.Instance.ShakeOnce(0.1f, 0.2f, 0.1f, 0.1f);
        ScoreManager.Miss();
        HitMiss?.Invoke();
    }

    private void AnimateObjects(int yPos)
    {
        if (yPos == 0)
            AnimateAllBottomLanes?.Invoke();
        else if (yPos == 1)
            AnimateAllMiddleLanes?.Invoke();
        else if (yPos == 2)
            AnimateAllTopLanes?.Invoke();
    }
}