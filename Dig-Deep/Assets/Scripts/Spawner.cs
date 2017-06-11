using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public int player;

    public GameObject[] spawner;
    public GameObject[] obstacles;
    public GameObject warningMiddle, warningLeft, warningRight;
    // Use this for initialization
    private float spawnTimeCur;

    private float xStart;

    public float spawnTime;

    private void Start()
    {
        spawnTimeCur = spawnTime;
        xStart = transform.position.x;
    }
    // Update is called once per frame
    void Update () {
        transform.position = new Vector3(xStart, transform.position.y, transform.position.z);

        if (spawnTimeCur < 0)
        {
            spawnTimeCur = Rn(1, 4);
            StartCoroutine(spawnObject(Rn(spawner.Length-1), Rn(obstacles.Length - 1)));
        }

        spawnTimeCur -= Time.deltaTime;
	}

    int Rn(int max)
    {
        return Random.Range(0, max + 1);
    }

    int Rn(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    IEnumerator spawnObject(int spawn, int obst)
    {
        StartCoroutine(warning(spawn));
        GameObject obstacle = Instantiate(obstacles[obst]);
        obstacle.transform.position = spawner[spawn].transform.position;
        yield return null;
    }

    IEnumerator warning(int spawn)
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
