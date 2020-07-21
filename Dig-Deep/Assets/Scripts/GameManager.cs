﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioClip stunClip;

    public static GameManager current;

    public Canvas canvas;

    public Sprite[] ButtonSprites;

    public GameObject[] plyers;

    public int playerScore1;
    public int playerScore2;
    public int winnerScore;

    public bool Player1EasterEgg;
    public bool Player2EasterEgg;

    public List<int> sequence;

    //public float spawnIntervalBase;

    //Spawn and speed controls
    public float secondsSmoothed;
    public float clickSecondsSmoothed;
    public float powerSpins;
    public float travelspeedBase;
    public float distance;
    public float speedMultiplier;

    public float gameDuration;
    public float totalDuration;
    public bool run = true;
    public bool tastatur = false;

    public float clockPercentage;

    //Anzahl erlaubter Fehler bevor man gestunned wird
    public int FehlerAnzahl;

    public float speedFeedback;
    public int speedFeedbackResponseTime;

    public int baseSpeed;
    public float speedEffect;

    // Obstacles
    public int[] hitsNeeded;
    public int[] buttonNeeded;

    // Lanes
    public float laneDistance;

    void Awake()
    {
        current = this;

        for (int i = 0; i < 1000; i++)
        {
            sequence.Add(Random.Range(0, 4));
        }

        DontDestroyOnLoad(this);
        gameDuration = totalDuration;
        StartCoroutine(gameTime());
    }

    private void Update()
    {
        clockPercentage = Mathf.Abs(gameDuration / totalDuration);
    }

    IEnumerator gameTime()
    {
        while (gameDuration > 0)
        {

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                run = true;
                gameDuration = totalDuration;
            }

            gameDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        run = false;
    }
    
    public void setKBActive()
    {
        tastatur = !tastatur;
    }
}