using System.Collections;
using UnityEngine;

/// <summary> This class handles switching the lanes. </summary>
public class Tunnelbohralgorithmus : MonoBehaviour
{
    #region Variables
    private float left, mid, right;
    private Player player;
    [SerializeField]
    private int lane = 1;
    private bool switching = false;
    #endregion

    #region Lifecycle
    void Start()
    {
        mid = transform.position.x;
        left = transform.position.x - GameManager.current.laneDistance;
        right = transform.position.x + GameManager.current.laneDistance;

        player = GetComponent<Player>();
    }

    void Update()
    {
        if (GameManager.current.run && !switching)
        {
            if (Input.GetKeyDown("joystick " + player.GetId() + " button 4") || (GameManager.current.tastatur && Input.GetMouseButtonDown(0)))
            {
                StartCoroutine(SwitchLane(true));
            }
            if (Input.GetKeyDown("joystick " + player.GetId() + " button 5") || (GameManager.current.tastatur && Input.GetMouseButtonDown(1)))
            {
                StartCoroutine(SwitchLane(false));
            }
        }
    }
    #endregion

    /// <summary> Animates a lane switch in the specified direction. </summary>
    /// <param name="left"> The switch direction </param>
    private IEnumerator SwitchLane(bool left)
    {
        if ((left && lane != 0) || (!left && lane != 2))
        {
            if (left) { lane -= 1; } else { lane += 1; }
            switching = true;
            //Rotate
            for (int i = 0; i < 10; i++)
            {
                transform.Rotate(new Vector3(0, 0, left ? -3 : 3));
                while (player.obstacle)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }

            float goal = mid;
            switch (lane)
            {
                case 0:
                    goal = this.left;
                    break;
                case 1:
                    goal = mid;
                    break;
                case 2:
                    goal = right;
                    break;
            }

            while (!(transform.position.x < goal + 0.1f && transform.position.x > goal - 0.1f))
            {
                while (player.obstacle)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return null;
            }

            for (int i = 0; i < 10; i++)
            {
                while (player.obstacle)
                {
                    yield return new WaitForEndOfFrame();
                }
                transform.Rotate(new Vector3(0, 0, left ? 3 : -3));
                yield return new WaitForEndOfFrame();
            }

            while (!(transform.position.x < goal + 0.1f && transform.position.x > goal - 0.1f))
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(goal, transform.position.y, transform.position.z), 0.2f);
                yield return null;
            }

            switching = false;
        }
    }
}