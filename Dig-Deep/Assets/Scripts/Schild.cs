using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Schild : MonoBehaviour
{
    public GameObject schild;
    public GameObject plyerRef;

    public Text point;

    public GameObject[] start_end;

    //TODO private bool move;

    void Update()
    {
        if (((int)(-1 * (plyerRef.transform.position.y - 27))) % 25 == 0)
        {
            //TODO move = true;
            int tiefe = (int)((-(plyerRef.transform.position.y - 27)) / 25);

            tiefe *= 25;
            point.text = tiefe.ToString();
            //StartCoroutine(displaySheeld());
        }
    }

    void SwapStartEnd()
    {
        GameObject cache = start_end[0];
        start_end[0] = start_end[1];
        start_end[1] = cache;
    }

    IEnumerator MoveShield()
    {
        //TODO move = true;
        float passedTime = 0;

        while (true)
        {
            passedTime += Time.deltaTime;

            schild.transform.position = Vector3.Lerp(start_end[0].transform.position, start_end[1].transform.position, passedTime / 2);

            if (schild.transform.position == start_end[1].transform.position)
            {
                //TODO move = false;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator displaySheeld()//TODO needed?
    {
        StartCoroutine(MoveShield());
        //TODO move = true;
        SwapStartEnd();
        yield return new WaitForSeconds(2f);
        StartCoroutine(MoveShield());
    }
}