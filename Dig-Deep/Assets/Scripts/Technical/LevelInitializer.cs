using UnityEngine;

/// <summary> This class initializes the GameManager at the start of every level. </summary>
public class LevelInitializer : MonoBehaviour
{
    void Awake()
    {
        GameManager.current.canvas = gameObject.GetComponent<Canvas>();
        GameManager.current.gameDuration = GameManager.current.totalDuration;
    }
}