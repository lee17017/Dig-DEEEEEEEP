using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    // Player Selection
    public int player;

    //*** Spin Input
    public GameObject speedDisplayBar;

    public float horizontalRaw;
    public float verticalRaw;
    float angleRaw;

    bool up, right, down, left;
    float spinTime;

    public float spinSpeed;
    //***

    //*** Buttons
    public bool a, b, x, y;
    //***

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        SpinInput();
        Buttons();
    }

    // Calculates Everything For Spin Input
    public void SpinInput()
    {
        // Every Frame
        spinTime += Time.deltaTime;

        // Actual Input
        horizontalRaw = Input.GetAxis("Horizontal " + player);
        verticalRaw = Input.GetAxis("Vertical " + player);
        angleRaw = Mathf.Atan2(verticalRaw, horizontalRaw) * 180 / Mathf.PI;
        
        // Only Calculate if Stick is actually being moved
        if (!(horizontalRaw == 0 && verticalRaw == 0))
        {
            if (angleRaw > -20 && angleRaw < 20)
            {
                right = true;
            }
            if (angleRaw > 70 && angleRaw < 110)
            {
                up = true;
            }
            if (angleRaw > 160 || angleRaw < -160)
            {
                left = true;
            }
            if (angleRaw > -110 && angleRaw < -70)
            {
                down = true; 
            }
        }

        // Full Cycle Completed
        if (up && right && down && left || spinTime > 1)
        {
            spinSpeed = 1 / spinTime ;
            up = right = down = left = false;
            spinTime = 0;
        }

        // Cap Spinspeed to [0,inf)
        spinSpeed = Mathf.Max(spinSpeed, 0);

        Vector3 og = speedDisplayBar.transform.localScale;
        speedDisplayBar.transform.localScale = new Vector3(og.x, spinSpeed, og.z);
        speedDisplayBar.transform.position = new Vector3(0, spinSpeed / 2, 0);
    }

    public void Buttons()
    {
        a = Input.GetButton("A " + player);
        b = Input.GetButton("B " + player);
        x = Input.GetButton("X " + player);
        y = Input.GetButton("Y " + player);
    }

    //*** Getter & Setter
    public float getSpinSpeed() { return spinSpeed; }
    //***
}
