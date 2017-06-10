using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    public int pSpeed;
    public int distance;
    public bool win;
    public GameObject winSprite, loseSprite;
    public int playerNr;
    public GameObject act, next;
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
	}

    void switchField()
    {
        
        transform.position = new Vector3(transform.position.x, 30, transform.position.z);
        act.transform.position = act.transform.position + Vector3.up * 32;
        next.transform.position = next.transform.position + Vector3.up * 32;
        act.GetComponent<Tiling>().numb = playerNr * 10 + 2;
        next.GetComponent<Tiling>().numb = playerNr * 10 + 1;
        StartCoroutine(switchNext());
    }
    IEnumerator switchNext()
    {
        while (transform.position.y > 16)
        {
            yield return new WaitForEndOfFrame();
        }
        act.transform.position = next.transform.position - Vector3.up * 32;
        GameObject temp = act;
        act = next;
        next = temp;
        next.GetComponent<Tiling>().init();
    }
	// Update is called once per frame
	void Update () {

        transform.Translate(Vector3.down * Time.deltaTime * pSpeed);
        
        if (transform.position.y <= 2)
        {
            switchField();
        }
    }
}
