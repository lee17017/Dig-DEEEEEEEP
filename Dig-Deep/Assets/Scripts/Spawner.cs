using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int player;

    public GameObject[] spawner;
    public GameObject[] obstacles;
    public GameObject warningMiddle, warningLeft, warningRight;

    private float spawnTimeCur;
    private float xStart;
    public float spawnTime;

    private void Start()
    {
        spawnTimeCur = spawnTime;
        xStart = transform.position.x;
    }

    void Update()
    {
        if (!transform.parent.GetComponent<Player>().obstacle && GameManager.current.run)
        {
            transform.position = new Vector3(xStart, transform.position.y, transform.position.z);

            if (spawnTimeCur < 0)
            {
                spawnTimeCur = Rand(1, 4);
                StartCoroutine(SpawnObject(Rand(spawner.Length - 1), Rand(obstacles.Length - 1)));
            }

            spawnTimeCur -= Time.deltaTime;
        }
    }

    private int Rand(int max)
    {
        return Random.Range(0, max + 1);
    }

    private int Rand(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    IEnumerator SpawnObject(int spawn, int obst)
    {
        StartCoroutine(Warning(spawn));
        GameObject obstacle = Instantiate(obstacles[obst]);
        obstacle.transform.position = spawner[spawn].transform.position;
        yield return null;
    }

    IEnumerator Warning(int spawn)
    {
        GameObject warningObj = null;

        switch (spawn)
        {
            case 0:
                warningObj = warningMiddle;
                break;
            case 1:
                warningObj = warningLeft;
                break;
            case 2:
                warningObj = warningRight;
                break;
        }

        warningObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(1);

        warningObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}