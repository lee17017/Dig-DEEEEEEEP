using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public GameObject p1Drill, p2Drill;

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
    
    void Update()
    {
        p1depth += (int)((p1speed) * p1multiplyer);
        p2depth += (int)((p2speed) * p2multiplyer);
        if (p1value) { p1score += 10 / p1multiplyer; }
        if (p2value) { p2score += 10 / p2multiplyer; }
        
        p1score = (int)-(p1Drill.transform.position.y - 27);
        p2score = (int)-(p2Drill.transform.position.y - 27);

        GameManager.current.playerScore1 = p1score;
        GameManager.current.playerScore2 = p2score;

        if (!GameManager.current.run)
        {
            GameManager.current.winnerScore = Mathf.Max(new int[]{ p1score, p2score });
        }
    }

    /*private void setMultiplyer(bool player, int materialID)//TODO just an idea
    {
        if (player)
        {
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

    }*/
    //Sollte bei Spielende aufgerufen werden
    /* public int getFinalScore(bool player)
     {
         if (player)
         {
             return p1score + p1depth;
         }
         else
         {
             return p2score + p2depth;
         }

     }*/

    public static void WriteToBoard(string name, int score)
    {
        string scorelist = "";
        if (System.IO.File.Exists(Application.dataPath + "//resources//scores.txt"))
        {
            scorelist = System.IO.File.ReadAllText(Application.dataPath + "//resources//scores.txt");//Lese von txt
        }
        //Füge neues Score ein
        string[] wordlist = scorelist.Split();
        int length = wordlist.Length;
        
        if (length % 2 != 0 || length < 2)
        {
            scorelist = score + " " + name;
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
                int.TryParse(wordlist[i], out compare);
                Debug.Log("Vergleich: " + score + " " + compare);
                if (score > compare)
                {
                    if (i > 18)
                    {
                        wordlist[i - 1] += " " + score + " " + name;
                        changed = true;
                        break;
                    }
                    wordlist[i] = score + " " + name + " " + wordlist[i];
                    changed = true;
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
                if (i > 18)
                {
                    break;
                }
                scorelist += wordlist[i];
                if (i < length - 1)
                {
                    scorelist += " ";
                }
            }
        }
        System.IO.File.WriteAllText(Application.dataPath + "//resources//scores.txt", scorelist);//Schreibe in txt
    }
}