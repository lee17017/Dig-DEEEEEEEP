using System;
using System.Collections;
using UnityEngine;

/// <summary> This class animates a translation of a gameobject. </summary>
public class MoveSprite : MonoBehaviour
{
    [SerializeField]
    [Obsolete("Has been replaced by 'waypoints'")]
    protected Transform start = null;
    [SerializeField]
    [Obsolete("Has been replaced by 'waypoints'")]
    protected Transform end = null;

    protected float durationModifier = 2;
    protected Transform[] waypoints;

    void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        waypoints = new Transform[2];
        if (start)
        {
            waypoints[0] = start;
        }
        if (end)
        {
            waypoints[1] = end;
        }
    }

    [Obsolete("'MoveDrill' has been replaced by 'StartMoving'")]
    public void MoveDrill()
    {
        StartMoving();
    }

    /// <summary> Starts the translation animation of the gameobject this component is attached to. </summary>
    public void StartMoving()
    {
        StartCoroutine(Move());
    }

    /// <summary> Animates the translation of the gameobject this component is attached to. </summary>
    protected IEnumerator Move()
    {
        yield return StartCoroutine(Move(transform));
    }

    /// <summary> Animates the translation of the given transform. </summary>
    /// <param name="t"> The animated transform</param>
    /// <returns></returns>
    protected IEnumerator Move(Transform t)
    {
        float passedTime = 0;

        while (true)
        {
            passedTime += Time.deltaTime;

            t.position = Vector3.Lerp(waypoints[0].position, waypoints[1].position, passedTime / durationModifier);

            if (t.position == waypoints[1].position)
            {
                yield break;
            }

            yield return null;
        }
    }
}