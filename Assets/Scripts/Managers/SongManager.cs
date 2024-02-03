﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class SongManager : Singleton<SongManager>
{
    private AudioSource audioSource;
    [Tooltip("How long to delay the song before it starts")]
    [SerializeField] private float songDelayInSeconds;
    [Tooltip("Margin of Error for notes (X in seconds)")]
    public double marginOfErrorX;
    [Tooltip("Margin of Error for notes (y in distance)")]
    public float marginOfErrorY;

    public int inputDelayInMilliseconds;

    [Tooltip("File path for the MIDI file (in streaming assets)")]
    public string fileLocation;
    [Tooltip("How long it takes the note to get to its destination (in seconds)")]
    public float noteTime;

    [Tooltip("X Position where the note spawns")]
    public float noteSpawnX;
    [Tooltip("X Position where the note gets hit")]
    public float noteTapX;
    public float noteDespawnX
    {
        get
        {
            return noteTapX - (noteSpawnX - noteTapX);
        }
    }

    [SerializeField] UnityEngine.Video.VideoPlayer video;

    public static bool songPlaying;

    public static MidiFile midiFile;

    public static Action SongStarted;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = AudioManager.Instance.musicSource;
        audioSource.Stop();
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            // StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }

    // private IEnumerator ReadFromWebsite()
    // {
    //     using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
    //     {
    //         yield return www.SendWebRequest();

    //         if (www.isNetworkError || www.isHttpError)
    //         {
    //             Debug.LogError(www.error);
    //         }
    //         else
    //         {
    //             byte[] results = www.downloadHandler.data;
    //             using (var stream = new MemoryStream(results))
    //             {
    //                 midiFile = MidiFile.Read(stream);
    //                 GetDataFromMidi();
    //             }
    //         }
    //     }
    // }

    // Reads the MIDI file from the project
    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    // Gets the time stamps from the MIDI file and starts the song
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        LaneManager.Instance.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void StartSong()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1: 
                AudioManager.Instance.PlayMusic("Level1Music");
                break;
            case 2:
                AudioManager.Instance.PlayMusic("Level2Music");
                break;
            case 3:
                AudioManager.Instance.PlayMusic("Level3Music");
                break;
        }

        songPlaying = true;
        SongStarted?.Invoke();

        if (video != null)
            video.Play();
    }

    // Get the current time in the AudioSource
    public static double GetAudioSourceTime()
    {
        if (Instance.audioSource == null) return 0;
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
}
