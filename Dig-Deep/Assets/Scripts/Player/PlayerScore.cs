using UnityEngine;

/// <summary> This class calculates the scores of the players. </summary>
public class PlayerScore : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject p1Drill, p2Drill;

    private int p1score = 0;        //Extrascore of the player
    private int p2score = 0;
    //For next version:
    //private int p1depth = 0;        //Depthscore of the player
    //private int p2depth = 0;
    //private int p1multiplyer = 1;   //Material-Multiplyer
    //private int p2multiplyer = 1;
    [SerializeField]
    [Tooltip("Relative speed of the player")]
    private float p1speed = 0, p2speed = 0;
    //private bool p1value = false;   //is the player surounded by valuable material
    //private bool p2value = false;
    #endregion

    void Update()
    {
        //p1depth += (int)((p1speed) * p1multiplyer);
        //p2depth += (int)((p2speed) * p2multiplyer);
        //if (p1value) { p1score += 10 / p1multiplyer; }
        //if (p2value) { p2score += 10 / p2multiplyer; }
        
        p1score = (int)-(p1Drill.transform.position.y - 27);
        p2score = (int)-(p2Drill.transform.position.y - 27);

        GameManager.current.playerScore1 = p1score;
        GameManager.current.playerScore2 = p2score;

        if (!GameManager.current.run)
        {
            GameManager.current.winnerScore = Mathf.Max(new int[]{ p1score, p2score });
        }
    }

    /*private void SetMultiplyer(bool player, int materialID)//TODO just an idea
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
    /* public int GetFinalScore(bool player)
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
}