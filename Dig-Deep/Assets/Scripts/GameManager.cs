using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float clockPercentage;

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

    private void Update()
    {
        clockPercentage = Mathf.Abs(gameDuration / totalDuration);
    }

    IEnumerator gameTime()
    {
        while(gameDuration > 0)        {
            
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                run = true;
                gameDuration = totalDuration;
            }

            gameDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        run = false;

        yield return new WaitForSeconds(5);

        /*
         run your you lose you win part here @Liou
         
         */

        SceneManager.LoadScene(2);

        
    }

}
