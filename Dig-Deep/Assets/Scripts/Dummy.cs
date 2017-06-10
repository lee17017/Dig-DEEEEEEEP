using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {   

	// Use this for initialization
	void Awake () {
        GameManager.current.canvas = this.gameObject.GetComponent<Canvas>();
        GameManager.current.gameDuration = GameManager.current.totalDuration;
	}
	
}
