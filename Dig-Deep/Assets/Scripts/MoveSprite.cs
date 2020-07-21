using System.Collections;
using UnityEngine;

public class MoveSprite : MonoBehaviour //TODO maybe integrate into "NextLayer"
{
    [SerializeField]
    private Transform start = null;
    [SerializeField]
    private Transform end = null;

    public void MoveDrill()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float passedTime = 0;

        while (true)
        {
            passedTime += Time.deltaTime;

            transform.position = Vector3.Lerp(start.position, end.position, passedTime / 2);

            if (transform.position == end.position)
            {
                yield break;
            }

            yield return null;
        }
    }
}