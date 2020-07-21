using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    void Awake()
    {
        GameManager.current.canvas = gameObject.GetComponent<Canvas>();
        GameManager.current.gameDuration = GameManager.current.totalDuration;
    }
}