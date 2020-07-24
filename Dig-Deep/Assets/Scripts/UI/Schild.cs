using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary> This class handles the animations of the score-sign and the update of its scoredisplay. </summary>
public class Schild : MoveSprite
{
    [Obsolete("'plyerRef' has been replaced by 'player' (its transform)")]
    public GameObject plyerRef;
    
    private Transform sign;
    private Text points;
    [SerializeField]//TODO try to find it in code
    private Transform player;

    protected override void OnAwake()
    {
        sign = transform.GetChild(0);
        points = sign.GetComponentInChildren<Text>();
        player = plyerRef.transform;
        waypoints = new Transform[2];
        waypoints[0] = transform.GetChild(1);
        waypoints[1] = transform.GetChild(2);
        StartCoroutine(SignAnimation());
    }

    void Update()
    {
        if (((int)(-1 * (player.position.y - 27))) % 25 == 0)
        {
            int currentDepth = (int)((-(player.position.y - 27)) / 25);

            currentDepth *= 25;
            points.text = currentDepth.ToString();
        }
    }

    /// <summary> Swaps 'start' and 'end'. </summary>
    void SwapStartEnd()
    {
        Transform cache = waypoints[0];
        waypoints[0] = waypoints[1];
        waypoints[1] = cache;
    }

    /// <summary> Executes the score-sign animation. </summary>
    private IEnumerator SignAnimation()
    {
        StartCoroutine(Move(sign));
        yield return new WaitForSeconds(GameManager.current.gameDuration);
        SwapStartEnd();
        StartCoroutine(Move(sign));
    }
}