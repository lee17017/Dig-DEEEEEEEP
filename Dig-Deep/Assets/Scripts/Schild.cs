using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Schild : MonoBehaviour {


    public GameObject schild;
    public GameObject plyerRef;

    public Text point;

    public GameObject[] start_end;

    private bool move;
	// Use this for initialization
	void Start () {
		
	}

   
    // Update is called once per frame
    void Update () {

        
        if(((int)(-1*(plyerRef.transform.position.y-27)))%25 == 0)
        {
            move = true;
            int tiefe = (int) ((-(plyerRef.transform.position.y - 27)) / 25);

            tiefe *= 25;
            point.text = tiefe.ToString();
            //StartCoroutine(displaySheeld());
        }

		
	}

    void swap_start_end()
    {
        GameObject cache = start_end[0];
        start_end[0] = start_end[1];
        start_end[1] = cache;
    }

    IEnumerator moveSheeld()
    {
        move = true;
        float passedTime = 0;

        while (true)
        {
            passedTime += Time.deltaTime;

            schild.transform.position = Vector3.Lerp(start_end[0].transform.position, start_end[1].transform.position, passedTime / 2);

            if (schild.transform.position == start_end[1].transform.position)
            {
                move = false;
                yield break;
                
            }

            yield return null;
        }
    }

    IEnumerator displaySheeld()
    {
        
        StartCoroutine(moveSheeld());
        move = true;
        //yield return new WaitForSeconds(2f);
        move = true;
        swap_start_end();
        yield return new WaitForSeconds(2f);
        StartCoroutine(moveSheeld());
    }
}
