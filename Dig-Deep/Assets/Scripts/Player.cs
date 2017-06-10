﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    
    // Player Selection
    public int player;
    
    //*** Spin Input
    public float horizontalRaw;
    public float verticalRaw;
    float angleRaw;

    bool up, right, down, left;
    float spinTime;

    public float spinsPerSecond;
    [SerializeField]
    public Queue<int> spinSpeedSaves;


    [SerializeField]
    Canvas canvas;

    [SerializeField]
    Image defaultImage;

    [SerializeField]
    Sprite[] ButtonSprites;

    [SerializeField]
    float spawnIntervalBase;
    [SerializeField]
    float travelspeedBase;

    private float timeSinceLastSpawn = 0;

    private List<Image> currentActiveButtons;
    private List<Image> clickedButtons;//have already been clicked, will only be moved out and destroyed

    public int correctClicked = 0;
    public int falseClicked = 0;
    //***

    // Use this for initialization
    void Start ()
    {
        currentActiveButtons = new List<Image>();
        clickedButtons = new List<Image>();

        spinSpeedSaves = new Queue<int>();

        for(int i=0; i<60; i++)
        {
            spinSpeedSaves.Enqueue(0);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Button Spawning
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnIntervalBase * (1/spinsPerSecond))
        {
            timeSinceLastSpawn = 0;

            Image newButton = Image.Instantiate(defaultImage);
            newButton.transform.SetParent(canvas.transform, false);
            newButton.transform.SetAsFirstSibling();
            newButton.sprite = ButtonSprites[Random.Range(0, ButtonSprites.Length)];
            currentActiveButtons.Add(newButton);
        }

        //Button movement
        foreach (Image button in currentActiveButtons)
        {
            button.transform.position = button.transform.position + Vector3.left * travelspeedBase*spinsPerSecond * Time.deltaTime;
        }
        foreach (Image button in clickedButtons)
        {
            button.transform.position = button.transform.position + Vector3.left * travelspeedBase*spinsPerSecond * Time.deltaTime;
        }

        //Button Destroying
        if (currentActiveButtons.Count > 0 && currentActiveButtons[0].transform.position.x < 0)
        {
            Destroy(currentActiveButtons[0].gameObject);
            currentActiveButtons.RemoveAt(0);
        }
        if (clickedButtons.Count > 0 && clickedButtons[0].transform.position.x < 0)
        {
            Destroy(clickedButtons[0].gameObject);
            clickedButtons.RemoveAt(0);
        }

        //Button Press Handling
        //Get current Button to press
        int currentButton = -1;
        int currentInput = -1;

        //Inout abfragen
        if (Input.GetKeyDown("joystick " + player + " button 0"))
        {
            currentInput = 0;
        }
        else if (Input.GetKeyDown("joystick " + player + " button 1"))
        {
            currentInput = 1;
        }
        else if (Input.GetKeyDown("joystick " + player + " button 2"))
        {
            currentInput = 2;
        }
        else if (Input.GetKeyDown("joystick " + player + " button 3"))
        {
            currentInput = 3;
        }

        foreach (Image button in currentActiveButtons)
        {

            if (button.transform.position.x > 75 && button.transform.position.x < 125)
            {
                //Button befindet sich über dem Strich
                if (button.sprite.Equals(ButtonSprites[0]))
                {
                    currentButton = 0;
                }
                else if (button.sprite.Equals(ButtonSprites[1]))
                {
                    currentButton = 1;
                }
                else if (button.sprite.Equals(ButtonSprites[2]))
                {
                    currentButton = 2;
                }
                else if (button.sprite.Equals(ButtonSprites[3]))
                {
                    currentButton = 3;
                }

                if (currentInput == currentButton)
                {
                    //Handle correct pressed button logic
                    correctClicked++;

                    //move button to already clicked so it doesn't get clicked again
                    currentActiveButtons.Remove(button);
                    clickedButtons.Add(button);
                }

                break;
            }
        }

        //Detect false button presses
        if (currentInput >= 0 && currentInput != currentButton)
        {
            falseClicked++;
        }

    }

    void FixedUpdate()
    {
        SpinInput();
    }


    // Calculates Everything For Spin Input
    public void SpinInput()
    {
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
        if (up && right && down && left)
        {
            Debug.Log("Full Cycle...");
            spinSpeedSaves.Enqueue ( 1 );
            spinSpeedSaves.Dequeue();
            up = right = down = left = false;
        }
        else
        {
            spinSpeedSaves.Enqueue(0);
            spinSpeedSaves.Dequeue();
        }

        // Cap Spinspeed to [0,inf)
        spinsPerSecond = 0;
        foreach(int speed in spinSpeedSaves)
        {
            spinsPerSecond += speed;
        }
    }
}
