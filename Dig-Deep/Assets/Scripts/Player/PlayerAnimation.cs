﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> This class handles the look / animation of the player and the update of the background tiles. </summary>
[RequireComponent(typeof(Player))]
public class PlayerAnimation : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float pSpeed;
    public bool obstacle;
    public bool cameraStop = false, cameraCenter = false;

    [SerializeField]
    private int distance;
    [SerializeField]
    private GameObject textPref = null;
    [SerializeField]
    private GameObject winSprite = null, loseSprite = null;
    private Player player;
    [SerializeField]
    private Tiling act = null, next = null;
    [SerializeField]
    private GameObject darkEarth = null;
    private bool switching = false, switching2 = false;
    private float animSpeed = 4;

    [SerializeField]
    private RuntimeAnimatorController[] drillAnims_normal = null;
    [SerializeField]
    private RuntimeAnimatorController drillAnim_special = null;
    #endregion

    #region Lifecycle
    void Start()
    {
        player = GetComponent<Player>();
        //Change Animation Controller if a Player is using Easter Egg model
        bool special = player.GetId() == 1 ? GameManager.current.p1EasterEgg : GameManager.current.p2EasterEgg;
        RuntimeAnimatorController controller = special ?
            drillAnim_special : drillAnims_normal[Random.Range(0, drillAnims_normal.Length)];

        GetComponentInChildren<Animator>().runtimeAnimatorController = controller;
    }

    void Update()
    {
        HandleMovementSpeed();
        HandleTileChange();
    }

    /// <summary> Handles the current movement speed of the player. </summary>
    private void HandleMovementSpeed()
    {
        if (GameManager.current.run)
        {
            if (!obstacle)
            {
                pSpeed = GameManager.current.baseSpeed + player.correctPerSecond * GameManager.current.speedEffect;
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
    }

    /// <summary> Handles updating the background tiles according to player movement. </summary>
    private void HandleTileChange()
    {
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
    #endregion

    /// <summary> Animates the drill at the end of the game, displays the stats and starts the Highscore scene. </summary>
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
            transform.Rotate(new Vector3(0, 0, player.GetId() == 1 ? -3 : 3));
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < distance / 2; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        GameObject temp;
        if ((player.GetId() == 1 && GameManager.current.winnerScore == GameManager.current.playerScore1) || (player.GetId() == 2 && GameManager.current.winnerScore == GameManager.current.playerScore2))
        {
            temp = Instantiate(winSprite);
        }
        else
        {
            temp = Instantiate(loseSprite);
        }
        temp.transform.position = new Vector3(transform.position.x + (player.GetId() == 1 ? -2.2f : 2.2f), transform.position.y - 0.5f, 0.5f);

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
        temp.GetComponent<TextMesh>().text = "" + (player.GetId() == 1 ? GameManager.current.playerScore1 : GameManager.current.playerScore2) + "m";
        temp.transform.position = new Vector3(x, transform.position.y, 0.5f);
        for (int i = 0; i < distance * 5; i++)
        {
            transform.Translate(Vector3.down * Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
    }

    #region Helpers Tileswitching
    private void SwitchField()
    {
        next.numb = player.GetId() * 10 + 1;
    }

    private void SwitchField2()
    {
        switching2 = true;
        act.numb = player.GetId() * 10 + 2;
    }

    /// <summary> Moves the old background tile in front of the player and resets it. </summary>
    private void SwitchNext()
    {
        switching = true;
        act.transform.position = next.transform.position - Vector3.up * 32;
        Tiling temp = act;
        act = next;
        next = temp;
        next.Init();
        act.RemoveShit();
        darkEarth.transform.Translate(Vector3.down * 32);
    }

    /// <summary> Calculates the positive modulo (a%b). </summary>
    /// <returns> The positive modulo </returns>
    private int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }
    #endregion
}