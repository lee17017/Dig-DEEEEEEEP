using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary> This singleton holds the relevant settings for running the actual game. </summary>
public class GameManager : MonoBehaviour
{
    #region Variables
    public AudioClip stunClip;

    public static GameManager current;

    public Canvas canvas;

    [FormerlySerializedAs("ButtonSprites")]
    public Sprite[] buttonSprites;

    public GameObject[] plyers;

    public int playerScore1;
    public int playerScore2;
    public int winnerScore;

    [FormerlySerializedAs("Player1EasterEgg")]
    public bool p1EasterEgg;
    [FormerlySerializedAs("Player2EasterEgg")]
    public bool p2EasterEgg;

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
    [FormerlySerializedAs("FehlerAnzahl")]
    public int maxErrors;

    public float speedFeedback;
    public int speedFeedbackResponseTime;

    public int baseSpeed;
    public float speedEffect;

    // Obstacles
    public int[] hitsNeeded;
    public int[] buttonNeeded;

    // Lanes
    public float laneDistance;
    #endregion

    void Awake()
    {
        current = this;

        for (int i = 0; i < 1000; i++)
        {
            sequence.Add(Random.Range(0, 4));
        }

        DontDestroyOnLoad(this);
        gameDuration = totalDuration;
        StartCoroutine(GameTimer());
    }

    private void Update()
    {
        clockPercentage = Mathf.Abs(gameDuration / totalDuration);
    }

    /// <summary> Updates the timer value. </summary>
    IEnumerator GameTimer()
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
    
    /// <summary> Activates the keyboard controls. </summary>
    public void SetKBActive()
    {
        tastatur = !tastatur;
    }
}