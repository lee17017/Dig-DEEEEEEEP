using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {

    public Text list;
    public InputField userInput;
    public GameObject input;

    private int plyerStat;
    private string playerName;

    private bool enterName;

    string[] scores = null; //{ "", "" };
    private bool nameEntered;

    private void Awake()
    {
        list.gameObject.SetActive(false);
        input.SetActive(false);
        
        plyerStat = GameManager.current.winnerScore;
    }

    private void Start()
    {
        loadList();

        if (checkIn(plyerStat))
        {
            requestName();
        }
        else
        {
            printList();
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            PlayerScore.writeToBoard(playerName, plyerStat);

            enterName = false;
            nameEntered = true;
        }

        if (nameEntered)
        {
            nameEntered = false;
            input.SetActive(false);
            loadList();
            printList();
        }

        if(Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0"))
        {
            SceneManager.LoadScene(0);
        }

       
    }

    private void requestName()
    {        
        input.SetActive(true);
        enterName = true;
        //StartCoroutine(limitTime());
        StartCoroutine(getName());
    }

    IEnumerator getName()
    {
        while (enterName)
        {
            playerName = userInput.text.Replace(' ', '_');
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator limitTime()
    {
        yield return new WaitForSeconds(30);
        SceneManager.LoadScene(0);
    }

    private bool checkIn(int score)
    {
       

        for (int i = 0; i < scores.Length; i += 2)
        {
            int x;
            if (Int32.TryParse(scores[i], out x) && (x < score))
            {
                return true;
            }
        }

        return false;
    }

    private void loadList()
    {
        string scorelist = "";
        if (System.IO.File.Exists(Application.dataPath + "//resources//scores.txt"))
        {
            scorelist = System.IO.File.ReadAllText(Application.dataPath + "//resources//scores.txt");//Lese von txt

        }
        scores = scorelist.Split();
    }

    private void printList()
    {
        string display = "";
        if (scores != null)
        {
            for (int i = 0; i < scores.Length-1; i += 2)
            {
                display += scores[i] + "\t\t\t" + scores[i + 1] + "\n";
            }
        }

        list.text = display;
        list.gameObject.SetActive(true);
    }

}
