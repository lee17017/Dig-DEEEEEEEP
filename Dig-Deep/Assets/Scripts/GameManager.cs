using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

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
    public float powerSpins;
    public float travelspeedBase;
    public float distance;
    public float speedMultiplier;

    public float gameDuration;
    public float totalDuration;
    public bool run = true;

    //Anzahl erlaubter Fehler bevor man gestunned wird
    public int FehlerAnzahl;
    
    void Awake()
    {
        current = this;

        for(int i = 0; i < 1000; i++)
        {
            sequence.Add(Random.Range(0, 4));
        }

        GameObject.DontDestroyOnLoad(this);
        gameDuration = totalDuration;
        StartCoroutine(gameTime());
    }

    // Use this for initialization
    void Start () {
	}

    IEnumerator gameTime()
    {
        while(gameDuration > 0)
        {
            gameDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        run = false;

        
    }

}
