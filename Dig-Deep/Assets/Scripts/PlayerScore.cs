﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScore : MonoBehaviour {
    private int p1score = 0;        //Extrascore des Spielers
    private int p2score = 0;
    private int p1depth = 0;        //Depthscore des Spielers
    private int p2depth = 0; 
    private int p1multiplyer = 1;   //Material-Multiplyer
    private int p2multiplyer = 1;
    public float p1speed = 0;       //relative Geschwindigkeit des Spielers
    public float p2speed = 0;
    private bool p1value = false;   //Befindet sich der Spieler in wertvollem Material?
    private bool p2value = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(Termination());
	}
	
	// Update is called once per frame
	void Update () {
        p1depth += (int)((p1speed) * p1multiplyer);
        p2depth += (int)((p2speed) * p2multiplyer);
        if (p1value){p1score += 10/p1multiplyer;}
        if (p2value){p2score += 10 / p2multiplyer;}
        Debug.Log(p1depth + " " + p1score + " " + p2depth + " " + p2score);

        if (Input.GetButtonDown("Fire1"))
        {
            p1speed++;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            writeToBoard("YouAreAFooooooooooool.JustAJoke:P",1000);
            writeToBoard("Test1", 100);
            writeToBoard("Test2", 90);
            writeToBoard("Test3", 80);
            writeToBoard("Test5", 50);
            writeToBoard("Test4", 60);
        }
	}

    public void setMultiplyer(bool player, int materialID)
    {
        if (player) {
            //setze p1Multiplyer
        }
        else
        {
            //setze p2Multiplyer
        }
        //Setze Multiplyer abhängig von Material
        //Erde 6
        //Sand 5
        //Kies 4
        //Stein 3
        //Metall 2
        //Edelstein 1

    }
    //Sollte bei Spielende aufgerufen werden
    public int getFinalScore(bool player)
    {
        if (player)
        {
            return p1score + p1depth;
        }
        else
        {
            return p2score + p2depth;
        }
        
    }

    public void writeToBoard(string name, int score)
    {
        string scorelist="";
        if (System.IO.File.Exists(Application.dataPath + "//resources//scores.txt"))
        {
            scorelist= System.IO.File.ReadAllText(Application.dataPath + "//resources//scores.txt");//Lese von txt
        }
        //Füge neues Score ein
        string[] wordlist = scorelist.Split();
        int length=wordlist.Length;
        Debug.Log(length);
        if (length % 2 != 0||length<2)
        {
            scorelist = score + " " + name;
            //0 -> 0    ->  Write direct            //Done
            //ungerade  ->  Fehler! File ersetzen   //Done
        }
        else
        {
            //int middle = length / 4;//Teile durch 2 wegen Datenpaaren und nochmal um die Mitte zu bekommen 
            //int min = 0;
            //int max = length/2-1;//Weil es Datenpaare sind
            //wordlist=binSea(wordlist, min, max, length, name, score);
            bool changed = false;
            for (int i = 0; i < length; i += 2)
            {
                int compare;
                Int32.TryParse(wordlist[i], out compare);
                Debug.Log("Vergleich: " + score + " " + compare);
                if (score > compare)
                {
                    wordlist[i] = score + " " + name + " " + wordlist[i];
                    changed = true;
                    Debug.Log(wordlist[i]);
                    break;
                }
            }
            if (!changed)
            {
                wordlist[length - 1] += " " + score + " " + name;
            }

            scorelist = "";
            for (int i = 0; i < length; i++)
            {
                scorelist += wordlist[i];
                if (i < length - 1)
                {
                    scorelist += " ";
                }
            }
            Debug.Log("Liste: " + scorelist);
            //2 -> 0    ->  convert 1 to int compare und setze dann davor/danach                                1           11/12
            //4 -> 1    ->  convert 3 to int compare und fahre dann bei 1 fort oder schreibe dahinter           1 3         1/32    11/12  
            //6 -> 1    ->  convert 3 to int compare und fahre dann bei 1 fort oder teste bei 5                 1 3 5       1/5     11/12 // 51/52
            //8 -> 2    ->  convert 5 to int compare und fahre dann bei 3 fort oder schreibe dahinter           1 3 5 7     3/7     1/32 // 71/72   11/12
            //10-> 2    ->  convert 5 to int compare und                                                        1 3 5 7 9   3/7     1/32 // 71/9    11/12 // 91/92
            //12-> 3    ->  convert 7 to int compare und
        }
        
        System.IO.File.WriteAllText(Application.dataPath + "//resources//scores.txt", scorelist);//Schreibe in txt
    }

    //Methode ist fehlerhaft
    private string[] binSea(string[] input, int min, int max, int length, string name, int score)
    {
        Debug.LogError("binSea() wurde genutzt, obwohl diese Methode fehlerhaft ist");
        while (min <= max)
        {
            int compare;
            int mid = (min + max) / 2;
            Int32.TryParse(input[mid * 2], out compare);
            if (score == compare)
            {
                //return ++mid;
                input[mid * 2+1] += " " + score + " " + name;
                return input;
            }
            else if (score < compare)
            {
                max = mid - 1;
            }
            else
            {
                min = mid + 1;
            }

        }
        input[max+1] += " " + score + " " + name;
        return input;
    }

    IEnumerator Termination()
    {
        yield return new WaitForSeconds(30);
        int score1 = getFinalScore(true);
        int score2 = getFinalScore(false);
        if (score1 < score2)
        {
            Debug.Log("Player2 wins!");
        }
        else if (score1 > score2)
        {
            Debug.Log("Player1 wins!");
        }
        else
        {
            Debug.Log("There is no winner... (Sudden Death ist available for only 2$. Just search for it in our InAppPurchase-section");
        }
    }
}
