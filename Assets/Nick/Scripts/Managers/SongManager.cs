using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : Singleton<SongManager>
{
    [SerializeField] private List<AudioClip> _tracks;
    [SerializeField] private List<float> _BPM;

    private float _currentBPM;
    public float CurrentSongBPM
    {
        get { return _currentBPM; }
    }

    private AudioSource _audioSource;

    private void Start()
    {
        if (_tracks.Count != _BPM.Count)
        {
            Debug.LogError("Track list count does not match BPM list count.");
            return;
        }

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _tracks[0];
        _audioSource.Play();

        _currentBPM = _BPM[0];
        InvokeRepeating(nameof(MoveNotesHalf), 1f, 60f/_currentBPM);
    }

    private void MoveNotesHalf()
    {
        ActionManager.Instance.MoveNotesHalf();
    }
}
