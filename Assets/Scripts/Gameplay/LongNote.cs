﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    float assignedYPos;

    SpriteRenderer _spriteRenderer;

    private float xOffset = 0.5f;

    //bool gameOver;

    float nextNotePositionX;

    private bool decreasing;
    private bool decreasedWhileActive;
    Vector2 startDecreasingSize;

    private double startDecreasingTime;

    private double endDecreasingTime;

    private float endDecreaseXPos;

    bool noteFading;


    public void Init(float assignedYPos, float nextNotePositionX)
    {
        this.assignedYPos = assignedYPos;
        this.nextNotePositionX = nextNotePositionX;
    }
    
    void OnEnable()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.color = new Color(255f, 255f, 255f, 0.6f);
        
        ///GameManager.GameOver += () => gameOver = true;
    }

    private void OnDisable()
    {
        //GameManager.GameOver -= () => gameOver = true;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameOver) return;
        if (!SongManager.songPlaying) return;
        GetComponent<SpriteRenderer>().enabled = true;

        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));
        float longNoteSize = (float)(timeSinceInstantiated / (nextNotePositionX - assignedTime));

        
        
        //if (t > 1)
        if (decreasing)
        {
            double timeSinceDecreaseStarted = SongManager.GetAudioSourceTime() - startDecreasingTime;
            float decreaseSize = (float)(timeSinceDecreaseStarted / (nextNotePositionX - assignedTime));
            _spriteRenderer.size = Vector2.Lerp(startDecreasingSize, new Vector2(0f, 20.48f), decreaseSize);
        }
        else
        {
            if (!decreasedWhileActive)
            {
                _spriteRenderer.size = Vector2.Lerp(new Vector2(0f, 20.48f), new Vector2(20.48f * 20.48f * 0.7f * (nextNotePositionX - assignedTime), 20.48f), longNoteSize);
                transform.localPosition = Vector3.Lerp(Vector3.right * (SongManager.Instance.noteSpawnX + xOffset) + Vector3.up * assignedYPos, Vector3.right * (SongManager.Instance.noteDespawnX + xOffset) + Vector3.up * assignedYPos, t);
            }
            else
            {
                if (!noteFading)
                    StartCoroutine(FadeOutNote(2f));
                // double timeSinceDecreaseEnded = SongManager.GetAudioSourceTime() - endDecreasingTime;
                // float s = (float)(timeSinceDecreaseEnded / (SongManager.Instance.noteTime * 2));
                // print(endDecreasingTime + " " + assignedTime);
                // transform.localPosition = Vector3.Lerp(Vector3.right * endDecreaseXPos + Vector3.up * assignedYPos, Vector3.right * SongManager.Instance.noteDespawnX + Vector3.up * assignedYPos, s);
            }
        }
    }

    private IEnumerator FadeOutNote(float fadeTime)
    {
        noteFading = true;
        float currentTime = 0f;

        Color curColor = _spriteRenderer.color;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            _spriteRenderer.color = Color.Lerp(curColor, Color.clear, currentTime / fadeTime);
            yield return null;
        }
        noteFading = false;
    }

    // Reset Position and release Note back into the object pool
    public void ReleaseNote()
    {
        decreasing = false;
        decreasedWhileActive = false;
        _spriteRenderer.size = Vector2.zero;
        transform.localPosition = Vector3.right * SongManager.Instance.noteSpawnX + Vector3.up * assignedYPos;
        LaneManager.Instance._longNotePool.Release(this);
    }

    public void GreyOutNote()
    {
        _spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
    }

    public void StartDecrease()
    {
        //print("start decrease");
        startDecreasingSize = _spriteRenderer.size;
        startDecreasingTime = SongManager.GetAudioSourceTime();
        decreasing = true;
        decreasedWhileActive = true;
    }
    public void EndDecrease()
    {
        decreasing = false;
        endDecreasingTime = SongManager.GetAudioSourceTime();
        endDecreaseXPos = transform.position.x;
    }
}