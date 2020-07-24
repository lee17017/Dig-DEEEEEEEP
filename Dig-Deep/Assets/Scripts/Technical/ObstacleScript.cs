using UnityEngine;

/// <summary> This script destroys its gameobject if the user leaves the game scene. </summary>
public class ObstacleScript : MonoBehaviour
{
    void Update()
    {
        if (!GameManager.current.run)
        {
            Destroy(gameObject);
        }
    }
}