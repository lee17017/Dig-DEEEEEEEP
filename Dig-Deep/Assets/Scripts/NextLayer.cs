using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLayer : MonoBehaviour {

    public Transform start;
    public Transform end;

    public bool active;

    private void Update()
    {
        if (active)
        {
            active = false;
            moveEnd();
        }
    }

    public void moveEnd()
    {
      //  start.position = this.transform.position;
        //end.position = end.transform.position;// - new Vector3(0,161,0);

       // end.position = end.position - new Vector3(0, 161, 0);
        StartCoroutine(Move());
    }
    
    IEnumerator Move()
    {
        float passedTime = 0;

        while (true)
        {
            passedTime += Time.deltaTime;

            this.transform.position = Vector3.Lerp(start.position, end.position, passedTime/5);

            if (this.transform.position == end.position)
            {                
                yield break;
            }

            yield return null;
        }
    }
}
