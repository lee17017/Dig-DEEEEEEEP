using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] spawner;
    public GameObject[] obstacles;
    // Use this for initialization
    private float spawnTimeCur;

    public float spawnTime;

    private void Start()
    {
        spawnTimeCur = spawnTime;
    }
    // Update is called once per frame
    void Update () {
        if (spawnTimeCur < 0)
        {
            spawnTimeCur = Rn(4, 7);
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
        GameObject obstacle = Instantiate(obstacles[obst]);
        obstacle.transform.position = spawner[spawn].transform.position;
        yield return null;
    }
}
