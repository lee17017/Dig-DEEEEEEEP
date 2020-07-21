using UnityEngine;

public class MashButton : MonoBehaviour
{
    public int player;
    private SpriteRenderer rend;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!GameManager.current.run)
        {
            Destroy(gameObject);
        }
        else if (Input.GetKey("joystick " + player + " button 0"))
        {
            rend.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            rend.color = new Color(1, 1, 1, 1);
        }
    }
}