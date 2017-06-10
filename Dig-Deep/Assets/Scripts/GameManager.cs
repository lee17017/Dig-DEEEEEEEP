using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager current;
       
    public Canvas canvas;
    
    public Sprite[] ButtonSprites;

    public List<int> sequence;

    //public float spawnIntervalBase;

    //Spawn and speed controls
    public float secondsSmoothed;
    public float powerSpins;
    public float travelspeedBase;
    public float distance;
    public float speedMultiplier;

    //Anzahl erlaubter Fehler bevor man gestunned wird
    public int FehlerAnzahl;
    
    void Awake()
    {
        current = this;

        for(int i = 0; i < 1000; i++)
        {
            sequence.Add(Random.Range(0, 4));
        }
    }

    // Use this for initialization
    void Start () {
	}
}
