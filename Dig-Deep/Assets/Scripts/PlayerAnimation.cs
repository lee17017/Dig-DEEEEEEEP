using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimation : MonoBehaviour
{
    public float pSpeed;
    public bool obstacle;
    public bool cameraStop = false, cameraCenter = false;

    public int distance;
    public GameObject textPref;
    public GameObject winSprite, loseSprite;
    public int playerNr;
    public GameObject act, next, darkEarth;
    private bool switching = false, switching2 = false;
    private float animSpeed = 4;

    [SerializeField]
    private RuntimeAnimatorController[] drillAnims_normal = null;
    [SerializeField]
    private RuntimeAnimatorController drillAnim_special = null;

    void Start()
    {
        //StartCoroutine(Animation());

        //Change Animation Controller if a Player is using Easter Egg model
        switch (playerNr)
        {
            case 1:
                if (GameManager.current.Player1EasterEgg)
                {
                    GetComponentInChildren<Animator>().runtimeAnimatorController = drillAnim_special;
                }
                else
                {
                    GetComponentInChildren<Animator>().runtimeAnimatorController = drillAnims_normal[Random.Range(0, drillAnims_normal.Length)];
                }
                break;
            case 2:
                if (GameManager.current.Player2EasterEgg)
                {
                    GetComponentInChildren<Animator>().runtimeAnimatorController = drillAnim_special;
                }
                else
                {
                    GetComponentInChildren<Animator>().runtimeAnimatorController = drillAnims_normal[Random.Range(0, drillAnims_normal.Length)];
                }
                break;
        }
    }

    void Update()
    {
        if (GameManager.current.run)
        {
            if (!obstacle)
            {
                pSpeed = GameManager.current.baseSpeed + GetComponent<Player>().correctPerSecond * GameManager.current.speedEffect;
            }
            else
            {
                pSpeed = 0;
            }

            transform.Translate(Vector3.down * Time.deltaTime * pSpeed);
        }
        else
        {
            pSpeed = GameManager.current.baseSpeed;
        }

        if ((Mod((int)transform.position.y, 32) <= 16 && !switching))
        {
            SwitchNext();
        }

        if (Mod((int)(transform.position.y), 32) <= 1)
        {
            SwitchField();
        }

        if (Mod((int)(transform.position.y), 32) >= 31 && !switching2)
        {
            SwitchField2();
        }

        if (switching && (Mod((int)transform.position.y, 32) >= 26))
        {
            switching = false;
        }

        if (switching2 && (Mod((int)transform.position.y, 32) >= 20))
        {
            switching2 = false;
        }
    }

    public IEnumerator EndAnimation()//TODO fix weird animation if pre-rotated
    {
        cameraCenter = true;
        yield return new WaitForSeconds(1);
        //Move
        for (int i = 0; i < distance; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        //Rotate
        for (int i = 0; i < 30; i++)
        {
            transform.Rotate(new Vector3(0, 0, playerNr == 1 ? -3 : 3));
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < distance / 2; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        GameObject temp;
        if ((playerNr == 1 && GameManager.current.winnerScore == GameManager.current.playerScore1) || (playerNr == 2 && GameManager.current.winnerScore == GameManager.current.playerScore2))
        {
            temp = Instantiate(winSprite);
        }
        else
        {
            temp = Instantiate(loseSprite);
        }
        temp.transform.position = new Vector3(transform.position.x + (playerNr == 1 ? -2.2f : 2.2f), this.transform.position.y - 0.5f, 0.5f);

        for (int i = 0; i < distance - 5; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        cameraStop = true;
        float x = transform.position.x;

        for (int i = 0; i < distance * 2f; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - 3);
        transform.Rotate(new Vector3(0, 0, 180));

        temp = Instantiate(textPref);
        temp.GetComponent<TextMesh>().text = "" + (playerNr == 1 ? GameManager.current.playerScore1 : GameManager.current.playerScore2) + "m";
        temp.transform.position = new Vector3(x, transform.position.y, 0.5f);
        for (int i = 0; i < distance * 5; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
    }

    private void SwitchField()
    {
        next.GetComponent<Tiling>().numb = playerNr * 10 + 1;
    }

    private void SwitchField2()
    {
        switching2 = true;
        act.GetComponent<Tiling>().numb = playerNr * 10 + 2;
    }

    private void SwitchNext()
    {
        switching = true;
        act.transform.position = next.transform.position - Vector3.up * 32;
        GameObject temp = act;
        act = next;
        next = temp;
        next.GetComponent<Tiling>().Init();
        act.GetComponent<Tiling>().RemoveShit();
        darkEarth.transform.Translate(Vector3.down * 32);
    }

    private int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }
}