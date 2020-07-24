using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary> This class handles userinput and displaying the current scorelist in the Highscore scene. </summary>
public class Highscore : MonoBehaviour
{
    private Text highscoreDisplay;
    private GameObject inputLayout;
    private InputField inputfield;

    private int playerStat;
    private string playerName;

    private bool enterName;

    private string[] scores = null;

    private void Awake()
    {
        highscoreDisplay = transform.GetChild(2).GetComponent<Text>();
        highscoreDisplay.gameObject.SetActive(false);
        inputLayout = transform.GetChild(3).gameObject;
        inputLayout.SetActive(false);
        inputfield = inputLayout.transform.GetComponentInChildren<InputField>();

        playerStat = GameManager.current.winnerScore;
    }

    private void Start()
    {
        scores = ScoreLoader.LoadList();

        if (CheckIn(playerStat))
        {
            RequestName();
        }
        else
        {
            PrintList();
            StartCoroutine(ContinueToMenu());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ScoreLoader.WriteToBoard(playerName, playerStat);

            enterName = false;
            inputLayout.SetActive(false);
            scores = ScoreLoader.LoadList();
            PrintList();
        }

        if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0") || (GameManager.current.tastatur && Input.GetKeyDown(KeyCode.Return)))
        {
            SceneManager.LoadScene(0);
        }
    }

    /// <summary> Asks the user to enter a name. </summary>
    private void RequestName()
    {
        inputLayout.SetActive(true);
        enterName = true;
        StartCoroutine(GetName());
    }

    /// <summary> Scans the entered text and replaces " " with "_". </summary>
    IEnumerator GetName()
    {
        while (enterName)
        {
            playerName = inputfield.text.Replace(' ', '_');
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(ContinueToMenu());
    }

    /// <summary> Waits for 20 Seconds and reloads the menu afterwards. </summary>
    IEnumerator ContinueToMenu()
    {
        yield return new WaitForSeconds(20);
        SceneManager.LoadScene(0);
    }

    /// <summary> Asks the user to enter a name. </summary>
    private bool CheckIn(int score)
    {
        for (int i = 0; i < scores.Length; i += 2)
        {
            int x;
            if (int.TryParse(scores[i], out x) && (x < score))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary> Writes all scores into the given textfield. </summary>
    private void PrintList()
    {
        string display = "";
        if (scores != null)
        {
            for (int i = 0; i < scores.Length - 1; i += 2)
            {
                display += scores[i] + "\t\t\t" + scores[i + 1] + "\n";
            }
        }

        highscoreDisplay.text = display;
        highscoreDisplay.gameObject.SetActive(true);
    }
}