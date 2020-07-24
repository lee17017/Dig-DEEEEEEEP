using UnityEngine;

/// <summary> This class updates the visibility of a given gameobject. </summary>
public class ShowGameOver : MonoBehaviour
{
    public GameObject gOver;
    private bool display = false;

    private void Awake()
    {
        gOver.SetActive(false);
    }
    
    void Update()
    {
        if (!GameManager.current.run && !display)
        {
            gOver.SetActive(true);
            display = true;
        }
    }
}