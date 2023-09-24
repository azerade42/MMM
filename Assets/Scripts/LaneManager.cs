﻿using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class LaneManager : Singleton<LaneManager>
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    //public KeyCode input;
    public Note _notePrefab;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    public List<int> timeYIndex = new List<int>();
    Queue<int> noteDeletionIndexQueue = new Queue<int>();

    public ObjectPool<Note> _notePool;

    int spawnIndex = 0;
    int inputIndex = 0;


    //private bool slashInputPressed;
    //private bool noteQueuedToDelete;

    private double audioTime;


    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerInput.OnSlashPeak += DetermineSlashAccuracy;
    }

    void OnDisable()
    {
        PlayerInput.OnSlashPeak -= DetermineSlashAccuracy;
    }

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
        25,
        50
        );

    }

    private void Start()
    {
        CreateNote(24);
    }

    private Note CreateNote()
    {
        return Instantiate(_notePrefab, transform.position + Vector3.right * 20f, Quaternion.identity, transform);
    }

    private void CreateNote(int amount)
    {
        for (int i = 0; i < amount; i++)
            _notePool.Get();
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
            timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);

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
        }

        if (inputIndex < timeStamps.Count)
        {
            audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            // float marginOfErrorY = SongManager.Instance.marginOfErrorY;

            // if (slashInputPressed)
            // {
            //     slashInputPressed = false;
            //     Vector2 playerPosition = PlayerInput.Instance.PlayerPosition;
            //     double xPosDifference = Math.Abs(audioTime - timeStamp);
            //     float yPosDifference = Math.Abs(ScreenManager.Instance.InsideLanesYPositions[timeYIndex[spawnIndex]] - playerPosition.y - 1.07f); // -1 to account for the sword's vertical offset

            //     //print($"lane: {gameObject.name} xPosDiff: {xPosDifference} yPosDiff: {yPosDifference}");

            //     if (xPosDifference < marginOfError / 4f && yPosDifference < marginOfErrorY)
            //     {
            //         PerfectHit();
            //         print($"Perfect Hit on {inputIndex} note");
            //         // print("Perfect");
            //         noteDeletionIndexQueue.Enqueue(inputIndex);
                    
            //     }
            //     else if (xPosDifference < marginOfError && yPosDifference < marginOfErrorY)
            //     {
            //         GoodHit();
            //         // print("Good");
            //         print($"Good Hit on {inputIndex} note");
                    
            //         noteDeletionIndexQueue.Enqueue(inputIndex);
            //     }
            //     else
            //     {
            //         //print($"Miss on {gameObject.name} lane with {Math.Abs(audioTime - timeStamp)} delay");
            //         // print("Miss (bad timing)");
            //         //Destroy(notes[inputIndex].gameObject);
            //     }
            // }

            if (timeStamp + marginOfError <= audioTime)
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

    // private void ProcessSlash() => slashInputPressed = true;

    private void DetermineSlashAccuracy()
    {
        print(timeStamps.Count + " " + timeYIndex.Count);
        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            float marginOfErrorY = SongManager.Instance.marginOfErrorY;
            audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            // if (slashInputPressed)
            // {
            //     slashInputPressed = false;
            Vector2 playerPosition = PlayerInput.Instance.PlayerPosition;
            double xPosDifference = Math.Abs(audioTime - timeStamp);
            float yPosDifference = Math.Abs(ScreenManager.Instance.InsideLanesYPositions[timeYIndex[inputIndex]] - playerPosition.y - 1.07f); // -1 to account for the sword's vertical offset

            print($"lane: {gameObject.name} xPosDiff: {xPosDifference} yPosDiff: {yPosDifference}");

            if (xPosDifference < marginOfError / 4f && yPosDifference < marginOfErrorY)
            {
                PerfectHit();
                // print($"Perfect Hit on {inputIndex} note");
                print("Perfect");
                noteDeletionIndexQueue.Enqueue(inputIndex);
                
            }
            else if (xPosDifference < marginOfError && yPosDifference < marginOfErrorY)
            {
                GoodHit();
                print("Good");
                // print($"Good Hit on {inputIndex} note");
                
                noteDeletionIndexQueue.Enqueue(inputIndex);
            }
            else
            {
                //print($"Miss on {gameObject.name} lane with {Math.Abs(audioTime - timeStamp)} delay");
                print("Miss (bad timing)");
                //Destroy(notes[inputIndex].gameObject);
            }
            

            

            if (noteDeletionIndexQueue.Count > 0)
            {
                Note noteToRelease = notes[noteDeletionIndexQueue.Dequeue()];
                noteToRelease.ReleaseNote();
                inputIndex++;
            }
        }       
    }


    private void PerfectHit()
    {
        ScoreManager.PerfectHit();
    }
    private void GoodHit()
    {
        ScoreManager.GoodHit();
    }
    private void Miss()
    {
        ScoreManager.Miss();
    }
}