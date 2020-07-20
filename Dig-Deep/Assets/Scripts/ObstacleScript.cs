using UnityEngine;

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