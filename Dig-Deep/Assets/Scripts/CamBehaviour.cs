using UnityEngine;
using System.Collections;

public class CamBehaviour : MonoBehaviour {

    public GameObject playerObject;
    public Player player;
    private Vector3 offset;

    public float speedOffset;

    void Start()
    {
        offset = transform.position - playerObject.transform.position;
        player = playerObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        speedOffset = Mathf.Lerp(speedOffset, player.correctPerSecondd * GameManager.current.speedFeedback, GameManager.current.speedFeedbackResponseTime/100f);
        transform.position = playerObject.transform.position + offset + Vector3.up*speedOffset;
    }
}
