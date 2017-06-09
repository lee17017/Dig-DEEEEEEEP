using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour {
    private int score = 0;
    private int multiplyer = 1;
    public float speed = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Pro Frame oder pro Sekunde? Frames sind hier eigentlich besser
        score += (int)((speed) * multiplyer);
        Debug.Log(score);

        if (Input.GetButtonDown("Fire1"))
        {
            speed++;
        }
	}

    public void setMultiplyer(int materialID)
    {
        //Setze Multiplyer abhängig von Material
    }

}
