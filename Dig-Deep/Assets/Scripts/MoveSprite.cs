using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSprite : MonoBehaviour {

    public Transform start;
    public Transform end;

    public void moveDrill()
    {
        StartCoroutine(move());
    }

    IEnumerator move()
    {
        float passedTime = 0;

        while (true)
        {
            passedTime += Time.deltaTime;

            this.transform.position = Vector3.Lerp(start.position, end.position, passedTime / 2);

            if (this.transform.position == end.position)
            {
                yield break;
            }

            yield return null;
        }
    }
}
