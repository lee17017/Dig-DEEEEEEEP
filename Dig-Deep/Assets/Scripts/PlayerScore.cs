using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            writeToBoard("You are a fooooooooooool. Just a joke :P");
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

    public void writeToBoard(string output)
    {
        string scorelist="";
        if (System.IO.File.Exists(Application.dataPath + "//resources//scores.txt"))
        {
            scorelist= System.IO.File.ReadAllText(Application.dataPath + "//resources//scores.txt");//Lese von txt
        }
        //Füge neues Score ein
        string[] wordlist = scorelist.Split();
        int mid = wordlist.Length / 2;
        scorelist += output;
        System.IO.File.WriteAllText(Application.dataPath + "//resources//scores.txt", scorelist);//Schreibe in txt
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
