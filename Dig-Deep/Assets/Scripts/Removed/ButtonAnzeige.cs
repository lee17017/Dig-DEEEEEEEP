using System;
using UnityEngine;

[Obsolete("This class has been removed and does nothing.")]
public class ButtonAnzeige : MonoBehaviour {//TODO remove this class (integrated into player)
    
    /*enum buttons {orange, blue, red, green };

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Image defaultImage;

    [SerializeField]
    private Sprite[] ButtonSprites;

    [SerializeField]
    private float spawnInterval;

    [SerializeField]
    private float travelspeed;

    private float timeSinceLastSpawn = 0;

    private List<Image> currentActiveButtons;
    private List<Image> clickedButtons;//have already been clicked, will only be moved out and destroyed
    
    private int correctClicked = 0;
    private int falseClicked = 0;
    
    void Start()
    {
        currentActiveButtons = new List<Image>();
        clickedButtons = new List<Image>();
    }
	
	void Update ()
    {
        Debug.Log(gameObject.name);
        //Button Spawning
        timeSinceLastSpawn += Time.deltaTime;

		if(timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0;

            Image newButton = Instantiate(defaultImage);
            newButton.transform.SetParent(canvas.transform, false);
            newButton.transform.SetAsFirstSibling();
            newButton.sprite = ButtonSprites[Random.Range(0, ButtonSprites.Length)];
            currentActiveButtons.Add(newButton);
        }

        //Button movement
        foreach(Image button in currentActiveButtons)
        {
            button.transform.position = button.transform.position + Vector3.left * travelspeed * Time.deltaTime;
        }
        foreach(Image button in clickedButtons)
        {
            button.transform.position = button.transform.position + Vector3.left * travelspeed * Time.deltaTime;
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

        foreach (Image button in currentActiveButtons)
        {
            if(button.transform.position.x > 75 && button.transform.position.x < 125)
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

                if(currentInput == currentButton)
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
        if(currentInput >= 0 && currentInput != currentButton)
        {
            falseClicked++;
        }
    }*/
}