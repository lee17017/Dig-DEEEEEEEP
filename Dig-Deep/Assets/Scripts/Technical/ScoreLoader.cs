using UnityEngine;

/// <summary> This class handles saving/loading of the scorelist. </summary>
public static class ScoreLoader
{
    /// <summary> Loads the current scorelist from the savefile. </summary>
    public static string[] LoadList()
    {
        string scorelist = "";
        if (System.IO.File.Exists(Application.dataPath + "//resources//scores.txt"))
        {
            scorelist = System.IO.File.ReadAllText(Application.dataPath + "//resources//scores.txt");
        }
        return scorelist.Split();
    }

    /// <summary> Saves the new scorelist to the savefile. </summary>
    public static void WriteToBoard(string name, int score)
    {
        string[] scorelist = LoadList();
        int length = scorelist.Length;

        string scoreText = "";
        if (length % 2 != 0 || length < 2)
        {
            scoreText = score + " " + name;
        }
        else
        {
            //int min = 0;
            //int middle = length / 4;//Teile durch 2 wegen Datenpaaren und nochmal um die Mitte zu bekommen 
            //int max = length/2-1;//Weil es Datenpaare sind
            bool changed = false;
            for (int i = 0; i < length; i += 2)
            {
                int compare;
                int.TryParse(scorelist[i], out compare);
                if (score > compare)
                {
                    if (i > 18)
                    {
                        scorelist[i - 1] += " " + score + " " + name;
                        changed = true;
                        break;
                    }
                    scorelist[i] = score + " " + name + " " + scorelist[i];
                    changed = true;
                    break;
                }
            }

            if (!changed)
            {
                scorelist[length - 1] += " " + score + " " + name;
            }

            scoreText = "";
            for (int i = 0; i < length; i++)
            {
                if (i > 18)
                {
                    break;
                }
                scoreText += scorelist[i];
                if (i < length - 1)
                {
                    scoreText += " ";
                }
            }
        }
        System.IO.File.WriteAllText(Application.dataPath + "//resources//scores.txt", scoreText);
    }
}