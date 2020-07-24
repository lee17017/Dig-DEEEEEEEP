using UnityEngine;
using UnityEngine.UI;

/// <summary> This class updates the fillamount of the clock. </summary>
[RequireComponent(typeof(Image))]
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