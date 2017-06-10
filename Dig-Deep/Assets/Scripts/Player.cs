using System.Collections;
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
    [SerializeField]
    float distance;

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

        for(int i=0; i<240; i++)
        {
            spinSpeedSaves.Enqueue(0);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        ButtonMovement();
        ButtonHandling();
    }

    void FixedUpdate()
    {
        SpinInput();
    }

    //Handles Button Spawning, movement and destruction
    public void ButtonMovement()
    {
        //Calculates Movement speed and spawn rate
        float Speed = Mathf.Max(travelspeedBase * spinsPerSecond * 0.15f, travelspeedBase) * Time.deltaTime;
        float SpawnTime = distance / Speed * Time.deltaTime;

        //Button Spawning
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= SpawnTime)
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
            button.transform.position = button.transform.position + Vector3.left * Speed;
        }
        foreach (Image button in clickedButtons)
        {
            button.transform.position = button.transform.position + Vector3.left * Speed;
        }

        //Button Destroying
        if (currentActiveButtons.Count > 0 && currentActiveButtons[0].transform.position.x < 0)
        {
            falseClicked++;
            Destroy(currentActiveButtons[0].gameObject);
            currentActiveButtons.RemoveAt(0);
        }
        if (clickedButtons.Count > 0 && clickedButtons[0].transform.position.x < 0)
        {
            falseClicked++;
            Destroy(clickedButtons[0].gameObject);
            clickedButtons.RemoveAt(0);
        }

    }

    //Handles Button Presses and Detection
    public void ButtonHandling()
    {

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
                    //clickedButtons.Add(button);
                    DestroyImmediate(button.gameObject);
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
