﻿using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    //public KeyCode input;
    public GameObject notePrefab;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int inputIndex = 0;

    bool enoughTimeSinceLastInput = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
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
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            float marginOfErrorY = SongManager.Instance.marginOfErrorY;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetMouseButtonDown(0) && enoughTimeSinceLastInput)
            {
                StartCoroutine(ClickCooldown(1.0f));
                Vector2 playerPosition = PlayerInput.Instance.PlayerPosition;
                double xPosDifference = Math.Abs(audioTime - timeStamp);
                float yPosDifference = Math.Abs(transform.position.y - playerPosition.y - 1.07f); // -1 to account for the sword's vertical offset

                print($"lane: {gameObject.name} xPosDiff: {xPosDifference} yPosDiff: {yPosDifference}");

                if (xPosDifference < marginOfError / 2f && yPosDifference < marginOfErrorY)
                {
                    PerfectHit();
                    //print($"Perfect Hit on {inputIndex} note");
                    print("Perfect");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else if (xPosDifference < marginOfError && yPosDifference < marginOfErrorY)
                {
                    GoodHit();
                    print("Good");
                    //print($"Good Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    //print($"Miss on {gameObject.name} lane with {Math.Abs(audioTime - timeStamp)} delay");
                    print("Miss (bad timing)");
                    //Destroy(notes[inputIndex].gameObject);
                }
            }

            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                //print($"Missed {inputIndex} note");
                print("Miss (did not attempt to hit)");
                Destroy(notes[inputIndex].gameObject);
                inputIndex++;
            }
        }       
    }

    private IEnumerator ClickCooldown(float cooldownTime)
    {
        enoughTimeSinceLastInput = false;
        yield return new WaitForSeconds(cooldownTime);
        enoughTimeSinceLastInput = true;
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