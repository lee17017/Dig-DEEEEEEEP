using UnityEngine;
using UnityEngine.UI;

public class Clockcode : MonoBehaviour
{
    private Image clockImage;

    void Awake()
    {
        clockImage = GetComponent<Image>();
    }

    void Update()
    {
        clockImage.fillAmount = GameManager.current.clockPercentage;
    }
}