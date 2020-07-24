using UnityEngine;

/// <summary> This class handles the camera movement according to the players speed. </summary>
public class CamBehaviour : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject playerObject = null;
    private Player player;
    private PlayerAnimation playerAnim;
    private Vector3 offset;

    [SerializeField]
    private float speedOffset = 0;
    [SerializeField]
    private float xStart = 0;
    #endregion

    void Start()
    {
        xStart = transform.position.x;
        offset = transform.position - playerObject.transform.position;
        player = playerObject.GetComponent<Player>();
        playerAnim = playerObject.GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        //TODO check if needed:
        /*if (player.GetComponent<PlayerAnimation>().cameraCenter)
        {
            speedOffset = 0;
            transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, -10f);
            offset = new Vector3(0, 0, -10f);
            player.GetComponent<PlayerAnimation>().cameraCenter = false;

        }*/
        if (!playerAnim.cameraStop)
        {
            if (GameManager.current.run)
            {
                speedOffset = Mathf.Lerp(speedOffset, player.correctPerSecond * GameManager.current.speedFeedback, GameManager.current.speedFeedbackResponseTime / 100f);
            }
            else
            {
                speedOffset = 0;
            }

            transform.position = playerObject.transform.position + offset + Vector3.up * speedOffset;

            if (GameManager.current.run)
            {
                transform.position = new Vector3(xStart, transform.position.y, transform.position.z);
            }
        }
    }
}