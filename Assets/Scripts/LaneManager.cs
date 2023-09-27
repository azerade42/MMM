﻿using Melanchall.DryWetMidi.Interaction;
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
    // The list of all notes in the song
    List<Note> notes = new List<Note>();
    // The times in the song where the notes are positioned
    [HideInInspector] public List<double> timeStamps = new List<double>();
    // The Y positions for each note that correlate with each time stamp
    [HideInInspector] public List<int> timeYIndex = new List<int>();
    // The queue created for releasing notes from the note pool
    private Queue<int> noteDeletionIndexQueue = new Queue<int>();
    // The object pool created for preallocating note objects to replace instantiation/destroy calls at runtime
    public ObjectPool<Note> _notePool;

    // Indexes for tracking the current note in the song
    private int spawnIndex = 0;
    private int inputIndex = 0;

    // The current time in the song
    private double audioTime;

    [SerializeField] float enemyTime;

    public static Action<Vector3> NoteSpawned;
    public static Action HitPerfect;
    public static Action HitGood;
    public static Action HitMiss;

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
        10,
        20
        );

    }

    // Called before the first frame of Update()
    private void Start()
    {
        CreateNote(5);
    }

    // Overloaded method for instantiating each individual note
    private Note CreateNote()
    {
        Note note = Instantiate(_notePrefab, transform.position + Vector3.right * SongManager.Instance.noteSpawnX * 2, Quaternion.identity, transform);
        note.gameObject.SetActive(false);
        return note;
    }

    // Overloaded method for preallocating note objects in the pool
    private void CreateNote(int amount)
    {
        for (int i = 0; i < amount; i++)
            _notePool.Get();
    }

    // Initialize the time stamps in the song
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
            timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);

            // Based on the note type in the MIDI file, determine which lane it should go in (0 = top, 1 = mid, 2 = bottom, 3 = some fourth lane)
            switch (note.NoteName)
            { 
                case Melanchall.DryWetMidi.MusicTheory.NoteName.A:
                    timeYIndex.Add(0);
                    break;
                case Melanchall.DryWetMidi.MusicTheory.NoteName.G:
                    timeYIndex.Add(1);
                    break;
                case Melanchall.DryWetMidi.MusicTheory.NoteName.F:
                    timeYIndex.Add(2);
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
                Miss();
                //print($"Missed {inputIndex} note");
                print("Miss (did not attempt to hit)");
                // _notePool.Release(notes[inputIndex]);
                notes[inputIndex].ReleaseNote();
                //Destroy(notes[inputIndex].gameObject);
                inputIndex++;
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

            // Debug that helps with determine how off the player's swing was compared to the margins of error
            // print($"lane: {gameObject.name} xPosDiff: {xPosDifference} yPosDiff: {yPosDifference}");

            // If the player's swing is perfect
            if (xPosDifference < marginOfErrorX / 4f && yPosDifference < marginOfErrorY)
            {
                PerfectHit();
                // print($"Perfect Hit on {inputIndex} note");
                print("Perfect");
                noteDeletionIndexQueue.Enqueue(inputIndex);
                
            }
            // If the player's swing is good (within the margin of error)
            else if (xPosDifference < marginOfErrorX && yPosDifference < marginOfErrorY)
            {
                GoodHit();
                print("Good");
                // print($"Good Hit on {inputIndex} note");
                
                noteDeletionIndexQueue.Enqueue(inputIndex);
            }
            // If the player's swing missed (could be used to release the note)
            else
            {
                //print($"Miss on {gameObject.name} lane with {Math.Abs(audioTime - timeStamp)} delay");
                Miss();
                print("Miss (bad timing)");
                //Destroy(notes[inputIndex].gameObject);
            }

            // Release the first note in the deletion queue
            if (noteDeletionIndexQueue.Count > 0)
            {
                Note noteToRelease = notes[noteDeletionIndexQueue.Dequeue()];
                noteToRelease.ReleaseNote();
                inputIndex++;
            }
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
        ScoreManager.Miss();
        HitMiss?.Invoke();
    }
}