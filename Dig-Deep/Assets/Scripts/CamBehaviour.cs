using UnityEngine;
using System.Collections;

public class CamBehaviour : MonoBehaviour {

    public GameObject playerObject;
    public Player player;
    private Vector3 offset;

    public float speedOffset;
    public float xStart;

    void Start()
    {
        xStart = transform.position.x;
        offset = transform.position - playerObject.transform.position;
        player = playerObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerObject.GetComponent<PlayerAnimation>().cameraStop)
        {
            speedOffset = Mathf.Lerp(speedOffset, player.correctPerSecond * GameManager.current.speedFeedback, GameManager.current.speedFeedbackResponseTime / 100f);
            transform.position = playerObject.transform.position + offset + Vector3.up * speedOffset;

            if (GameManager.current.run)
            {
                transform.position = new Vector3(xStart, transform.position.y, transform.position.z);
            }
        }
    }
}
