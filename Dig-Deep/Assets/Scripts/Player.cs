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
    Image defaultImage;

    private float timeSinceLastSpawn = 0;

    private List<Image> currentActiveButtons;

    private int sequencePos = 0;

    public int correctClicked = 0;
    public int falseClicked = 0;
    //***

    // Use this for initialization
    void Start ()
    {
        currentActiveButtons = new List<Image>();

        spinSpeedSaves = new Queue<int>();

        for(int i=0; i< GameManager.current.secondsSmoothed *60; i++)
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
        float Speed = Mathf.Max(GameManager.current.travelspeedBase * Mathf.Pow(spinsPerSecond, GameManager.current.powerSpins) * GameManager.current.speedMultiplier, GameManager.current.travelspeedBase) * Time.deltaTime;
        float SpawnTime = GameManager.current.distance / Speed * Time.deltaTime;

        //Button Spawning
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= SpawnTime)
        {
            timeSinceLastSpawn = 0;

            Image newButton = Image.Instantiate(defaultImage);
            newButton.transform.SetParent(GameManager.current.canvas.transform, false);
            newButton.transform.SetAsLastSibling();
            //newButton.sprite = GameManager.current.ButtonSprites[Random.Range(0, GameManager.current.ButtonSprites.Length)];
            newButton.sprite = GameManager.current.ButtonSprites[GameManager.current.sequence[sequencePos]];
            sequencePos++;
            newButton.color = new Color(1,1,1,1);
            currentActiveButtons.Add(newButton);
        }

        //Button movement
        foreach (Image button in currentActiveButtons)
        {
            button.transform.position = button.transform.position + Vector3.down * Speed;
        }

        //Button Destroying
        if (currentActiveButtons.Count > 0 && currentActiveButtons[0].transform.position.y < 50)
        {
            falseClicked++;
            Destroy(currentActiveButtons[0].gameObject);
            currentActiveButtons.RemoveAt(0);

            if(falseClicked % GameManager.current.FehlerAnzahl == 0)
            {
                StunPlayer();
            }
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

        currentButton = -1;

        if (currentActiveButtons.Count > 0)
        {
            //Button befindet sich über dem Strich
            if (currentActiveButtons[0].sprite.Equals(GameManager.current.ButtonSprites[0]))
            {
                currentButton = 0;
            }
            else if (currentActiveButtons[0].sprite.Equals(GameManager.current.ButtonSprites[1]))
            {
                currentButton = 1;
            }
            else if (currentActiveButtons[0].sprite.Equals(GameManager.current.ButtonSprites[2]))
            {
                currentButton = 2;
            }
            else if (currentActiveButtons[0].sprite.Equals(GameManager.current.ButtonSprites[3]))
            {
                currentButton = 3;
            }

            if (currentInput == currentButton)
            {
                //Handle correct pressed button logic
                correctClicked++;

                //move button to already clicked so it doesn't get clicked again
                DestroyImmediate(currentActiveButtons[0].gameObject);
                currentActiveButtons.RemoveAt(0);
                //clickedButtons.Add(button);
            }
        }


        //Detect false button presses
        if (currentInput >= 0 && currentInput != currentButton)
        {
            falseClicked++;

            if(falseClicked % GameManager.current.FehlerAnzahl  == 0)
            {
                StunPlayer();
            }
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

    //Stuns the player
    public void StunPlayer()
    {
        spinsPerSecond = 0;
        for (int i = 0; i < GameManager.current.secondsSmoothed * 60; i++)
        {
            spinSpeedSaves.Dequeue();
            spinSpeedSaves.Enqueue(0);
        }

        //löscht alle buttons des Spieler
        foreach(Image button in currentActiveButtons)
        {
            Destroy(button.gameObject);
        }
        currentActiveButtons.Clear();


    }
}
