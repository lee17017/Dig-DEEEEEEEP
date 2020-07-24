using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary> This class enables toggling between two specified sprites. </summary>
[RequireComponent(typeof(Image))]
public class ChangeSprite : MonoBehaviour
{
    #region Variables
    [SerializeField]
    [FormerlySerializedAs("red")]
    private Sprite defaultSprite = null;
    [SerializeField]
    [FormerlySerializedAs("green")]
    private Sprite changedSprite = null;
    private Image spriteImage;
    private GameObject subtitle;
    private bool state = false;
    #endregion

    void Awake()
    {
        spriteImage = GetComponent<Image>();
        if (transform.childCount == 2)
        {
            subtitle = transform.GetChild(1).gameObject;
        }
    }

    /// <summary> Toggles the button sprite between red and green </summary>
    public void ToggleSprite()
    {
        subtitle.SetActive(state);
        state = !state;
        spriteImage.sprite = state ? changedSprite : defaultSprite;
    }
}