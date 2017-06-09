using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour {
    private int score = 0;
    private int depth = 0;
    private int multiplyer = 1;
    public float speed = 0;
    private bool value = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        depth += (int)((speed) * multiplyer);
        if (value)
        {
            score += 10/multiplyer;
        }
        Debug.Log(depth + " " + score);

        if (Input.GetButtonDown("Fire1"))
        {
            speed++;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            writeToBoard("You are a fooooooooooool. Just a joke :P");
        }
	}

    public void setMultiplyer(int materialID)
    {
        //Setze Multiplyer abhängig von Material
        //Erde 6
        //Sand 5
        //Kies 4
        //Stein 3
        //Metall 2
        //Edelstein 1

    }

    public void writeToBoard(string output)
    {
        string scorelist="";
        if (System.IO.File.Exists(Application.dataPath + "//resources//scores.txt"))
        {
            scorelist= System.IO.File.ReadAllText(Application.dataPath + "//resources//scores.txt");//Lese von txt
        }
        //Füge neues Score ein
        Debug.Log("Read List");
        scorelist += output;
        System.IO.File.WriteAllText(Application.dataPath + "//resources//scores.txt", scorelist);//Schreibe in txt
        Debug.Log("Saved List");
    }
}
