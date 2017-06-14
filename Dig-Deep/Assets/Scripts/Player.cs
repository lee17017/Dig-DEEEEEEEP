using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class Player : MonoBehaviour {
    
    // Player Selection
    public int player;

    public LayerMask obstacles;
    
    // Spin Input
    public float horizontalRaw;
    public float verticalRaw;
    float angleRaw;

    bool up, right, down, left;
    float spinTime;

    public float spinsPerSecond;
    [SerializeField]
    public Queue<int> spinSpeedSaves;

    public float correctPerSecond;
    public Queue<int> correctButtonPresses;
    
    [SerializeField]
    Image defaultImage;

    private float timeSinceLastSpawn = 0;

    private List<Image> currentActiveButtons;

    private int sequencePos = 0;

    public int correctClicked = 0;
    public int falseClicked = 0;
    private bool clicked = false;

    private bool endAnimStarted = false;

    public bool obstacle;
    public GameObject mashButton;

    // Use this for initialization
    void Start ()
    {
        currentActiveButtons = new List<Image>();

        spinSpeedSaves = new Queue<int>();
        correctButtonPresses = new Queue<int>();

        for(int i=0; i< GameManager.current.secondsSmoothed *60; i++)
        {
            spinSpeedSaves.Enqueue(0);

        }
        for(int i=0; i<GameManager.current.clickSecondsSmoothed*60; i++)
        {
            correctButtonPresses.Enqueue(0);
        }

        //StartCoroutine(findControllerIndex());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.current.run)
        {
            if (!obstacle)
            {
                correctPerSecond = 0;
                foreach (int click in correctButtonPresses)
                {
                    correctPerSecond += click;
                }
                ButtonMovement();
                ButtonHandling();
            }
        }
        else
        {
            foreach(Image cur in currentActiveButtons)
            {
                Destroy(cur.gameObject);
            }
            currentActiveButtons.Clear();

            if(!endAnimStarted)
            {
                StartCoroutine(GetComponentInParent<PlayerAnimation>().EndAnimation());
                endAnimStarted = true;
            }
        }
        
    }

    void FixedUpdate()
    {
        if (GameManager.current.run)
        {
            SpinInput();
        }
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

        //Input abfragen
        if (!clicked)
        {
            clicked = true;
            if (Input.GetKeyDown("joystick " + player + " button 0") || Input.GetAxis("Vertical") < 0)
            {
                currentInput = 0;
            }
            else if (Input.GetKeyDown("joystick " + player + " button 1") || Input.GetAxis("Horizontal") > 0)
            {
                currentInput = 1;
            }
            else if (Input.GetKeyDown("joystick " + player + " button 2") || Input.GetAxis("Horizontal") < 0)
            {
                currentInput = 2;
            }
            else if (Input.GetKeyDown("joystick " + player + " button 3") || Input.GetAxis("Vertical") > 0)
            {
                currentInput = 3;
            }
            else
            {
                clicked = false;
            }
        }
        else
        {
            //Might cause problems :/ But i'll give it a try
            if (Input.GetAxis("Vertical") < 0.0001f && Input.GetAxis("Vertical") > -0.0001f && Input.GetAxis("Horizontal") < 0.0001f && Input.GetAxis("Horizontal") > -0.0001f)
            {
                clicked = false;
            }
        }

        currentButton = -1;

        if (currentActiveButtons.Count > 0)
        {
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
            if (currentInput >= 0)
            {
                Debug.Log(currentInput + " " + currentButton);
            }
            if (currentInput == currentButton)
            {
                Debug.Log("Richtig!");
                //Handle correct pressed button logic
                correctClicked++;
                correctButtonPresses.Enqueue(1);
                correctButtonPresses.Dequeue();

                //move button to already clicked so it doesn't get clicked again
                DestroyImmediate(currentActiveButtons[0].gameObject);
                currentActiveButtons.RemoveAt(0);
                //clickedButtons.Add(button);
            }
            else
            {
                correctButtonPresses.Enqueue(0);
                correctButtonPresses.Dequeue();
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
        AudioSource.PlayClipAtPoint(GameManager.current.stunClip, transform.position, 1f);

        StartCoroutine(vibrateController(player - 1));
        spinsPerSecond = 0;
        for (int i = 0; i < GameManager.current.secondsSmoothed * 60; i++)
        {
            spinSpeedSaves.Dequeue();
            spinSpeedSaves.Enqueue(0);
        }

        correctPerSecond = 0;
        for(int i=0; i<GameManager.current.clickSecondsSmoothed*60; i++)
        {
            correctButtonPresses.Dequeue();
            correctButtonPresses.Enqueue(0);
        }

        //löscht alle buttons des Spieler
        foreach(Image button in currentActiveButtons)
        {
            Destroy(button.gameObject);
        }
        currentActiveButtons.Clear();


    }

    IEnumerator findControllerIndex()
    {
        bool playerIndexSet = false;
        PlayerIndex playerIndex;

        for (int i = 0; i < 4; ++i)
        {
            Debug.Log(i);
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                playerIndex = testPlayerIndex;
                playerIndexSet = true;

                GamePad.SetVibration(playerIndex, 0.2f, 0.2f);
                yield return new WaitForSeconds(2);
                GamePad.SetVibration(playerIndex, 0f, 0f);

            }

            
        }

       
    }

    IEnumerator vibrateController(int i)
    {
        bool playerIndexSet = false;
        PlayerIndex playerIndex;


        PlayerIndex testPlayerIndex = (PlayerIndex)i;//((i+1)%2);
        GamePadState testState = GamePad.GetState(testPlayerIndex);
        if (testState.IsConnected)
        {
           // Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
            playerIndex = testPlayerIndex;
            playerIndexSet = true;

            GamePad.SetVibration(playerIndex, 0.2f, 0.2f);
            yield return new WaitForSeconds(2);
            GamePad.SetVibration(playerIndex, 0f, 0f);

        }


        
    }

    void OnTriggerEnter(Collider other)
    {
            if(GameManager.current.run)
                StartCoroutine(Obstacle(other));
    }

    IEnumerator Obstacle(Collider other)
    {
        yield return null;

        GetComponent<PlayerAnimation>().obstacle = true;
        obstacle = true;

        GameObject mash = Instantiate(mashButton, transform.position + Vector3.down*8, Quaternion.identity);
        mash.GetComponent<MashButton>().player = player;

        StunPlayer();

        int hits = 0;

        int type;

        switch (other.tag)
        {
            case "Rock":
                type = 0;
                break;
            case "Iron":
                type = 1;
                break;
            default:
                type = 0;
                break;
        }

        while(hits < GameManager.current.hitsNeeded[type])
        {
            if (Input.GetKeyDown("joystick " + player + " button " + GameManager.current.buttonNeeded[type]))
            {
                hits++;
            }
            yield return null;
        }

        GetComponent<PlayerAnimation>().obstacle = false;
        obstacle = false;

        DestroyImmediate(other.gameObject);
        DestroyImmediate(mash);
    }
}
