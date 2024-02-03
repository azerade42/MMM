﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    float assignedYPos;

    SpriteRenderer _spriteRenderer;

    Color startingColor;

    private Animator enemyAnim;
    bool hit = false;
    Vector3 deathPos;
    //bool gameOver;


    public void Init(float assignedYPos)
    {
        this.assignedYPos = assignedYPos;
        startingColor = GetComponent<SpriteRenderer>().color;
    }
    
    void OnEnable()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        ///GameManager.GameOver += () => gameOver = true;
    }

    private void OnDisable()
    {
        
        _spriteRenderer.color = startingColor;
        //GameManager.GameOver -= () => gameOver = true;
    }

    private void Start()
    {
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameOver) return;
        if (!SongManager.songPlaying) return;

        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

        
        if (t > 1)
        {
            // Destroy(gameObject);
        }
        else
        {
            if(!hit)
            {
                transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnX + Vector3.up * assignedYPos, Vector3.right * SongManager.Instance.noteDespawnX + Vector3.up * assignedYPos, t); 
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else if(hit){
                //transform.localPosition = Vector3.Lerp((Vector3.right * SongManager.Instance.noteTapX)  + Vector3.up * assignedYPos, (Vector3.right * SongManager.Instance.noteDespawnX) + Vector3.up * assignedYPos, t/6);
            }
        }
    }

    // Reset Position and release Note back into the object pool
    public void ReleaseNote()
    {
        hit = true;
        deathPos = transform.localPosition;
        enemyAnim.SetTrigger("Death");
        Invoke("noteDeath", 2f);
    }
    public void noteDeath()
    {
        transform.localPosition = Vector3.right * SongManager.Instance.noteSpawnX + Vector3.up * assignedYPos;
        LaneManager.Instance._notePool.Release(this);
        hit = false;
    }

    public void GreyOutNote()
    {
        _spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
    }
}