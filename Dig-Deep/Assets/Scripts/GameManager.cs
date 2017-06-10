using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager current;

    public float secondsSmoothed;

   
    public Canvas canvas;
    
    public Sprite[] ButtonSprites;

    //public float spawnIntervalBase;
    public float travelspeedBase;
    public float distance;
    public float speedMultiplier;

    void Awake()
    {
        current = this;
    }

    // Use this for initialization
    void Start () {
	}
}
