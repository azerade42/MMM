﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    float assignedYPos;

    public void Init(float assignedYPos)
    {
        this.assignedYPos = assignedYPos;
    }
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

        
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnX + Vector3.up * assignedYPos, Vector3.right * SongManager.Instance.noteDespawnX + Vector3.up * assignedYPos, t); 
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}