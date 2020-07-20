using System.Collections;
using UnityEngine;

public class Tunnelbohralgorithmus : MonoBehaviour
{
    public float left, mid, right;
    public int spielerNr;
    public int lane = 1;
    bool switching = false;
    
    void Start()
    {
        mid = transform.position.x;
        left = transform.position.x - GameManager.current.laneDistance;
        right = transform.position.x + GameManager.current.laneDistance;
    }

    void Update()
    {
        if (GameManager.current.run && !switching)
        {
            if (Input.GetKeyDown("joystick " + spielerNr + " button 4") || (GameManager.current.tastatur && Input.GetMouseButtonDown(0)))
            {
                StartCoroutine(switchLane(-1));
            }
            if (Input.GetKeyDown("joystick " + spielerNr + " button 5") || (GameManager.current.tastatur && Input.GetMouseButtonDown(1)))
            {
                StartCoroutine(switchLane(1));
            }
        }
    }

    IEnumerator switchLane(int direction)
    {
        if ((direction < 0 && lane != 0) || (direction > 0 && lane != 2))
        {
            if (direction < 0) { lane -= 1; } else { lane += 1; }
            switching = true;
            //Rotate
            for (int i = 0; i < 10; i++)
            {
                transform.Rotate(new Vector3(0, 0, direction < 0 ? -3 : 3));
                while (GetComponent<Player>().obstacle)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }

            float goal = mid;

            switch (lane)
            {
                case 0:
                    goal = left;
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
                while (GetComponent<Player>().obstacle)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return null;
            }

            for (int i = 0; i < 10; i++)
            {
                while (GetComponent<Player>().obstacle)
                {
                    yield return new WaitForEndOfFrame();
                }
                transform.Rotate(new Vector3(0, 0, direction < 0 ? 3 : -3));
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