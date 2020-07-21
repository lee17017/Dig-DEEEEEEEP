using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public Text list;
    public InputField userInput;
    public GameObject input;

    private int playerStat;
    private string playerName;

    private bool enterName;

    private string[] scores = null;
    private bool nameEntered;

    private void Awake()
    {
        list.gameObject.SetActive(false);
        input.SetActive(false);

        playerStat = GameManager.current.winnerScore;
    }

    private void Start()
    {
        LoadList();

        if (CheckIn(playerStat))
        {
            RequestName();
        }
        else
        {
            PrintList();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayerScore.WriteToBoard(playerName, playerStat);

            enterName = false;
            nameEntered = true;
        }

        if (nameEntered)
        {
            nameEntered = false;
            input.SetActive(false);
            LoadList();
            PrintList();
        }

        if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void RequestName()
    {
        input.SetActive(true);
        enterName = true;
        //StartCoroutine(limitTime()); //TODO why not?
        StartCoroutine(GetName());
    }

    IEnumerator GetName()
    {
        while (enterName)
        {
            playerName = userInput.text.Replace(' ', '_');
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator LimitTime()
    {
        yield return new WaitForSeconds(30);
        SceneManager.LoadScene(0);
    }

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

    private void LoadList()
    {
        string scorelist = "";
        if (System.IO.File.Exists(Application.dataPath + "//resources//scores.txt"))
        {
            scorelist = System.IO.File.ReadAllText(Application.dataPath + "//resources//scores.txt");//Lese von txt

        }
        scores = scorelist.Split();
    }

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

        list.text = display;
        list.gameObject.SetActive(true);
    }
}