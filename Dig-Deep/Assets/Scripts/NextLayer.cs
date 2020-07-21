using System.Collections;
using UnityEngine;

public class NextLayer : MonoBehaviour
{
    [SerializeField]
    private Transform start = null;
    [SerializeField]
    private Transform end = null;
    [SerializeField]
    private bool active = false;

    private void Update()
    {
        if (active)
        {
            active = false;
            MoveEnd();
        }
    }

    private void MoveEnd()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float passedTime = 0;

        while (true)
        {
            passedTime += Time.deltaTime;

            transform.position = Vector3.Lerp(start.position, end.position, passedTime / 5);

            if (transform.position == end.position)
            {
                yield break;
            }

            yield return null;
        }
    }
}