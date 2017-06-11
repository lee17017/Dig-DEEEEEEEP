using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    public float pSpeed;
    
    public int distance;
    public bool win;
    public GameObject winSprite, loseSprite;
    public int playerNr;
    public GameObject act, next, darkEarth;
    private bool switching = false, switching2 = false;

    [SerializeField]
    RuntimeAnimatorController drillAnim_default, drillAnim_special;

    
    IEnumerator Animation()
    {
        //Move
        for (int i = 0; i < distance; i++)
        {
            transform.Translate(Vector3.down*Time.deltaTime* pSpeed);
            yield return new WaitForEndOfFrame();
        }

        Vector3 pos = transform.position;
        //Rotate
        for (int i = 0; i < 30; i++)
        {
            transform.Rotate(new Vector3(0, 0, -3));
            transform.Translate(Vector3.down * Time.deltaTime * pSpeed);
            yield return new WaitForEndOfFrame();
        }
        //Move
        for (int i = 0; i < distance/2; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * pSpeed);
            yield return new WaitForEndOfFrame();
        }
        //Rotate
        for (int i = 0; i < 60; i++)
        {
            transform.Rotate(new Vector3(0, 0, 3));
            transform.Translate(Vector3.down * Time.deltaTime* pSpeed);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < distance / 2; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * pSpeed);
            yield return new WaitForEndOfFrame();
        }
        if (win)
        {
            GameObject temp = (GameObject)Instantiate(winSprite);
            temp.transform.position = new Vector3(pos.x, this.transform.position.y-0.5f, 1);
        }
        for (int i = 0; i < distance; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * pSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
	// Use this for initialization
	void Start () {
        // StartCoroutine(Animation());

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

        pSpeed = GameManager.current.baseSpeed + GetComponent<Player>().correctPerSecond * GameManager.current.speedEffect;

        transform.Translate(Vector3.down * Time.deltaTime * pSpeed);

        if ((mod((int)transform.position.y, 32) <= 20 && !switching))
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
