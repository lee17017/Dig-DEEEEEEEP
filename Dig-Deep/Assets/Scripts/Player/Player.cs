using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

/// <summary> This class handles the players input (and its effects) as well as collisions with obstacles. </summary>
[RequireComponent(typeof(PlayerAnimation))]
public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int player = 0;
    private PlayerAnimation anim;
    private PlayerAnimation parentAnim;

    public LayerMask obstacles;

    // Spin Input
    [SerializeField]
    private float horizontalRaw;
    [SerializeField]
    private float verticalRaw;
    private float angleRaw;

    private bool up, right, down, left;
    private float spinTime;

    [SerializeField]
    private float spinsPerSecond;
    [SerializeField]
    private Queue<int> spinSpeedSaves;

    public float correctPerSecond;
    [SerializeField]
    private Queue<int> correctButtonPresses;

    [SerializeField]
    private Image defaultImage = null;

    private float timeSinceLastSpawn = 0;

    private List<Image> currentActiveButtons;

    private int sequencePos = 0;

    [SerializeField]
    private int correctClicked = 0;
    [SerializeField]
    private int falseClicked = 0;

    private bool endAnimStarted = false;

    public bool obstacle;
    [SerializeField]
    private GameObject mashButton;
    #endregion

    #region Lifecycle
    void Start()
    {
        anim = GetComponent<PlayerAnimation>();
        parentAnim = GetComponentInParent<PlayerAnimation>();

        currentActiveButtons = new List<Image>();

        spinSpeedSaves = new Queue<int>();
        correctButtonPresses = new Queue<int>();

        for (int i = 0; i < GameManager.current.secondsSmoothed * 60; i++)
        {
            spinSpeedSaves.Enqueue(0);
        }

        for (int i = 0; i < GameManager.current.clickSecondsSmoothed * 60; i++)
        {
            correctButtonPresses.Enqueue(0);
        }
        //StartCoroutine(findControllerIndex());
    }

    void Update()
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
                HandleSequenceButtons();
                HandleInput();
            }
        }
        else
        {
            foreach (Image cur in currentActiveButtons)
            {
                Destroy(cur.gameObject);
            }
            currentActiveButtons.Clear();

            if (!endAnimStarted)
            {
                StartCoroutine(parentAnim.EndAnimation());
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
    #endregion

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.run)
        {
            StartCoroutine(Obstacle(other));
        }
    }

    /// <summary> Handles button spawning, movement and destruction </summary>
    private void HandleSequenceButtons()
    {
        float speed = Mathf.Max(GameManager.current.travelspeedBase * Mathf.Pow(spinsPerSecond, GameManager.current.powerSpins) * GameManager.current.speedMultiplier, GameManager.current.travelspeedBase) * Time.deltaTime;
        float spawnTime = GameManager.current.distance / speed * Time.deltaTime;
        
        timeSinceLastSpawn += Time.deltaTime;

        //Button spawning
        if (timeSinceLastSpawn >= spawnTime)
        {
            timeSinceLastSpawn = 0;

            Image newButton = Instantiate(defaultImage);
            newButton.transform.SetParent(GameManager.current.canvas.transform, false);
            newButton.transform.SetAsLastSibling();
            newButton.sprite = GameManager.current.buttonSprites[GameManager.current.sequence[sequencePos]];
            sequencePos++;
            newButton.color = new Color(1, 1, 1, 1);
            currentActiveButtons.Add(newButton);
        }

        //Button movement
        foreach (Image button in currentActiveButtons)
        {
            button.transform.position = button.transform.position + Vector3.down * speed;
        }

        //Button destruction
        if (currentActiveButtons.Count > 0 && currentActiveButtons[0].transform.position.y < 50)
        {
            falseClicked++;
            Destroy(currentActiveButtons[0].gameObject);
            currentActiveButtons.RemoveAt(0);

            if (falseClicked % GameManager.current.maxErrors == 0)
            {
                StunPlayer();
            }
        }
    }

    #region Inputhandling
    /// <summary> Handles button presses and sequence validation. </summary>
    private void HandleInput()
    {
        int currentInput = GetInputButtonIndex(); ;
        int currentButton = -1;

        if (currentActiveButtons.Count > 0)
        {
            if (currentActiveButtons[0].sprite.Equals(GameManager.current.buttonSprites[0]))
            {
                currentButton = 0;
            }
            else if (currentActiveButtons[0].sprite.Equals(GameManager.current.buttonSprites[1]))
            {
                currentButton = 1;
            }
            else if (currentActiveButtons[0].sprite.Equals(GameManager.current.buttonSprites[2]))
            {
                currentButton = 2;
            }
            else if (currentActiveButtons[0].sprite.Equals(GameManager.current.buttonSprites[3]))
            {
                currentButton = 3;
            }

            if (currentInput == currentButton)
            {
                //Handle correct pressed button logic
                correctClicked++;
                correctButtonPresses.Enqueue(1);
                correctButtonPresses.Dequeue();

                //move button to already clicked so it doesn't get clicked again
                DestroyImmediate(currentActiveButtons[0].gameObject);
                currentActiveButtons.RemoveAt(0);
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

            if (falseClicked % GameManager.current.maxErrors == 0)
            {
                StunPlayer();
            }
        }
    }

    /// <summary> Returns which of the four buttons has been pressed. </summary>
    /// <returns> The index of the pressed button or -1 </returns>
    private int GetInputButtonIndex()
    {
        if (Input.GetKeyDown("joystick " + player + " button 0") || (GameManager.current.tastatur && Input.GetKeyDown("s")))
        {
            return 0;
        }

        if (Input.GetKeyDown("joystick " + player + " button 1") || (GameManager.current.tastatur && Input.GetKeyDown("d")))
        {
            return 1;
        }

        if (Input.GetKeyDown("joystick " + player + " button 2") || (GameManager.current.tastatur && Input.GetKeyDown("a")))
        {
            return 2;
        }

        if (Input.GetKeyDown("joystick " + player + " button 3") || (GameManager.current.tastatur && Input.GetKeyDown("w")))
        {
            return 3;
        }

        return -1;
    }

    /// <summary> Handles spin input </summary>
    public void SpinInput()
    {
        // Actual Input
        if (GameManager.current.tastatur)
        {
            horizontalRaw = Input.GetAxis("Mouse X");
            verticalRaw = Input.GetAxis("Mouse Y");
        }
        else
        {
            horizontalRaw = Input.GetAxis("Horizontal " + player);
            verticalRaw = Input.GetAxis("Vertical " + player);
        }
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
            spinSpeedSaves.Enqueue(1);
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
        foreach (int speed in spinSpeedSaves)
        {
            spinsPerSecond += speed;
        }
    }
    #endregion

    /// <summary> Stuns the player, deletes his active sequence and plays the collision audio. </summary>
    public void StunPlayer()
    {
        AudioSource.PlayClipAtPoint(GameManager.current.stunClip, transform.position, 1f);

        StartCoroutine(VibrateController(player - 1));
        spinsPerSecond = 0;
        for (int i = 0; i < GameManager.current.secondsSmoothed * 60; i++)
        {
            spinSpeedSaves.Dequeue();
            spinSpeedSaves.Enqueue(0);
        }

        correctPerSecond = 0;
        for (int i = 0; i < GameManager.current.clickSecondsSmoothed * 60; i++)
        {
            correctButtonPresses.Dequeue();
            correctButtonPresses.Enqueue(0);
        }

        //delete the current sequence
        foreach (Image button in currentActiveButtons)
        {
            Destroy(button.gameObject);
        }
        currentActiveButtons.Clear();
    }

    #region Coroutines
    /// <summary> Activates vibration for two seconds one controller after another. </summary>
    private IEnumerator FindControllerIndex()
    {
        for (int i = 0; i < 4; ++i)
        {
            yield return StartCoroutine(VibrateController(i));
        }
    }

    /// <summary> Activates vibration for two seconds at the specified controller. </summary>
    /// <param name="i"> The player index </param>
    private IEnumerator VibrateController(int i)
    {
        PlayerIndex playerIndex;
        PlayerIndex testPlayerIndex = (PlayerIndex)i;//((i+1)%2);

        GamePadState testState = GamePad.GetState(testPlayerIndex);
        if (testState.IsConnected)
        {
            playerIndex = testPlayerIndex;

            GamePad.SetVibration(playerIndex, 0.2f, 0.2f);
            yield return new WaitForSeconds(2);
            GamePad.SetVibration(playerIndex, 0f, 0f);
        }
    }


    /// <summary> Stuns the player on collsion with obstacle and handles destruction logic of obstacle. </summary>
    /// <param name="other"> The collider of the other object </param>
    private IEnumerator Obstacle(Collider other)
    {
        yield return null;

        anim.obstacle = true;
        obstacle = true;

        GameObject mash = Instantiate(mashButton, transform.position + Vector3.down * 8, Quaternion.identity);
        mash.GetComponent<MashButton>().player = player;

        StunPlayer();

        int hits = 0;
        int type = 0;

        if (other.tag == "Iron")
        {
            type = 1;
        }

        while (hits < GameManager.current.hitsNeeded[type])
        {
            if (Input.GetKeyDown("joystick " + player + " button " + GameManager.current.buttonNeeded[type]) || Input.GetAxis("Horizontal") < 0)//Noch unschön mit Tastaturinput, aber fürs erste passts
            {
                hits++;
            }
            yield return null;
        }

        anim.obstacle = false;
        obstacle = false;

        DestroyImmediate(other.gameObject);
        DestroyImmediate(mash);
    }
    #endregion

    public int GetId()
    {
        return player;
    }
}