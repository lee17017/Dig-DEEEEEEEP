using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    #region Variables
    [SerializeField]
    [Tooltip("Default sprite")]
    private Sprite red = null;
    [SerializeField]
    [Tooltip("Toggled sprite")]
    private Sprite green = null;
    private Image spriteImage;
    public Text pressA;//TODO check if it could be found programmatically
    private bool state = false; /* false = red true=green*/
    #endregion

    void Awake()
    {
        spriteImage = GetComponent<Image>();
    }

    /// <summary> Toggles the button sprite between red and green </summary>
    public void ToggleSprite()
    {
        pressA.gameObject.SetActive(state);
        state = !state;
        spriteImage.sprite = state ? green : red;
    }
}