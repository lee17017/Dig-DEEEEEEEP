using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimation : MonoBehaviour {
    public float pSpeed;
    public bool obstacle;
    public bool cameraStop=false;
    
    public int distance;
    public GameObject winSprite, loseSprite;
    public int playerNr;
    public GameObject act, next, darkEarth;
    private bool switching = false, switching2 = false;
    private float animSpeed = 4;

    [SerializeField]
    RuntimeAnimatorController drillAnim_default, drillAnim_special;

    
    public IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(1);
        //Move
        for (int i = 0; i < distance; i++)
        {
            transform.Translate(Vector3.down*Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        //Rotate
        for (int i = 0; i < 30; i++)
        {
            transform.Rotate(new Vector3(0, 0, playerNr==1 ? -3:3));
            transform.Translate(Vector3.down * Time.deltaTime* animSpeed);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < distance / 2; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }
        GameObject temp;
        if ((playerNr == 1 && GameManager.current.winnerScore == GameManager.current.playerScore1) || (playerNr == 2 && GameManager.current.winnerScore == GameManager.current.playerScore2))
        {
            temp = (GameObject)Instantiate(winSprite);
        }
        else
        {
            temp = (GameObject)Instantiate(loseSprite);
        }
            temp.transform.position = new Vector3(transform.position.x + (playerNr == 1 ? -2.2f : 2.2f), this.transform.position.y-0.5f, 0.5f);
       

        for (int i = 0; i < distance-5; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }
        cameraStop = true;
        for (int i = 0; i < distance * 3; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
    }
	// Use this for initialization
	void Start () {
        //StartCoroutine(Animation());

        //Change Animation Controller if a Player is using Easter Egg model
        switch(playerNr)
        {
            case 1:
                if (GameManager.current.Player1EasterEgg)
                {
                    GetComponentInChildren<Animator>().runtimeAnimatorController = drillAnim_special;
                }
                else
                {
                    GetComponentInChildren<Animator>().runtimeAnimatorController = drillAnim_default;
                }
                break;
            case 2:
                if (GameManager.current.Player2EasterEgg)
                {
                    GetComponentInChildren<Animator>().runtimeAnimatorController = drillAnim_special;
                }
                else
                {
                    GetComponentInChildren<Animator>().runtimeAnimatorController = drillAnim_default;
                }
                break;
        }
	}
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.current.run)
        {
            if (!obstacle)
            {
                pSpeed = GameManager.current.baseSpeed + GetComponent<Player>().correctPerSecond * GameManager.current.speedEffect;
            }
            else
            {
                pSpeed = 0;
            }

        transform.Translate(Vector3.down * Time.deltaTime * pSpeed);

        }
        else
        {
            pSpeed = GameManager.current.baseSpeed;
        }

        if ((mod((int)transform.position.y, 32) <= 16 && !switching))
        {
            switchNext();
        }

        if (mod((int)(transform.position.y), 32) <= 1)
        {
            switchField();
        }

        if (mod((int)(transform.position.y), 32) >= 31 && !switching2)
        {
            switchField2();
        }



        if (switching && (mod((int)transform.position.y, 32) >= 26))
        {
            switching = false;
        }

        if (switching2 && (mod((int)transform.position.y, 32) >= 20))
        {
            switching2 = false;
        }
    }

    void switchField()
    {
        
        next.GetComponent<Tiling>().numb = playerNr * 10 + 1;
        
    }
    void switchField2()
    {
        switching2 = true;
        act.GetComponent<Tiling>().numb = playerNr * 10 + 2;
    }
    void switchNext()
    {

        switching = true;
        act.transform.position = next.transform.position - Vector3.up * 32;
        GameObject temp = act;
        act = next;
        next = temp;
        next.GetComponent<Tiling>().init();
        act.GetComponent<Tiling>().removeShit();
        darkEarth.transform.Translate(Vector3.down * 32);
    }
    int mod(int a, int b)
    {
        return (a % b + b) % b;
    }
}
