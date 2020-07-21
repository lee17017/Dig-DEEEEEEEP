using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public UnityEvent p1pressedA, p2pressedA, p1pressedB, p2pressedB, transition;
    private bool p1Done = false, p2Done = false, done = false;

    [SerializeField]
    private int player1EasterEgg = 0;
    private int player2EasterEgg = 0;
    [SerializeField]
    private GameObject drill = null;
    [SerializeField]
    private bool trans = false;

    void Update()
    {
        if (trans)
        {
            done = true;
            StartCoroutine(NextLevel());
        }

        HandleReadyButtons();

        if (p1Done && p2Done)
        {
            done = true;
            StartCoroutine(NextLevel());
        }
        
        HandleEasterEgg();
    }

    private void HandleReadyButtons()
    {
        if (Input.GetKeyDown("joystick 1 button 0") && !p1Done && !done && player1EasterEgg != 7)
        {
            p1pressedA.Invoke();
            p1Done = !p1Done;
        }

        if (Input.GetKeyDown("joystick 2 button 0") && !p2Done && !done && player2EasterEgg != 7)
        {
            p2pressedA.Invoke();
            p2Done = !p2Done;
        }

        if (Input.GetKeyDown("joystick 1 button 1") && p1Done && !done)
        {
            p1pressedB.Invoke();
            p1Done = !p1Done;
        }

        if (Input.GetKeyDown("joystick 2 button 1") && p2Done && !done)
        {
            p2pressedB.Invoke();
            p2Done = !p2Done;
        }
    }

    private void HandleEasterEgg()
    {
        switch (player1EasterEgg)
        {
            case 0:
                if (Input.GetAxis("Vertical 1") > 0)
                    player1EasterEgg++;
                break;
            case 1:
                if (Input.GetAxis("Vertical 1") < 0)
                    player1EasterEgg++;
                break;
            case 2:
                if (Input.GetAxis("Horizontal 1") < 0)
                    player1EasterEgg++;
                break;
            case 3:
                if (Input.GetAxis("Horizontal 1") > 0)
                    player1EasterEgg++;
                break;
            case 4:
                if (Input.GetAxis("Horizontal 1") < 0)
                    player1EasterEgg++;
                break;
            case 5:
                if (Input.GetAxis("Horizontal 1") > 0)
                    player1EasterEgg++;
                break;
            case 6:
                if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                    player1EasterEgg++;
                break;
            case 7:
                if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                    player1EasterEgg++;
                break;
            case 8:
                if (Input.GetKeyDown(KeyCode.Joystick1Button7))
                    GameManager.current.Player1EasterEgg = true;
                break;
        }
        switch (player2EasterEgg)
        {
            case 0:
                if (Input.GetAxis("Vertical 2") > 0)
                    player2EasterEgg++;
                break;
            case 1:
                if (Input.GetAxis("Vertical 2") < 0)
                    player2EasterEgg++;
                break;
            case 2:
                if (Input.GetAxis("Horizontal 2") < 0)
                    player2EasterEgg++;
                break;
            case 3:
                if (Input.GetAxis("Horizontal 2") > 0)
                    player2EasterEgg++;
                break;
            case 4:
                if (Input.GetAxis("Horizontal 2") < 0)
                    player2EasterEgg++;
                break;
            case 5:
                if (Input.GetAxis("Horizontal 2") > 0)
                    player2EasterEgg++;
                break;
            case 6:
                if (Input.GetKeyDown(KeyCode.Joystick2Button1))
                    player2EasterEgg++;
                break;
            case 7:
                if (Input.GetKeyDown(KeyCode.Joystick2Button0))
                    player2EasterEgg++;
                break;
            case 8:
                if (Input.GetKeyDown(KeyCode.Joystick2Button7))
                    GameManager.current.Player2EasterEgg = true;
                break;
        }
    }

    IEnumerator NextLevel()
    {
        drill.GetComponent<MoveSprite>().MoveDrill();
        yield return new WaitForSeconds(2);
        transition.Invoke();

        yield return new WaitForSeconds(8);
        SceneManager.LoadScene(1);
    }
}