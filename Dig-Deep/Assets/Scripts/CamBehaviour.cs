using UnityEngine;

public class CamBehaviour : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject playerObject = null;
    [SerializeField]
    private Player player = null;
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
    }
    
    void Update()
    {
        /*if (player.GetComponent<PlayerAnimation>().cameraCenter)
        {
            speedOffset = 0;
            transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, -10f);
            offset = new Vector3(0, 0, -10f);
            player.GetComponent<PlayerAnimation>().cameraCenter = false;

        }*/
        if (!playerObject.GetComponent<PlayerAnimation>().cameraStop)
        {
            if (GameManager.current.run)
                speedOffset = Mathf.Lerp(speedOffset, player.correctPerSecond * GameManager.current.speedFeedback, GameManager.current.speedFeedbackResponseTime / 100f);
            else
                speedOffset = 0;

            transform.position = playerObject.transform.position + offset + Vector3.up * speedOffset;

            if (GameManager.current.run)
            {
                transform.position = new Vector3(xStart, transform.position.y, transform.position.z);
            }
        }
    }
}
